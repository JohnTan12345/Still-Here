using UnityEngine;

public class WaterPourTilt : MonoBehaviour
{
    public Transform pourPoint;
    public ParticleSystem pourParticles;



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

    bool IsTiltedCorrectly()
    {
        float angle = Vector3.Angle(pourPoint.forward, Vector3.down);
        return angle < pourAngle;
    }

   

    void StartPouring()
    {
        if (!pourParticles.isPlaying)
            pourParticles.Play();

        
    }

    void StopPouring()
    {
        if (pourParticles.isPlaying)
            pourParticles.Stop();
    }
}
