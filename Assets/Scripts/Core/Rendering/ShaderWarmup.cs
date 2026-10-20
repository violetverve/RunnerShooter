using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using UnityEngine;

namespace RunnerShooter.Core.Rendering
{
    public sealed class ShaderWarmup : IShaderWarmup
    {
        private const int VariantsPerFrame = 8;
        private const int IdleFramesToSettle = 3;
        private const float CompilationTimeoutSeconds = 30f;

        private readonly IAssetProvider m_AssetProvider;

        public ShaderWarmup(IAssetProvider assetProvider)
        {
            m_AssetProvider = assetProvider;
        }

        public async UniTask WarmUpAsync(CancellationToken cancellationToken)
        {
            ShaderVariantCollection variants = await m_AssetProvider.LoadAsync<ShaderVariantCollection>(
                RenderingConstants.ShaderVariantsAddress, cancellationToken);

            if (variants.isWarmedUp)
            {
                return;
            }

#if UNITY_EDITOR
            bool allowAsyncCompilation = UnityEditor.ShaderUtil.allowAsyncCompilation;
            UnityEditor.ShaderUtil.allowAsyncCompilation = false;
#endif
            try
            {
                while (!variants.WarmUpProgressively(VariantsPerFrame))
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
            }
            finally
            {
#if UNITY_EDITOR
                UnityEditor.ShaderUtil.allowAsyncCompilation = allowAsyncCompilation;
#endif
            }
        }

        public async UniTask WaitForCompilationAsync(CancellationToken cancellationToken)
        {
#if UNITY_EDITOR
            float deadline = Time.realtimeSinceStartup + CompilationTimeoutSeconds;
            int idleFrames = 0;

            while (idleFrames < IdleFramesToSettle && Time.realtimeSinceStartup < deadline)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                idleFrames = UnityEditor.ShaderUtil.anythingCompiling ? 0 : idleFrames + 1;
            }
#else
            await UniTask.CompletedTask;
#endif
        }
    }
}
