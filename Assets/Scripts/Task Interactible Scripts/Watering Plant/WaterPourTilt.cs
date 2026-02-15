// Created by: Xander
// Description: Temporary script for pour mechanic

using UnityEngine;

public class WaterPourTilt : MonoBehaviour
{
    public Transform pourPoint;
    public ParticleSystem pourParticles;

    public GameObject waterParticlePrefab;

    [Header("Pour Settings")]
    public float pourAngle = 60f;



    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool tiltedEnough = IsTiltedCorrectly();
       

        if (tiltedEnough)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    // Check if the object is tilted enough
    bool IsTiltedCorrectly()
    {
        float angle = Vector3.Angle(pourPoint.forward, Vector3.down);
        return angle < pourAngle;
    }

    // Start spawning the "particles"
    void StartPouring()
    {
        if (!pourParticles.isPlaying)
            pourParticles.Play();

        GameObject newWaterParticle = Instantiate(waterParticlePrefab, pourPoint.position, pourPoint.rotation);
    }

    // Stop spawning the "particles"
    void StopPouring()
    {
        if (pourParticles.isPlaying)
            pourParticles.Stop();
    }
}
