using UnityEngine;

public class ItemReturnArea : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out Rigidbody rb);
        if (rb != null && !rb.isKinematic)
        {
            other.TryGetComponent(out InteractableItemInfo interactableItemInfo);
            if (interactableItemInfo != null && !interactableItemInfo.recentlyKinematic)
            {
                other.transform.position = interactableItemInfo.startingPosition;
                rb.linearVelocity = Vector3.zero;
            } 
        }
        
    }
}
