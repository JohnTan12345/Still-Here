using System.Collections;
using UnityEngine;

public class WashingMachineSpin : MonoBehaviour
{
    public float spinSpeed = 180f;
    public float spinDuration = 10f;

    private bool isMachineOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isMachineOn)
            return;

        if (!other.CompareTag("laundry"))
            return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            StartCoroutine(SpinLaundry(other.transform, rb));
        }
    }

    public void StartMachine()
    {
        if (isMachineOn) return;
        isMachineOn = true;
    }

    public void StopMachine()
    {
        isMachineOn = false;
    }

    IEnumerator SpinLaundry(Transform obj, Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        RigidbodyConstraints originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        float timer = 0f;

        while (timer < spinDuration && isMachineOn)
        {
            obj.RotateAround(transform.position, Vector3.up, spinSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.constraints = originalConstraints;
    }
}
