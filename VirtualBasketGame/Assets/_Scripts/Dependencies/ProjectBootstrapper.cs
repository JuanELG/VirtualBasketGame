using UnityEngine;
using BasketGame.Core;

namespace BasketGame.Dependencies
{
    public class ProjectBootstrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeProjectContext()
        {
            GameObject prefab = Resources.Load<GameObject>("ProjectContext");
            if (prefab == null)
            {
                Debug.LogError("[ProjectBootstrapper] ProjectContext prefab not found in Resources.");
                return;
            }
            GameObject instanceGO = Instantiate(prefab);
            ServiceLocatorProject.Initialize(instanceGO.GetComponent<ServiceLocatorProject>());
            DontDestroyOnLoad(instanceGO);
            Debug.Log("[ProjectBootstrapper] ProjectContext instantiated via Bootstrapper.");
        }
    }
}