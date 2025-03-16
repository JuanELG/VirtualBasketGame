using BasketGame.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BasketGame.InputSystem
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        public event Action AimStarted;
        public event Action AimCanceled;

        private GameControls gameControl = null;

        public Vector2 AimVector => gameControl.Gameplay.Aim.ReadValue<Vector2>();

        private void Awake()
        {
            gameControl = new GameControls();
        }

        private void OnEnable()
        {
            gameControl.Enable();
            gameControl.Gameplay.Aim.started += RaiseAimStarted;
            gameControl.Gameplay.Aim.canceled += RaiseAimCanceled;
        }

        private void OnDisable()
        {
            gameControl.Disable();
            gameControl.Gameplay.Aim.started -= RaiseAimStarted;
            gameControl.Gameplay.Aim.canceled -= RaiseAimCanceled;
        }

        private void RaiseAimStarted(InputAction.CallbackContext context)
        {
            AimStarted?.Invoke();
        }

        private void RaiseAimCanceled(InputAction.CallbackContext context)
        {
            AimCanceled?.Invoke();
        }
    }
}