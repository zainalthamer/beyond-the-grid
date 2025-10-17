using UnityEngine;
using System.Collections;

public class CameraFocusZoom : MonoBehaviour
{
    [Header("Links")]
    public Transform cameraTransform;      // drag PlayerFollowCamera here
    public Camera cam;                     // drag the Camera component here
    public Transform focusPoint;           // drag MonitorFocusPoint here
    public Transform player;               // drag your player root (Mira)
    public Behaviour playerController;     // (optional) your movement script e.g. ThirdPersonController
    public Behaviour selector;             // (optional) Pixel Crushers Selector on player

    [Header("Settings")]
    public float moveTime = 0.5f;          // seconds to move in/out
    public float focusDistance = 0.35f;    // how far from focusPoint to place the cam
    public float focusFOV = 35f;           // zoomed-in FOV
    public float normalFOV = 60f;          // normal FOV

    [Header("Input")]
    public KeyCode toggleKey = KeyCode.E;
    public KeyCode cancelKey = KeyCode.Escape;

    bool isFocused = false;
    Vector3 savedCamPos;
    Quaternion savedCamRot;
    float savedFOV;

    void Awake()
    {
        if (cam == null && cameraTransform != null) cam = cameraTransform.GetComponent<Camera>();
        if (cam != null) normalFOV = cam.fieldOfView;
    }

    public void ToggleFocus()
    {
        StopAllCoroutines();
        if (!isFocused) StartCoroutine(FocusIn());
        else StartCoroutine(FocusOut());
    }

    IEnumerator FocusIn()
    {
        isFocused = true;

        // save
        savedCamPos = cameraTransform.position;
        savedCamRot = cameraTransform.rotation;
        savedFOV = cam.fieldOfView;

        // disable movement/selector while focused
        if (playerController) playerController.enabled = false;
        if (selector) selector.enabled = false;

        // target cam pose
        Vector3 targetPos = focusPoint.position - focusPoint.forward * focusDistance;
        Quaternion targetRot = Quaternion.LookRotation(focusPoint.forward, Vector3.up);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            cameraTransform.position = Vector3.Lerp(savedCamPos, targetPos, t);
            cameraTransform.rotation = Quaternion.Slerp(savedCamRot, targetRot, t);
            cam.fieldOfView = Mathf.Lerp(savedFOV, focusFOV, t);
            yield return null;
        }

        cameraTransform.position = targetPos;
        cameraTransform.rotation = targetRot;
        cam.fieldOfView = focusFOV;
    }

    IEnumerator FocusOut()
    {
        isFocused = false;

        Vector3 startPos = cameraTransform.position;
        Quaternion startRot = cameraTransform.rotation;
        float startFOV = cam.fieldOfView;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            cameraTransform.position = Vector3.Lerp(startPos, savedCamPos, t);
            cameraTransform.rotation = Quaternion.Slerp(startRot, savedCamRot, t);
            cam.fieldOfView = Mathf.Lerp(startFOV, savedFOV, t);
            yield return null;
        }

        cameraTransform.position = savedCamPos;
        cameraTransform.rotation = savedCamRot;
        cam.fieldOfView = savedFOV;

        // re-enable control
        if (playerController) playerController.enabled = true;
        if (selector) selector.enabled = true;
    }

    // Optional keyboard toggle while focused
    void Update()
    {
        if (isFocused && Input.GetKeyDown(cancelKey)) ToggleFocus();
    }
}

