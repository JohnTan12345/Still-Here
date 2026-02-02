using System.Collections;
using UnityEngine;

public class WashingMachineSpin : MonoBehaviour
{
    public float spinSpeed = 180f;   // Degrees per second
    public float spinDuration = 10f; // Spin time
    
    private void OnTriggerEnter(Collider other)
    {
        // Only affect laundry objects
        if (!other.CompareTag("Laundry"))
            return;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            StartCoroutine(SpinLaundry(other.transform, rb));
        }
    }

    IEnumerator SpinLaundry(Transform obj, Rigidbody rb)
    {
        // Stop existing movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Freeze position so it can't clip out
        RigidbodyConstraints originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezePosition;

        float timer = 0f;

        while (timer < spinDuration)
        {
            obj.RotateAround(transform.position, Vector3.up, spinSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Restore original constraints
        rb.constraints = originalConstraints;
    }
}
