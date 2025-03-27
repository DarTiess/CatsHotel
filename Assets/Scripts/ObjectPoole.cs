using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectPoole<T> where T: Component
    {
        private List<T> _pool;
        private int _size;

        public void CreatePool(T prefab, int size, Transform parent)
        {
            _size = size;
            _pool = new List<T>(size);

            for (int i = 0; i < size; i++)
            {
                var obj = Object.Instantiate(prefab, parent, true);
                obj.gameObject.SetActive(false);
                _pool.Add(obj);
            }
        }
        public void CreatePool(List<T> prefab, int size, Transform parent)
        {
            
            _size = size;
            _pool = new List<T>(size);

            for (int i = 0; i < size; i++)
            {
                int rnd = Random.Range(0, prefab.Count);
                var obj = Object.Instantiate(prefab[rnd], parent, true);
                obj.gameObject.SetActive(false);
                _pool.Add(obj);
            }
        }

        public T GetObject()
        {
            foreach (T obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
            return null;
        }

    }
}