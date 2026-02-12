// Done by: Xander
// Edited by: John
// Description: Detects how the jar is poured

using UnityEngine;

public class PourController : MonoBehaviour
{
    public Transform PourPoint;
    public ParticleSystem PourParticles;
    public GameObject PourItemPrefab;

    [Header("Pour Settings")]
    public float PourAngle = 60f;

    [Header("Shake Settings")]
    public bool RequireShaking = false;
    public float ShakeThreshold = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Check if the object is tilted enough and is being shaken if enabled
    void Update()
    {
        bool tiltedEnough = IsTiltedCorrectly();
        bool isShaking = IsBeingShaken();

        if (tiltedEnough)
        {
            if (RequireShaking && !isShaking)
            {
                StopPouring();
            }
            else
            {
                StartPouring();
            }
            
        }
        else
        {
            StopPouring();
        }
    }

    // Check if the object is tilted enough
    bool IsTiltedCorrectly()
    {
        float angle = Vector3.Angle(PourPoint.up, Vector3.down);
        return angle < PourAngle;
    }

    // Check if the object is being shaken hard enough
    bool IsBeingShaken()
    {
        return rb.linearVelocity.magnitude > ShakeThreshold;
    }

    // Spawn the prefab
    void StartPouring()
    {
        if (PourParticles != null && !PourParticles.isPlaying)
            PourParticles.Play();

        Instantiate(PourItemPrefab, PourPoint.position, PourPoint.rotation);
    }

    // Stop spawning the prefab
    void StopPouring()
    {
        if (PourParticles != null && PourParticles.isPlaying)
            PourParticles.Stop();
    }
}