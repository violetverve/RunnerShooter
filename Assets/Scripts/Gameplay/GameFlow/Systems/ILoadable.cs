using System.Threading;
using Cysharp.Threading.Tasks;

namespace RunnerShooter.Gameplay.GameFlow
{
    public interface ILoadable
    {
        /// <summary>
        /// Loads prefabs and warms pools. The game leaves Loading once every loadable has finished.
        /// </summary>
        UniTask LoadAsync(CancellationToken cancellationToken);
    }
}
