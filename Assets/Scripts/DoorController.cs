using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float speed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
    }

    System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * speed
            );
            yield return null;
        }
    }
}
