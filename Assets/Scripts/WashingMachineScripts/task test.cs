using UnityEngine;

public class tasktest : MonoBehaviour
{
    public void TakeDirtyClothes()
    {
        GameTask.StartGameTask("Take Dirty Clothes to Washing Machine");
   }
   public void LoadWashingMachine()
   {
        GameTask.AddGameTaskProgress("Load Washing Machine with Dirty Clothes", 1, 1);
    }

    public void StartWashingMachine()
    {
       GameTask.AddGameTaskProgress("Start Washing Machine", 2, 1);
   }

    public void UnloadWashingMachine()
    {
        GameTask.AddGameTaskProgress("Unload Washing Machine", 3, 1);
    }
}