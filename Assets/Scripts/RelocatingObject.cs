// Created by: John
// Description: Relocating object mechanic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RelocatingObject : MonoBehaviour
{
    private bool pickedUpBefore = false;
    private bool currentlyPickedUp = false;
    [Tooltip("The object name inside the database this script will reference. Will use this object name if none set")]
    public string ObjectPositionsReferenceName;
    [Tooltip("Sets the time taken for this object to change position. Will use GameManager's game settings if set to -1")]
    public float RelocationTime = -1;
    [Tooltip("Transform that will get teleported, will use this transform if none set")]
    [SerializeField]
    private Transform targetTransform;

    // Sets up the variables
    void Start()
    {
        if (string.IsNullOrEmpty(ObjectPositionsReferenceName))
        {
            ObjectPositionsReferenceName = gameObject.name;
        }

        if (RelocationTime <= -1)
        {
            RelocationTime = GameManager.Instance.ObjectRelocationTime;
        }

        if (targetTransform == null)
        {
            targetTransform = transform;
        }

        gameObject.GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnPickUp);
        gameObject.GetComponent<XRGrabInteractable>().selectExited.AddListener(OnRelease);
    }

    // Allows the object to relocate after being picked up once
    private void OnPickUp(SelectEnterEventArgs eventArgs)
    {
        pickedUpBefore = true;
        currentlyPickedUp = true;
    }

    // Starts the relocation timer after releasing the object
    private void OnRelease(SelectExitEventArgs eventArgs)
    {
        currentlyPickedUp = false;
        StartCoroutine(RelocateTimer());
    }

    // Timer before the object relocates
    private IEnumerator RelocateTimer()
    {
        float timer = 0;
        while (!currentlyPickedUp && pickedUpBefore)
        {
            timer += Time.deltaTime;

            if (timer >= RelocationTime)
            {
                RelocateObject();
                yield break;
            }
            yield return null;
        }
    }

    // Relocates the object based off possible positions inside the GameInfo static class
    private void RelocateObject()
    {
        pickedUpBefore = false;
        List<Vector3> possibleObjectPositions = GameInfo.GetObjectPositions(ObjectPositionsReferenceName);
        targetTransform.localPosition = possibleObjectPositions[Random.Range(0, possibleObjectPositions.Count - 1)];
    }
}
