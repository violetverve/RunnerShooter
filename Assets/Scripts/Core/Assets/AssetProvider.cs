using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RunnerShooter.Core.Assets
{
    public sealed class AssetProvider : IAssetProvider, IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> m_Handles = new();

        public UniTask<T> LoadAsync<T>(AssetReference reference, CancellationToken cancellationToken)
            where T : UnityEngine.Object
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            return LoadCachedAsync<T>(reference.AssetGUID, reference, cancellationToken);
        }

        public UniTask<T> LoadAsync<T>(string address, CancellationToken cancellationToken)
            where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address must not be empty.", nameof(address));
            }

            return LoadCachedAsync<T>(address, address, cancellationToken);
        }

        public void Dispose()
        {
            foreach (AsyncOperationHandle handle in m_Handles.Values)
            {
                Addressables.Release(handle);
            }

            m_Handles.Clear();
        }

        private async UniTask<T> LoadCachedAsync<T>(string cacheKey, object key, CancellationToken cancellationToken)
            where T : UnityEngine.Object
        {
            if (!m_Handles.TryGetValue(cacheKey, out AsyncOperationHandle handle))
            {
                handle = Addressables.LoadAssetAsync<T>(key);
                m_Handles.Add(cacheKey, handle);
            }

            await handle.ToUniTask(cancellationToken: cancellationToken);

            return (T)handle.Result;
        }
    }
}
