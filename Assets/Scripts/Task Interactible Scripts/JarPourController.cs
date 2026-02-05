// Done by: Xander
// Edited by: John
// Description: Detects how the jar is poured

using UnityEngine;

public class JarPourController : MonoBehaviour
{
    public Transform pourPoint;
    public ParticleSystem pourParticles;

    public GameObject pourItemPrefab;

    [Header("Pour Settings")]
    public float pourAngle = 60f;

    [Header("Shake Settings")]
    public float shakeThreshold = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool tiltedEnough = IsTiltedCorrectly();
        bool isShaking = IsBeingShaken();

        if (tiltedEnough && isShaking)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    bool IsTiltedCorrectly()
    {
        float angle = Vector3.Angle(pourPoint.up, Vector3.down);
        return angle < pourAngle;
    }

    bool IsBeingShaken()
    {
        return rb.linearVelocity.magnitude > shakeThreshold;
    }

    void StartPouring()
    {
        if (pourParticles != null && !pourParticles.isPlaying)
            pourParticles.Play();

        Instantiate(pourItemPrefab, pourPoint.position, pourPoint.rotation);
    }

    void StopPouring()
    {
        if (pourParticles != null && pourParticles.isPlaying)
            pourParticles.Stop();
    }
}