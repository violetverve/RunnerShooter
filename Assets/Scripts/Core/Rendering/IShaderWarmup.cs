using System.Threading;
using Cysharp.Threading.Tasks;

namespace RunnerShooter.Core.Rendering
{
    public interface IShaderWarmup
    {
        /// <summary>
        /// Loads the gameplay shader variant collection from Addressables and compiles it, spread across frames.
        /// </summary>
        UniTask WarmUpAsync(CancellationToken cancellationToken);

        /// <summary>
        /// In the editor, completes once asynchronous shader compilation has settled; completes immediately in builds.
        /// </summary>
        UniTask WaitForCompilationAsync(CancellationToken cancellationToken);
    }
}
