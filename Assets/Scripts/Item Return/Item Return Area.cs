// Created by: John
// Description: Object going out of bounds handler

using UnityEngine;

public class ItemReturnArea : MonoBehaviour
{
    void OnTriggerExit(Collider other) // When object exits play area
    {
        other.TryGetComponent(out Rigidbody rb);
        if (rb != null && !rb.isKinematic) // Check if the object has a rigidbody and is not kinematic
        {
            other.TryGetComponent(out InteractableItemInfo interactableItemInfo);
            if (interactableItemInfo != null && !interactableItemInfo.recentlyKinematic) // Check if the object has a starting position reference and is recently not kinematic
            {
                other.transform.position = interactableItemInfo.startingPosition;
                rb.linearVelocity = Vector3.zero; // Stops all movement for the object
            } 
        }
        
    }
}
