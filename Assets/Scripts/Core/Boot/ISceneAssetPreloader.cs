using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;

namespace RunnerShooter.Core.Boot
{
    public interface ISceneAssetPreloader
    {
        /// <summary>
        /// Loads the assets an installer binds, before its scene container is built.
        /// </summary>
        UniTask PreloadAsync(IAssetProvider assetProvider, CancellationToken cancellationToken);
    }
}
