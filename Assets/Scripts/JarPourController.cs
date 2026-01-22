using UnityEngine;

public class JarPourController : MonoBehaviour
{

    public Transform pourPoint;
    public ParticleSystem pourParticles;

    [Range(0f, 90f)]
    public float pourAngle = 60f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float angle = Vector3.Angle(pourPoint.forward, Vector3.down);

        if (angle < pourAngle)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        } 
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
