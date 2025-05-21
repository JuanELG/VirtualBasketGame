using BasketGame.Core;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody ballRigidbody; 
    [SerializeField] private float baseForwardForce = 10f;
    [SerializeField] private float horizontalMultiplier = 5f;
    [SerializeField] private float verticalMultiplier = 10f;

    private IInputManager inputManager;
    private Vector2 lastAimValue;

    private void Awake()
    {
        inputManager = ServiceLocatorProject.Instance.GetService<IInputManager>();
        if (inputManager == null)
            Debug.LogError("[PlayerController][Awake] Input Manager not found.");
    }

    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.AimStarted += OnAimStarted;
            inputManager.AimPerformed += OnAimPerformed;
            inputManager.AimCanceled += OnAimCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.AimStarted -= OnAimStarted;
            inputManager.AimPerformed -= OnAimPerformed;
            inputManager.AimCanceled -= OnAimCanceled;
        }
    }

    private void OnAimStarted()
    {
        Debug.Log("[PlayerController][OnAimStarted] Aim started");
    }

    private void OnAimPerformed(Vector2 aim)
    {
        lastAimValue = aim;
    }

    private void OnAimCanceled()
    {
        Debug.Log("[PlayerController][OnAimCanceled] Aim canceled");
        LaunchBall(lastAimValue);
    }

    private void LaunchBall(Vector2 aim)
    {
        ballRigidbody.constraints = RigidbodyConstraints.None;
        float horizontal = aim.x * horizontalMultiplier;
        float vertical = (aim.y < 0 ? -aim.y : 0) * verticalMultiplier;
        float forward = baseForwardForce;
        Vector3 finalForce = new Vector3(horizontal, vertical, forward);
        ballRigidbody.AddForce(finalForce, ForceMode.Impulse);
        Debug.Log($"[PlayerController][OnAimCanceled] Ball launched with force: {finalForce}");
    }
}