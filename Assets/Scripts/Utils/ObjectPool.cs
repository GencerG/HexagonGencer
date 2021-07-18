using System.Collections.Generic;
using UnityEngine;

namespace HexagonGencer.Utils
{
    public static class ObjectPool
    {

        private static Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
        private const string instancesOptionalNameEnding = "(Pool)";

        public static void PreLoadInstances(GameObject prefab, int number, Transform parent = null)
        {
            GameObject prefabInstance;
            bool setParent = parent != null;
            for (int i = 0; i < number; ++i)
            {
                prefabInstance = Instantiate(prefab);
                if (setParent)
                    prefabInstance.transform.SetParent(parent);

                StoreInstance(prefabInstance);
            }
        }

        public static GameObject GetInstance(GameObject prefab, Transform parent = null)
        {
            GameObject prefabInstance = GetInstanceFromPool(prefab);
            if (prefabInstance == null)
            {
                prefabInstance = Instantiate(prefab);
            }

            GameObject gameObjectInstance = prefabInstance.gameObject;
            gameObjectInstance.SetActive(true);

            if (parent != null)
                gameObjectInstance.transform.SetParent(parent);

            return gameObjectInstance;
        }

        public static void StoreInstance(GameObject gameObjectInstance)
        {
            gameObjectInstance.gameObject.SetActive(false);
            List<GameObject> instancesList;

            if (pool.TryGetValue(gameObjectInstance.name, out instancesList))
            {
                instancesList.Add(gameObjectInstance);
            }
            else
            {
                instancesList = new List<GameObject>();
                instancesList.Add(gameObjectInstance);
                pool.Add(gameObjectInstance.name, instancesList);
            }
        }

        public static void Clear()
        {
            
            foreach(KeyValuePair<string, List<GameObject>> entry in pool)
            {
                foreach(GameObject obj in entry.Value)
                {
                    GameObject.Destroy(obj);
                }
            }
            
            pool.Clear();
        }

        private static GameObject GetInstanceFromPool(GameObject prefab)
        {
            GameObject prefabInstance = null;
            List<GameObject> instancesList;
            if (pool.TryGetValue(GeneratePrefabInstancesName(prefab), out instancesList))
            {
                if (instancesList.Count != 0)
                {
                    prefabInstance = instancesList[0];
                    instancesList.RemoveAt(0);
                }
            }

            return prefabInstance;
        }

        private static GameObject Instantiate(GameObject prefab)
        {
            GameObject prefabInstance = Object.Instantiate(prefab);
            prefabInstance.name = GeneratePrefabInstancesName(prefab);
            return prefabInstance;
        }

        private static string GeneratePrefabInstancesName(GameObject prefab)
        {
            return prefab.name + prefab.GetInstanceID() + instancesOptionalNameEnding;
        }
    }
}
