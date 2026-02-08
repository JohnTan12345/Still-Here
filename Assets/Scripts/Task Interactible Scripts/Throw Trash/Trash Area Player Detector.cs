using UnityEngine;

public class TrashAreaPlayerDetector : MonoBehaviour
{
    public TrashTaskController trashTaskController;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trashTaskController.NearTrashCan();
        }
    }
}
