// Done by: Lucas
// Edited by: John
// Description: Washing machine spin clothes

using System.Collections;
using UnityEngine;

public class WashingMachineController : MonoBehaviour
{
    public float spinSpeed = 180f;
    public float washCycleTime = 15f;
    public ParticleSystem washingVFX;
    public AudioClip endBeepAudio;

    private bool isMachineOn = false;

    public Transform clothesSocketGroup;

    void Awake()
    {
        if (clothesSocketGroup == null)
        {
            clothesSocketGroup = transform;
        }
    }

    public void ClothesPickedUp()
    {
        GameTasks.StartGameTask("Laundry");
    }
    public void ClothesEntered()
    {
        GameTasks.AddGameTaskProgress("Laundry", 1, 1);
        GameTasks.AddGameTaskProgress("Laundry", 3, -1);
    }

    public void ClothesExited()
    {
        GameTasks.AddGameTaskProgress("Laundry", 1, -1);
        GameTasks.AddGameTaskProgress("Laundry", 3, 1);
    }
    public void ButtonPressed()
    {
        StartCoroutine(SpinLaundry());
    }

    public void StopMachine()
    {
        isMachineOn = false;
    }

    public void OnDoorOpen()
    {
        clothesSocketGroup.gameObject.SetActive(true);
    }

    public void OnDoorClose()
    {
        clothesSocketGroup.gameObject.SetActive(false);
    }

    IEnumerator SpinLaundry()
    {
        if (isMachineOn)
        {
            yield break;
        }

        isMachineOn = true;

        float timer = 0f;
        washingVFX.Play();

        while (isMachineOn)
        {
            clothesSocketGroup.RotateAround(clothesSocketGroup.position, Vector3.up, spinSpeed * Time.deltaTime);
            timer += Time.deltaTime;

            if (timer >= washCycleTime)
            {
                isMachineOn = false;
                break;
            }

            yield return null;
        }

        washingVFX.Stop();
        if (endBeepAudio != null) {AudioSource.PlayClipAtPoint(endBeepAudio, transform.position);}
        GameTasks.AddGameTaskProgress("Laundry", 2, 1);
    }
}
