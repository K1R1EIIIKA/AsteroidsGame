using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.Pools
{
    public class MonoPool<T> : IPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly int _initialSize;
        private Transform _parent;
        private readonly Queue<T> _queue = new Queue<T>();
        private bool _initialized;

        public MonoPool(T prefab, int initialSize)
        {
            _prefab = prefab;
            _initialSize = initialSize;
        }

        public void Initialize(Transform parent)
        {
            if (_initialized) return;
            _parent = parent;
            _initialized = true;

            for (int i = 0; i < _initialSize; i++)
                Return(CreateNew());
        }

        public T Get()
        {
            var item = _queue.Count > 0 ? _queue.Dequeue() : CreateNew();
            return item;
        }

        public void Return(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(_parent);
            _queue.Enqueue(item);
        }

        private T CreateNew()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}
