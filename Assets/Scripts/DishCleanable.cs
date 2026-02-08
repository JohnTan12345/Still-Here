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

        if (other.CompareTag("Water"))
        {
            cleanTimer += Time.deltaTime;

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
            cleanTimer = Mathf.Max(cleanTimer - 0.5f, 0f);
        }
    }

    void CleanDish()
    {
        isClean = true;
        dishRenderer.material = cleanMaterial;
    }
}
