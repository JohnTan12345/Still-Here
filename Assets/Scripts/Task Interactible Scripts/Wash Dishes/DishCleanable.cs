// Created by: Xander
// Description: Make the dishes look clean after being washed and washing mechanic

using UnityEngine;

public class DishCleanable : MonoBehaviour
{
    public Renderer dishRenderer;
    public Material dirtyMaterial;
    public Material cleanMaterial;

    public float cleanTimeRequired = 5f;

    private float cleanTimer = 0f;
    private bool isClean = false;

    private void OnTriggerStay(Collider other)
    {
        if (isClean) return;

        if (other.CompareTag("Water")) // If the plate is being washed
        {
            cleanTimer += Time.deltaTime; // Start or continue timer until clean

            if (cleanTimer >= cleanTimeRequired)
            {
                CleanDish();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            cleanTimer = Mathf.Max(cleanTimer - 0.5f, 0f); // stop the timer
        }
    }

    // Add progress to dishes task after being cleaned
    void CleanDish()
    {
        isClean = true;
        GameTasks.AddGameTaskProgress("Wash Dishes", 1, 1);
        dishRenderer.material = cleanMaterial;
    }

    // Gametask wrapper to start the task
    public void OnDishPickup()
    {
        GameTasks.StartGameTask("Wash Dishes");
    }
}
