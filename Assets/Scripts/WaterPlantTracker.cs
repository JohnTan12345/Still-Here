using UnityEngine;

public class WaterPlantChecker : MonoBehaviour
{
    public float requiredWaterTime = 5f;
    public ParticleSystem successSparkles;

    private float waterTimer = 0f;
    private bool watering = false;
    private bool completed = false;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterParticle"))
        {
            Debug.Log("Watering started.");
            watering = true;

            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterParticle"))
        {
            Debug.Log("Watering stopped.");
            watering = false;
        }
    }

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