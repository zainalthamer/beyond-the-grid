using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerMovementLock : MonoBehaviour
{
    private PlayerInput playerInput;
    private StarterAssetsInputs inputScript;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputScript = GetComponent<StarterAssetsInputs>();
    }

    // Call this when dialogue starts
    public void DisableMovement()
    {
        if (playerInput != null)
            playerInput.enabled = false; // stops input from being read

        if (inputScript != null)
            inputScript.move = Vector2.zero; // stops any movement in progress
    }

    // Call this when dialogue ends
    public void EnableMovement()
    {
        if (playerInput != null)
            playerInput.enabled = true; // re-enable input
    }
}
