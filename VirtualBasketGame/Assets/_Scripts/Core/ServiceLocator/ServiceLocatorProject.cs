using UnityEngine;

namespace BasketGame.Core
{
    public class ServiceLocatorProject : ServiceLocatorBase, IServiceLocator
    {
        private static ServiceLocatorProject _instance;
        public static IServiceLocator Instance => _instance;

        public static void Initialize(ServiceLocatorProject instance)
        {
            if (instance == null)
            {
                Debug.LogError("Cannot initialize ServiceLocatorProject with a null instance.");
                return;
            }
            _instance = instance;
        }

        protected override void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}