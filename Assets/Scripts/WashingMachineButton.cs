using System.Collections;
using UnityEngine;

public class WashingMachineButton : MonoBehaviour
{
    public WashingMachineSpin washingMachine;
    public ParticleSystem washingVFX;
    public float vfxDuration = 15f;

    private bool isPressed = false;

    // Called by XR Simple Interactable → Activated event
    public void OnButtonPressed()
    {
        if (isPressed)
            return;

        isPressed = true;

        // Start washing
        washingMachine.StartMachine();

        // Play VFX
        if (washingVFX != null)
        {
            washingVFX.Play();
            StartCoroutine(StopVFXAfterTime());
        }
    }

    IEnumerator StopVFXAfterTime()
    {
        yield return new WaitForSeconds(vfxDuration);

        if (washingVFX != null)
            washingVFX.Stop();
    }
}
