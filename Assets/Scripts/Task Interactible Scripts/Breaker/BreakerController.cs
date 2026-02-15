// Created by: Rayner
// Edited by: John
// Descripion: Controls the breaker switches and functions

using UnityEngine;
using System.Collections;

public class BreakerController : MonoBehaviour
{
    [Header("Lever Settings")]
    public Transform leverTransform;
    public float flipAngle = -45f;
    public float resetAngle = 45f;
    public float flipSpeed = 3f;

    [Header("Neon Settings")]
    public Light[] neonLights;
    public Renderer[] neonRenderers;
    public Color glowColor = Color.yellow;
    public float finalIntensity = 5f;

    [Header("Audio Settings")]
    public AudioSource snapSource;          // normal snap sound
    public AudioClip snapSound;
    public AudioSource shortCircuitSource;   // short circuit sound
    public AudioClip shortCircuitClip;

    [Header("VFX Settings")]
    public ParticleSystem sparkVFX;

    [Header("Breaker Settings")]
    public int maxFlipsBeforeBreak = 5;

    private bool breakerOn = false;
    private bool breakerBroken = false;
    private int flipCount = 0;

    private Material[] neonMats;
    private Quaternion offRotation;
    private Quaternion onRotation;

    void Start()
    {
        // Initialize lever rotations
        offRotation = Quaternion.Euler(flipAngle, 0, 0);
        onRotation = Quaternion.Euler(resetAngle, 0, 0);
        if (leverTransform != null)
            leverTransform.localRotation = offRotation;
        
        // Cache neon materials
        neonMats = new Material[neonRenderers.Length];

        // Assign materials
        for (int i = 0; i < neonRenderers.Length; i++)
        {
            neonMats[i] = neonRenderers[i].material;
        }
     
        TurnOff();
    }

    public void ToggleBreaker()
    {
        Debug.Log("trigger");
        if (breakerBroken) return; // no toggling if broken

        breakerOn = !breakerOn;
        flipCount++;
        OnFlipSwitch();
        // Check if breaker should break
        if (flipCount >= maxFlipsBeforeBreak)
        {
            BreakBreaker();
            return;
        }

        // Play snap sound
        if (snapSource && snapSound)
            snapSource.PlayOneShot(snapSound);

        // Lever animation
        if (leverTransform != null)
        {
            StopAllCoroutines();
            StartCoroutine(RotateLever(breakerOn ? onRotation : offRotation));
        }

        // Neon flicker
        if (breakerOn) StartCoroutine(FlickerRoutine());
        else TurnOff();
    }

    private void BreakBreaker()
    {
        breakerBroken = true;

        // Stop any flicker
        StopAllCoroutines();
        TurnOff();

        // Play sparks VFX
        if (sparkVFX != null)
            sparkVFX.Play();

        // Play short circuit sound
        if (shortCircuitSource && shortCircuitClip)
            shortCircuitSource.PlayOneShot(shortCircuitClip);

        // Optional: force lever to "stuck" position
        if (leverTransform != null)
            leverTransform.localRotation = offRotation;
    }

    // --- Lever Animation ---
    private IEnumerator RotateLever(Quaternion targetRotation)
    {
        while (Quaternion.Angle(leverTransform.localRotation, targetRotation) > 0.1f)
        {
            leverTransform.localRotation = Quaternion.Slerp(
                leverTransform.localRotation,
                targetRotation,
                Time.deltaTime * flipSpeed
            );
            yield return null;
        }
        leverTransform.localRotation = targetRotation;
    }

    // --- Neon Flicker ---
    private void TurnOff()
    {
        StopCoroutine("FlickerRoutine");
        foreach (var light in neonLights) light.intensity = 0;
        foreach (var mat in neonMats) mat.SetColor("_EmissionColor", Color.black);
    }

    // Light flickering animation
    private IEnumerator FlickerRoutine()
    {
        foreach (var light in neonLights) light.intensity = 0;
        foreach (var mat in neonMats) mat.SetColor("_EmissionColor", Color.black);

        for (int i = 0; i < 6; i++)
        {
            float randomIntensity = Random.Range(0.5f, finalIntensity);

            foreach (var light in neonLights) light.intensity = randomIntensity;
            foreach (var mat in neonMats) mat.SetColor("_EmissionColor", glowColor * randomIntensity);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

            foreach (var light in neonLights) light.intensity = 0;
            foreach (var mat in neonMats) mat.SetColor("_EmissionColor", Color.black);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        foreach (var light in neonLights) light.intensity = finalIntensity;
        foreach (var mat in neonMats) mat.SetColor("_EmissionColor", glowColor * finalIntensity);
    }

    // --- Game Task Integration ---
    public void OnOpenDoor()
    {
      GameTasks.StartGameTask("Fix circuit breaker");
    }

    public void OnFlipSwitch()
    {
      GameTasks.AddGameTaskProgress("Fix circuit breaker", 1, 1);
    }

}
