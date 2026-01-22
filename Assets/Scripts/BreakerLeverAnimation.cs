using UnityEngine;
using System.Collections;

public class BreakerLeverAnimation : MonoBehaviour
{
    public float flipAngle = -45f;   // OFF position
    public float resetAngle = 45f;   // ON position
    public float flipSpeed = 3f;     // rotation speed

    private bool isOn = false;

    private Quaternion offRotation;
    private Quaternion onRotation;

    void Start()
    {
        offRotation = Quaternion.Euler(flipAngle, 0, 0);
        onRotation = Quaternion.Euler(resetAngle, 0, 0);
        transform.localRotation = offRotation;
    }

    public bool IsOn => isOn;

    public void FlipUp()
    {
        isOn = true;
        StopAllCoroutines();
        StartCoroutine(RotateLever(onRotation));
    }

    public void FlipDown()
    {
        isOn = false;
        StopAllCoroutines();
        StartCoroutine(RotateLever(offRotation));
    }

    private IEnumerator RotateLever(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * flipSpeed
            );
            yield return null;
        }
        transform.localRotation = targetRotation;
    }
}
