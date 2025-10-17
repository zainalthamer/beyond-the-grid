using UnityEngine;
using Cinemachine;

public class CinemachineFocusToggle : MonoBehaviour
{
    public CinemachineVirtualCamera monitorCam;
    public Behaviour playerController;  // drag ThirdPersonController or your player movement
    public Behaviour selector;          // drag Pixel Crushers Selector
    private bool isFocused = false;

    public void ToggleFocus()
    {
        isFocused = !isFocused;
        monitorCam.Priority = isFocused ? 20 : 0;

        if (playerController) playerController.enabled = !isFocused;
        if (selector) selector.enabled = !isFocused;
    }
}
