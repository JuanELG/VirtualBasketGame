using System;
using UnityEngine;

namespace BasketGame.Core
{
    public interface IInputManager
    {
        event Action AimStarted;
        event Action<Vector2> AimPerformed;
        event Action AimCanceled;
        Vector2 AimVector { get; }
    }
}