using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narzioth.Utilities
{
    /// <summary>
    /// Holds a globally accessible reference for any class that inherits this type.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
    #region Variables

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError($"Monosingleton type {typeof(T)} is null! Make sure you only have 1 and it's active in the scene.");
                
                return _instance;
            }
        }

    #endregion
    #region Unity Methods

        void Awake()
        {
            _instance = this as T;
            Initialize();
        }

    #endregion

        /// <summary>
        /// Use as a replacement for Awake().
        /// </summary>
        protected virtual void Initialize()
        {}


    }
}