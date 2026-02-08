// Done by: Xander
// Description: Detects when fish feed enters the tank. Also has jar pick up as a side function

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
            Debug.Log("feeding");
            Debug.Log(other.gameObject);
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
        OnFishFed();

        if (successBubbles != null)
        {
            successBubbles.Play();
        }

        Debug.Log("Fish have been fed!");
    }

    public void OnJarPickUp()
    {
        Debug.Log("Jar picked up, starting fish feeding task.");
        GameTasks.StartGameTask("Feed fishes");
    }

    public void OnFishFed()
    {
        Debug.Log("Fish fed, adding progress to fish feeding task.");
        GameTasks.AddGameTaskProgress("Feed fishes", 1, 1);
    }
}

