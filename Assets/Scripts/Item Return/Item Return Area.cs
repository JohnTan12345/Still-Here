using UnityEngine;

public class ItemReturnArea : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>().isKinematic) return;
        other.transform.position = other.GetComponent<InteractableItemInfo>().startingPosition;
        other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
