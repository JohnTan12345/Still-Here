using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineSpin : MonoBehaviour
{
    public float spinSpeed = 180f;
    public float spinDuration = 10f;

    private bool isMachineOn = false;

    // Track which objects are already spinning
    private HashSet<Rigidbody> spinningLaundry = new HashSet<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        TryStartSpinning(other);
    }

    // IMPORTANT: handles laundry already inside when machine turns on
    private void OnTriggerStay(Collider other)
    {
        TryStartSpinning(other);
    }

    private void TryStartSpinning(Collider other)
    {
        if (!isMachineOn)
            return;

        if (!other.CompareTag("laundry"))
            return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        if (spinningLaundry.Contains(rb))
            return;

        StartCoroutine(SpinLaundry(other.transform, rb));
    }

    public void StartMachine()
    {
        isMachineOn = true;
    }

    public void StopMachine()
    {
        isMachineOn = false;
    }

    IEnumerator SpinLaundry(Transform obj, Rigidbody rb)
    {
        GameTasks.AddGameTaskProgress("WashingMachineTask", 2, 1);

        spinningLaundry.Add(rb);

        // Stop movement
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

        // Restore physics
        rb.constraints = originalConstraints;
        spinningLaundry.Remove(rb);
    }
}
