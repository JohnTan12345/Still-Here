using UnityEngine;

public class InteractableItemInfo : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startingPosition;

    void Awake()
    {
        startingPosition = transform.position;
    }
}
