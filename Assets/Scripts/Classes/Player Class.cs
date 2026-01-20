using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player
{
    public PlayerData playerData = new PlayerData();
    public Run currentRun;
    public bool isLoaded() => playerData != null;

    public async Task CreatePlayer()
    {
        if (DatabaseAccountManager.isAuthenticated())
        {
            DatabaseResult result = await DatabaseManager.GetUserDataAsync();
            if (result.snapshot.Exists)
            {
                string playerDataJSON = result.snapshot.GetRawJsonValue();
                Debug.Log(playerDataJSON);
                playerData = JsonUtility.FromJson<PlayerData>(playerDataJSON);
            }
            else
            {
                Debug.Log("No data found");
            }
        } else
        {
            Debug.Log("Player not logged in");
        }
    }

    public void SavePlayerData()
    {
        DatabaseManager.SaveUserDataAsync(this);
    }

    public void SaveRun()
    {
        List<Run> previousRuns = playerData.PreviousRuns;

        if (previousRuns.Count > 5)
        {
            previousRuns.Remove(previousRuns[0]);
        }

        previousRuns.Add(currentRun);
    }
}

[System.Serializable]
public class PlayerData
{
    public List<string> Endings = new List<string>();
    public List<Run> PreviousRuns = new List<Run>();
}
[System.Serializable]
public class Run
{
    public float Time = 0;
    public List<TaskInfo> TaskList = new List<TaskInfo>();
}
[System.Serializable]
public class TaskInfo
{
    public string TaskName = "";
    public int CompletionCount = 0;
}

