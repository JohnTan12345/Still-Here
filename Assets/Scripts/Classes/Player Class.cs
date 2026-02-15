// Created by: John
// Description: Player data saving and handling player

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player
{
    public static Player currentPlayer {get; private set;} = null; // Return the current player
    public PlayerData playerData = new PlayerData();
    public Run currentRun;
    public bool isLoaded() => playerData != null;

    // Creates a player with a fresh player data or from database
    public async Task CreatePlayer()
    {
        if (DatabaseAccountManager.isAuthenticated())
        {
            DatabaseResult result = await DatabaseManager.GetUserDataAsync(); // Wait until data returns from database
            if (result.snapshot.Exists)
            {
                string playerDataJSON = result.snapshot.GetRawJsonValue();
                Debug.Log(playerDataJSON);
                playerData = JsonUtility.FromJson<PlayerData>(playerDataJSON); // Set player data to this player's data
            }
            else
            {
                Debug.Log("No data found");
            }
        } else
        {
            Debug.Log("Player not logged in");
        }

        currentPlayer = this;
    }

    // Saves player data to the database
    public void SavePlayerData()
    {
        DatabaseManager.SaveUserDataAsync(this);
    }

    // Saves the current run
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
    public int Time = 0;
    public string GetTimeString() // Get the time in string form for convenience
    {
        if (Time < 60)
        {
            return $"00:{(Time >= 10 ? Time : "0" + Time.ToString())}";
        }
        int TimeSeconds = Time % 60;
        int TimeMinutes = (int)Mathf.Floor(Time/60);

        return $"{(TimeMinutes >=10 ? TimeMinutes : "0"+ TimeMinutes)}:{(TimeSeconds >= 10 ? TimeSeconds : "0" + TimeSeconds.ToString())}";
    }
    public List<TaskInfo> Tasklist = new List<TaskInfo>();
}
[System.Serializable]
public class TaskInfo
{
    public string TaskName = "";
    public int CompletionCount = 0;
}

