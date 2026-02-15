// Created by: John
// Description: Clothes too big, make it phase through the washing machine at the entrance

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClothesPhysicsChange : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laundry"))
        {
            other.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.Instantaneous; // Make it phase through when entering the washing machine
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Laundry"))
        {
            StartCoroutine(RecentlyKinematic(other.GetComponent<InteractableItemInfo>()));
            other.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking; // Make it not phase through things when exiting the washing machine
        }
    }

    // Start a timer to potentially stop the clothes from teleporting back to starting position when switching from Instantaneous to Velocity Tracking
    private IEnumerator RecentlyKinematic(InteractableItemInfo interactableItemInfo)
    {
        interactableItemInfo.recentlyKinematic = true;
        yield return new WaitForSecondsRealtime(1);
        interactableItemInfo.recentlyKinematic = false;
    }
}
