using System.Threading;
using Cysharp.Threading.Tasks;

namespace RunnerShooter.Core.Boot
{
    public interface ILoadingScreen
    {
        /// <summary>
        /// Loads the loading screen prefab and spawns it fully opaque.
        /// </summary>
        UniTask LoadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Fades the screen in and completes once it fully covers the game.
        /// </summary>
        UniTask ShowAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Fades the screen out to reveal the loaded game, once pending shader compilation has settled.
        /// </summary>
        void Hide();
    }
}
