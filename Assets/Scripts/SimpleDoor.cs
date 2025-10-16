using UnityEngine;
using System.Collections;

public class SimpleDoor : MonoBehaviour
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("Door Settings")]
    public float openAngle = 90f; 
    public float speed = 2f;     
    public Vector3 hingeOffset = new Vector3(-0.5f, 0, 0); 

    private Transform hinge; 

    void Start()
    {
        hinge = new GameObject("HingePivot_Runtime").transform;
        hinge.position = transform.TransformPoint(hingeOffset);
        hinge.rotation = transform.rotation;

        transform.SetParent(hinge);

        closedRotation = hinge.rotation;

        openRotation = Quaternion.Euler(hinge.eulerAngles + new Vector3(0, openAngle, 0));
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }

        hinge.rotation = targetRotation;
    }
}
