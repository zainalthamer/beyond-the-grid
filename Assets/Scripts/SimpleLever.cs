using UnityEngine;
using System.Collections;

public class SimpleLever : MonoBehaviour
{
    [Header("Positions (local)")]
    public float offPosZ = 0f;      // lever rest position (z)
    public float onPosZ = -0.02f;   // lever pulled down position (z)

    [Header("Motion")]
    public float speed = 6f;        // move speed

    [Header("State")]
    public bool isOn = false;

    public UnityEngine.Events.UnityEvent onTurnOn;
    public UnityEngine.Events.UnityEvent onTurnOff;

    private void Start()
    {
        // Ensure lever starts in the correct position based on its current state
        Vector3 pos = transform.localPosition;
        pos.z = isOn ? onPosZ : offPosZ;
        transform.localPosition = pos;
    }

    public void ToggleLever()
    {
        isOn = !isOn;
        StopAllCoroutines();
        StartCoroutine(MoveTo(isOn ? onPosZ : offPosZ));

        if (isOn) onTurnOn?.Invoke();
        else onTurnOff?.Invoke();
    }

    private IEnumerator MoveTo(float targetZ)
    {
        Vector3 start = transform.localPosition;
        Vector3 target = new Vector3(start.x, start.y, targetZ);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.localPosition = target;
    }
}
