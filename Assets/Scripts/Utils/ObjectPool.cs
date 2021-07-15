using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Utils
{
    public class ObjectPool
    {
        #region Fields

        private GameObject _prefab;
        private Transform _poolContainer;

        private Queue<GameObject> _availableObjects = new Queue<GameObject>();

        #endregion

        #region Constructer

        public ObjectPool() { }

        public ObjectPool(GameObject prefab, Transform poolContainer)
        {
            this._prefab = prefab;
            this._poolContainer = poolContainer;
        }

        #endregion

        #region Custom Methods

        public void InstantiatePool()
        {
            for (int i = 0; i < 120; i++)
            {
                var objectInstance = GameObject.Instantiate(_prefab);
                objectInstance.transform.SetParent(_poolContainer);
                AddToPool(objectInstance);
            }
        }

        public void AddToPool(GameObject instance)
        {
            instance.SetActive(false);
            _availableObjects.Enqueue(instance);
        }

        public GameObject GetFromPool()
        {
            if (_availableObjects.Count == 0)
                InstantiatePool();

            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }

        #endregion
    }

}