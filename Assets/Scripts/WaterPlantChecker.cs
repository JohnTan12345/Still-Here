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

        if (successSparkles != null)
        {
            successSparkles.Play();
        }

        Debug.Log("Plant has been watered!");
    }


}