// Created by: John
// Description: Interactable door detector for interaction event purposes

using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    public TrashTaskController trashTaskController;
    public WashingMachineController washingMachineController;

    // When the door is closed
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable Door"))
        {
            // Run the listener when door closes
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

    // When the door is opened
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable Door"))
        {
            // Run the listener when door closes
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
