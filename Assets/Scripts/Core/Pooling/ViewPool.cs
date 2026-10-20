using System;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace RunnerShooter.Core.Pooling
{
    public sealed class ViewPool<TView> : IDisposable where TView : Component
    {
        private readonly ObjectPool<TView> m_Pool;
        private readonly Transform m_Root;

        public ViewPool(IInstantiator instantiator, TView prefab, Transform root, int prewarmCount)
        {
            m_Root = root;
            m_Pool = new ObjectPool<TView>(
                () => instantiator.InstantiatePrefabForComponent<TView>(prefab, root),
                view => view.gameObject.SetActive(true),
                view => view.gameObject.SetActive(false),
                DestroyView);

            Prewarm(prewarmCount);
        }

        public TView Get()
        {
            return m_Pool.Get();
        }

        public void Release(TView view)
        {
            view.transform.SetParent(m_Root, false);
            m_Pool.Release(view);
        }

        public void Dispose()
        {
            m_Pool.Dispose();
        }

        private static void DestroyView(TView view)
        {
            // On scene unload Unity may destroy pooled views before Zenject disposes the pool.
            if (view != null)
            {
                UnityEngine.Object.Destroy(view.gameObject);
            }
        }

        private void Prewarm(int count)
        {
            TView[] views = new TView[count];

            for (int i = 0; i < count; i++)
            {
                views[i] = m_Pool.Get();
            }

            foreach (TView view in views)
            {
                m_Pool.Release(view);
            }
        }
    }
}
