// Created by: John
// Description: Throw the trash task interaction controller

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashTaskController : MonoBehaviour
{
    public GameObject TrashBagDetectionArea;

    private bool TrashInside = false;
    private GameObject trashBag;
    public void NearTrashCan() // Add progress when player is near trash can
    {
        GameTasks.AddGameTaskProgress("Take out trash", 1, 1);
    }

    public void TrashPickUp() // Gametask wrapper to start the task
    {
        GameTasks.StartGameTask("Take out trash");
    }

    // Deletes the trash bag if the door is closed
    public void OnDoorClose()
    {

        if (TrashInside && trashBag != null)
        {
            Destroy(trashBag);
            TrashBagDetectionArea.SetActive(false);
            GameTasks.AddGameTaskProgress("Take out trash", 2, 1); // Add progress once trash bag is deleted
        }
        else
        {
            TrashBagDetectionArea.SetActive(false);
        }
    }
    
    // Allow trash bag to enter the chute when door opened
    public void OnDoorOpen() 
    {
        TrashBagDetectionArea.SetActive(true);
    }

    // Set the trash bag as the current trash inside the chute
    public void TrashBagEntered(SelectEnterEventArgs eventArgs) 
    {
        TrashInside = true;
        trashBag = eventArgs.interactableObject.transform.gameObject;
        Debug.Log(trashBag);
    }

    // remove the reference to the trash bag
    public void TrashBagExited() 
    {
        TrashInside = false;
        trashBag = null;
    }
}
