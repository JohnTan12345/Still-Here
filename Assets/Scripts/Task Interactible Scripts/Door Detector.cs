using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public TrashTaskController trashTaskController;
    public WashingMachineController washingMachineController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable Door"))
        {
            if (trashTaskController != null)
            {
                trashTaskController.OnDoorClose();
            }
            else if (washingMachineController != null)
            {
                washingMachineController.OnDoorClose();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable Door"))
        {
            if (trashTaskController != null)
            {
                trashTaskController.OnDoorOpen();
            }
            else if (washingMachineController != null)
            {
                washingMachineController.OnDoorOpen();
            }

        }
    }
}
