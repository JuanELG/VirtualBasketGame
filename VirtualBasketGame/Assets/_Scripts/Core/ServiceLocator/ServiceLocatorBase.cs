using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketGame.Core
{
    public class ServiceLocatorBase : MonoBehaviour, IServiceLocator
    {
        [SerializeField] private List<Installer> _installers = new List<Installer>();

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        protected virtual void Awake()
        {
            foreach (Installer service in _installers)
                service.Install();
        }

        public T GetService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out object service))
            {
                return service as T;
            }

            return null;
        }

        public void RegisterService<T>(T service)
        {
            if (service == null)
            {
                Debug.LogError($"The service {typeof(T)} can't be null");
                return;
            }

            Type serviceType = typeof(T);

            if (_services.ContainsKey(serviceType))
            {
                Debug.LogWarning("There is already a registered service for the type " + serviceType.Name);
                return;
            }

            _services.Add(serviceType, service);
        }
    }
}