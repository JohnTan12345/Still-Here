// Created by: Xander
// Description: Water plant task interactivity

using UnityEngine;

public class WaterPlantChecker : MonoBehaviour
{
    public float requiredWaterTime = 5f;
    public ParticleSystem successSparkles;

    private float waterTimer = 0f;
    private bool watering = false;
    private bool completed = false;

    // Timer when the plant is being watered
    void Update()
    {
        if (watering && !completed)
        {
            waterTimer += Time.deltaTime;

            if (waterTimer >= requiredWaterTime)
            {
                CompleteWatering();
            }
        }
    }

    // When player waters the plant
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterParticle"))
        {
            watering = true;

            Destroy(other.gameObject);
        }
    }

    // When player stops watering the plant
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterParticle"))
        {
            watering = false;
        }
    }

    // When the plant is watered enough
    void CompleteWatering()
    {
        completed = true;
        watering = false;
        waterTimer = 0f;
        OnPlantsWatered();

        if (successSparkles != null)
        {
            successSparkles.Play();
        }

        Debug.Log("Plant has been watered!");
    }

    public void OnCanPickUp()
    {
        Debug.Log("Can picked up, starting watering plants task.");
        GameTasks.StartGameTask("Water plants");
    }

    public void OnPlantsWatered()
    {
        Debug.Log("Plants watered, adding progress to watering plants task.");
        GameTasks.AddGameTaskProgress("Water plants", 1, 1);
    }


}