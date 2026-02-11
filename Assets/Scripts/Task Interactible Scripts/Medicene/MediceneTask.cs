// Created by: John
// Description: Gametask wrapper for take medication task

using UnityEngine;

public class MediceneTask : MonoBehaviour
{
    public void onPickUp()
    {
        GameTasks.StartGameTask("Take medicene");
    }
}
