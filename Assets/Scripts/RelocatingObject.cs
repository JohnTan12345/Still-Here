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

        gameObject.GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnPickUp);
        gameObject.GetComponent<XRGrabInteractable>().selectExited.AddListener(OnRelease);
    }

    private void OnPickUp(SelectEnterEventArgs eventArgs)
    {
        pickedUpBefore = true;
        currentlyPickedUp = true;
    }

    private void OnRelease(SelectExitEventArgs eventArgs)
    {
        currentlyPickedUp = false;
        StartCoroutine(RelocateTimer());
    }

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

    private void RelocateObject()
    {
        pickedUpBefore = false;
        List<Vector3> possibleObjectPositions = GameInfo.GetObjectPositions(ObjectPositionsReferenceName);
        transform.localPosition = possibleObjectPositions[Random.Range(0, possibleObjectPositions.Count - 1)];
    }
}
