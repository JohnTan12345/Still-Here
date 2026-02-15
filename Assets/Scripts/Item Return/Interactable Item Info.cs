// Created by: John
// Description: Starting position reference to put the object back when thrown out of bounds

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
