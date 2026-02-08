using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public TrashTaskController trashTaskController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash Door"))
        {
            trashTaskController.OnDoorClose();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trash Door"))
        {
            trashTaskController.OnDoorOpen();
        }
    }
}
