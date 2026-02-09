using UnityEngine;

public class InteractableItemInfo : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startingPosition;
    [HideInInspector]
    public bool recentlyKinematic;

    void Awake()
    {
        startingPosition = transform.position;
    }
}
