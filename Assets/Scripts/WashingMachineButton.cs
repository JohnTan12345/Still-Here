using System.Collections;
using UnityEngine;

public class WashingMachineButton : MonoBehaviour
{
    public WashingMachineSpin washingMachine;
    public ParticleSystem washingVFX;
    public float cycleDuration = 15f;

    private bool isRunning = false;

    public void OnButtonPressed()
    {
        if (isRunning)
            return;

        StartCoroutine(WashCycle());
    }

    IEnumerator WashCycle()
    {
        isRunning = true;

        // Start washing
        washingMachine.StartMachine();

        if (washingVFX != null)
            washingVFX.Play();

        // Wait for full cycle
        yield return new WaitForSeconds(cycleDuration);

        // Stop washing
        if (washingVFX != null)
            washingVFX.Stop();

        washingMachine.StopMachine();

        isRunning = false;
    }
}
