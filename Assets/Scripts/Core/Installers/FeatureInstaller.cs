using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using Zenject;

namespace RunnerShooter.Core.Installers
{
    /// <summary>
    /// A feature's bindings, installed from code by the scene's installer. Zenject's own Installer&lt;T&gt;
    /// cannot await an asset load before binding, so features preload here first.
    /// </summary>
    public abstract class FeatureInstaller
    {
        public virtual UniTask PreloadAsync(IAssetProvider assetProvider, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public abstract void Install(DiContainer container);
    }
}
