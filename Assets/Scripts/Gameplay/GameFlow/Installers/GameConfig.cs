using System;
using System.Collections.Generic;
using RunnerShooter.Core.Installers;
using UnityEngine;

namespace RunnerShooter.Gameplay.GameFlow
{
    [CreateAssetMenu(menuName = "RunnerShooter/Game Config", fileName = "GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [SerializeField]
        private FeatureConfig[] m_Configs = Array.Empty<FeatureConfig>();

        public T Get<T>() where T : FeatureConfig
        {
            T found = null;

            foreach (FeatureConfig config in m_Configs)
            {
                if (config is not T typed)
                {
                    continue;
                }

                if (found != null)
                {
                    throw new InvalidOperationException($"GameConfig has more than one {typeof(T).Name}.");
                }

                found = typed;
            }

            return found != null ? found : throw new InvalidOperationException($"GameConfig has no {typeof(T).Name}.");
        }

        private void OnValidate()
        {
            HashSet<Type> types = new();

            foreach (FeatureConfig config in m_Configs)
            {
                if (config == null)
                {
                    Debug.LogWarning("GameConfig has an empty entry in its config list.", this);
                }
                else if (!types.Add(config.GetType()))
                {
                    Debug.LogWarning($"GameConfig has more than one {config.GetType().Name}.", this);
                }
            }
        }
    }
}
