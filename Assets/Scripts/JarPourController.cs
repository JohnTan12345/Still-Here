using UnityEngine;

public class JarPourController : MonoBehaviour
{
    public Transform pourPoint;
    public ParticleSystem pourParticles;

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
        float angle = Vector3.Angle(pourPoint.forward, Vector3.down);
        return angle < pourAngle;
    }

    bool IsBeingShaken()
    {
        return rb.linearVelocity.magnitude > shakeThreshold;
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

    public void OnJarPickUp()
    {
        GameTasks.StartGameTask("Fish Feeding Task");
    }

    public void OnFishFed()
    {
        GameTasks.AddGameTaskProgress("Fish Feeding Task", 1, 1);
    }

    
    
}
