using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public PlayerData playerData;
    public Run currentRun;
    public bool isLoaded() => playerData != null;

    public async void CreatePlayer()
    {
        if (DatabaseAccountManager.isAuthenticated())
        {
            DatabaseResult result = await DatabaseManager.GetUserDataAsync();
            playerData = JsonUtility.FromJson<PlayerData>(result.snapshot.GetRawJsonValue());
        } else
        {
            playerData = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        DatabaseManager.SaveUserDataAsync(this);
    }
}

public class PlayerData
{
    public Dictionary<string, bool> Endings;
    public List<Run> previousRuns;
}

public class Run
{
    public float Time = 0;
    public List<TaskListInfo> TaskList;
}

public class TaskListInfo
{
    public string TaskName = "";
    public int CompletionCount = 0;
}

