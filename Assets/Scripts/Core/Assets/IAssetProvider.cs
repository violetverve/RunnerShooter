using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Core.Assets
{
    public interface IAssetProvider
    {
        /// <summary>
        /// Loads the asset behind the reference, or returns the cached one if it was loaded before.
        /// </summary>
        UniTask<T> LoadAsync<T>(AssetReference reference, CancellationToken cancellationToken) where T : Object;

        /// <summary>
        /// Loads the asset at the Addressables address, or returns the cached one if it was loaded before.
        /// </summary>
        UniTask<T> LoadAsync<T>(string address, CancellationToken cancellationToken) where T : Object;
    }
}
