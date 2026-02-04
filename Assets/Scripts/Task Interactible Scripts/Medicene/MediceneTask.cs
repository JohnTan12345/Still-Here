using UnityEngine;

public class MediceneTask : MonoBehaviour
{
    public void onPickUp()
    {
        GameTasks.StartGameTask("Take meds");
    }
}
