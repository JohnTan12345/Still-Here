using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClothesPhysicsChange : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laundry"))
        {
            other.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Instantaneous;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Laundry"))
        {
            StartCoroutine(RecentlyKinematic(other.GetComponent<InteractableItemInfo>()));
            other.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }
    }

    private IEnumerator RecentlyKinematic(InteractableItemInfo interactableItemInfo)
    {
        interactableItemInfo.recentlyKinematic = true;
        yield return new WaitForSecondsRealtime(1);
        interactableItemInfo.recentlyKinematic = false;
    }
}
