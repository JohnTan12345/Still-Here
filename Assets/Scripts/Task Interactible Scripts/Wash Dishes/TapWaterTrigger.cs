using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TapWaterTrigger : MonoBehaviour
{
    public ParticleSystem waterParticles;
    private bool waterOn = false;

    public void ToggleWater()
    {
        waterOn = !waterOn;

        if (waterOn)
            waterParticles.Play();
        else
            waterParticles.Stop();
    }
}
