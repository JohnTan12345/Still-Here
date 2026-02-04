// Done by: John
// Description: "Eating" stuff

using UnityEngine;

public class MouthAreaInteraction : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pills"))
        {
            GameTasks.AddGameTaskProgress("Take meds", 1, 1);
            Destroy(other.gameObject);
        }
    }
}
