using BasketGame.InputSystem;
using BasketGame.Core;
using UnityEngine;


namespace BasketGame.Dependencies
{
    public class InputSystemInstaller : Installer
    {
        [SerializeField] private InputManager inputManager;

        public override void Install()
        {
            ServiceLocatorProject.Instance.RegisterService<IInputManager>(inputManager);
        }
    }
}