using UnityEngine;

public class FishFeedingChecker : MonoBehaviour
{
    public float requiredFeedTime = 5f;
    public ParticleSystem successBubbles;

    private float feedTimer = 0f;
    private bool feeding = false;
    private bool completed = false;

    void Update()
    {
        if (feeding && !completed)
        {
            feedTimer += Time.deltaTime;

            if (feedTimer >= requiredFeedTime)
            {
                CompleteFeeding();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishFood"))
        {
            Debug.Log("Feeding started.");
            feeding = true;

            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FishFood"))
        {
            Debug.Log("Feeding stopped.");
            feeding = false;
        }
    }

    void CompleteFeeding()
    {
        completed = true;
        feeding = false;

        if (successBubbles != null)
        {
            successBubbles.Play();
        }

        Debug.Log("Fish have been fed!");
    }
}
