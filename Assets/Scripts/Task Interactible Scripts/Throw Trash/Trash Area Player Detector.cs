// Created by: John
// Description: Update the "Throw the trash" task when player is near the trash chute

using UnityEngine;

public class TrashAreaPlayerDetector : MonoBehaviour
{
    public TrashTaskController trashTaskController;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trashTaskController.NearTrashCan(); // Update the task
        }
    }
}
