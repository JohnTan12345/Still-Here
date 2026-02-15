// Created by: Xander
// Description: trigger the tap water from the sink

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TapWaterTrigger : MonoBehaviour
{
    public ParticleSystem waterParticles;
    private bool waterOn = false;

    // Turn the water on or off
    public void ToggleWater()
    {
        waterOn = !waterOn;

        if (waterOn)
            waterParticles.Play();
        else
            waterParticles.Stop();
    }
}
