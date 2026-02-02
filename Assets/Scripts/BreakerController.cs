using UnityEngine;
using System.Collections;

public class BreakerController : MonoBehaviour
{
    [Header("Lever Settings")]
    public Transform leverTransform;     // assign lever object
    public float flipAngle = -45f;       // OFF position
    public float resetAngle = 45f;       // ON position
    public float flipSpeed = 3f;         // rotation speed

    [Header("Neon Settings")]
    public Light[] neonLights;           // assign multiple lights
    public Renderer[] neonRenderers;     // assign multiple renderers
    public Color glowColor = Color.yellow;
    public float finalIntensity = 5f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip snapSound;

    private bool breakerOn = false;
    private Material[] neonMats;
    private Quaternion offRotation;
    private Quaternion onRotation;

    void Start()
    {
        // Lever setup
        offRotation = Quaternion.Euler(flipAngle, 0, 0);
        onRotation = Quaternion.Euler(resetAngle, 0, 0);
        if (leverTransform != null)
            leverTransform.localRotation = offRotation;

        // Neon setup
        neonMats = new Material[neonRenderers.Length];
        for (int i = 0; i < neonRenderers.Length; i++)
        {
            neonMats[i] = neonRenderers[i].material;
        }
        TurnOff();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            ToggleBreaker();
        }
    }

    private void ToggleBreaker()
    {
        breakerOn = !breakerOn;

        // Play sound
        if (audioSource && snapSound)
            audioSource.PlayOneShot(snapSound);

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
      GameTasks.StartGameTask("Circuit Breaker Task");
    }

    public void OnFlipSwitch()
    {
      GameTasks.AddGameTaskProgress("Circuit Breaker Task", 1, 1);
    }

    public void OnLightFlicker()
    {
      GameTasks.AddGameTaskProgress("Circuit Breaker Task", 2, 1);
    }

    public void OnCloseDoor()
    {
      GameTasks.AddGameTaskProgress("Circuit Breaker Task", 3, 1);
    }

}
