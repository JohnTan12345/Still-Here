// Created by: Lucas
// Description: enables low pass filter for the rain when player enters an area

using JetBrains.Annotations;
using UnityEngine;

public class RainSoundEffects : MonoBehaviour
{
    public AudioLowPassFilter lowPassFilter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            lowPassFilter.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            lowPassFilter.enabled = false;
    }
}
