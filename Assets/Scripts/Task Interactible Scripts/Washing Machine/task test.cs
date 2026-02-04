using UnityEngine;

public class tasktest : MonoBehaviour
{
   public void TakeDirtyClothes()
    {
        GameTasks.AddGameTaskProgress("Take Dirty Clothes to Washing Machine", 0, 1);
    }
   public void LoadWashingMachine()
   { 
        GameTasks.AddGameTaskProgress("Load Washing Machine with Dirty Clothes", 1, 1);
   }

   public void StartWashingMachine()
    {
       GameTasks.AddGameTaskProgress("Start Washing Machine", 2, 1);
    }

   public void UnloadWashingMachine()
    {
        GameTasks.AddGameTaskProgress("Unload Washing Machine", 3, 1);
    }
}