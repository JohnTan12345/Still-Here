using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashTaskController : MonoBehaviour
{
    public GameObject TrashBagDetectionArea;

    private bool TrashInside = false;
    private GameObject trashBag;
    public void NearTrashCan()
    {
        GameTasks.AddGameTaskProgress("Take out trash", 1, 1);
    }

    public void TrashPickUp()
    {
        GameTasks.StartGameTask("Take out trash");
    }

    public void OnDoorClose()
    {

        if (TrashInside && trashBag != null)
        {
            Destroy(trashBag);
            TrashBagDetectionArea.SetActive(false);
            GameTasks.AddGameTaskProgress("Take out trash", 2, 1);
        }
        else
        {
            TrashBagDetectionArea.SetActive(false);
        }
    }

    public void OnDoorOpen()
    {
        TrashBagDetectionArea.SetActive(true);
    }

    public void TrashBagEntered(SelectEnterEventArgs eventArgs)
    {
        TrashInside = true;
        trashBag = eventArgs.interactableObject.transform.gameObject;
        Debug.Log(trashBag);
    }

    public void TrashBagExited()
    {
        TrashInside = false;
        trashBag = null;
    }
}
