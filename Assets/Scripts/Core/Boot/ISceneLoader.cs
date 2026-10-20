using Cysharp.Threading.Tasks;

namespace RunnerShooter.Core.Boot
{
    public interface ISceneLoader
    {
        /// <summary>
        /// Loads a scene additively as a child of the Boot container and makes it active.
        /// </summary>
        UniTask LoadAsync(string sceneName);

        /// <summary>
        /// Unloads the scene loaded last and loads it again, so every object in it starts fresh.
        /// </summary>
        UniTask ReloadAsync();
    }
}
