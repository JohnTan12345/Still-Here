using System.Collections;
using UnityEngine;

public class WashingMachineButton : MonoBehaviour
{
    public WashingMachineSpin washingMachine;
    public ParticleSystem washingVFX;
    public AudioSource endBeepAudio;

    public float cycleDuration = 15f;

    private bool isRunning = false;
    private bool hasStartedAtLeastOnce = false;

    public void OnButtonPressed()
    {
        if (isRunning)
            return;

        hasStartedAtLeastOnce = true;
        StartCoroutine(WashCycle());
    }

    IEnumerator WashCycle()
    {
        isRunning = true;
        GameTasks.AddGameTaskProgress("Laundry", 2, 1);

        washingMachine.StartMachine();

        if (washingVFX != null)
            washingVFX.Play();

        yield return new WaitForSeconds(cycleDuration);

        if (washingVFX != null)
            washingVFX.Stop();

        washingMachine.StopMachine();

        // EXTRA SAFETY: only play after a real cycle
        if (hasStartedAtLeastOnce && endBeepAudio != null)
            endBeepAudio.Play();

        isRunning = false;
    }
}
