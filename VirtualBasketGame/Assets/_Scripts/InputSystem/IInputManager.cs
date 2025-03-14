using System;
using UnityEngine;

public interface IInputManager
{
    event Action AimStarted;
    event Action AimCanceled;
    Vector2 AimVector { get; }
}