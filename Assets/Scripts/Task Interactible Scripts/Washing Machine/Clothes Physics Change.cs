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
            other.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }
    }
}
