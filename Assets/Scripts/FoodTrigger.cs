using UnityEngine;

public class FishTankFoodTrigger : MonoBehaviour
{
    private float feedTimer = 0f;
    public float requiredFeedTime = 3f;
    private bool fed = false;

    void OnTriggerStay(Collider other)
    {
        if (fed) return;

        if (other.CompareTag("FishFood"))
        {
            feedTimer += Time.deltaTime;

            if (feedTimer >= requiredFeedTime)
            {
                fed = true;
                Debug.Log("Fish Fed!");
                
            }
        }
    }
}
