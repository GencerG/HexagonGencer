using UnityEngine;

namespace HexagonGencer.Utils
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        #region Fields

        private static T _instance;

        #endregion

        #region Static Instance

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError(typeof(T).ToString() + " is NULL");

                return _instance;
            }
        }

        #endregion

        #region Unity

        private void Awake()
        {
            _instance = this as T;
            Initialize();
        }

        #endregion

        #region Init

        public virtual void Initialize()
        {

        }

        #endregion
    }
}