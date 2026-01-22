using UnityEngine;
using System.Collections;

public class NeonFlicker : MonoBehaviour
{
    public Light neonLight;          // assign in Inspector
    public Renderer neonRenderer;    // assign bar’s renderer
    public Color glowColor = Color.cyan;
    public float finalIntensity = 5f;

    private Material neonMat;

    void Start()
    {
        neonMat = neonRenderer.material;
        TurnOff();
    }

    public void StartFlicker()
    {
        StopAllCoroutines();
        StartCoroutine(FlickerRoutine());
    }

    public void TurnOff()
    {
        StopAllCoroutines();
        neonLight.intensity = 0;
        neonMat.SetColor("_EmissionColor", Color.black);
    }

    private IEnumerator FlickerRoutine()
    {
        neonLight.intensity = 0;
        neonMat.SetColor("_EmissionColor", Color.black);

        for (int i = 0; i < 6; i++)
        {
            float randomIntensity = Random.Range(0.5f, finalIntensity);
            neonLight.intensity = randomIntensity;
            neonMat.SetColor("_EmissionColor", glowColor * randomIntensity);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

            neonLight.intensity = 0;
            neonMat.SetColor("_EmissionColor", Color.black);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        neonLight.intensity = finalIntensity;
        neonMat.SetColor("_EmissionColor", glowColor * finalIntensity);
    }
}
