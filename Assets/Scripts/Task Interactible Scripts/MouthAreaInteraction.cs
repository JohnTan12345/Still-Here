// Done by: John
// Description: "Eating" stuff

using UnityEngine;

public class MouthAreaInteraction : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Add progress if pills are brought to the mouth
        if (other.CompareTag("Pills"))
        {
            GameTasks.AddGameTaskProgress("Take medicene", 1, 1);
            Destroy(other.gameObject);
        }
    }
}
