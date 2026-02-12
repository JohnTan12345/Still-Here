// Created by: John
// Description: Manages the leaderboard data

using System.Collections.Generic;
using System.Threading.Tasks;
using Google.MiniJSON;
using UnityEngine;

public static class LeaderboardManager
{
    public static List<LeaderboardInfo> leaderboard {get; private set;}
    private static int leaderboardLength = 5;

    // Fetch leaderboard
    public async static Task<List<LeaderboardInfo>> GetLeaderboard()
    {
        leaderboard = new List<LeaderboardInfo>();
        DatabaseResult result = await DatabaseManager.GetValueFromPath("Leaderboard");
        
        if (result.faulted)
        {
            Debug.Log("Something happened");
            return null;
        } else
        {
            // Adds in the current leaderboard rankings to the leaderboard list
            foreach(var child in result.snapshot.Children)
            {
                LeaderboardInfo leaderboardInfo = JsonUtility.FromJson<LeaderboardInfo>(child.GetRawJsonValue());
                leaderboard.Add(leaderboardInfo);
            }
            return leaderboard;
        }
    }

    // Update the leaderboard with the new run
    public static void UpdateLeaderboard(Run currentRun)
    {
        if (!DatabaseAccountManager.isAuthenticated())
        {
            Debug.Log("No user logged in");
            return;
        }

        if (leaderboard == null) // Do nothing if there is no leaderboard due to reasons
        {
            return;
        }

        int runTime = currentRun.Time;
        bool inLeaderboard = false;

        // Check if the run beats any leaderboard ranking
        if (leaderboard.Count < 5) // If the leaderboard is not even full
        {
            inLeaderboard = true;
        }
        else
        {
            for (int i = 0; i < leaderboard.Count; i++) // Compare the run time with the times in the leaderboard
            {
                if (runTime < leaderboard[i].Time)
                {
                    inLeaderboard = true;
                    break;
                }
            }
        }

        // If the run is in the leaderboard
        if (inLeaderboard)
        {
            Debug.Log("Inside leaderboard!");
            bool statsAddedToLeaderboard = false;
            string username = DatabaseAccountManager.user.Email[..DatabaseAccountManager.user.Email.IndexOf("@")]; // Set part of email before the @ as the username
            LeaderboardInfo info = new()
            {
                Username = username,
                Time = currentRun.Time
            };
            List<LeaderboardInfo> newLeaderboard = new();
            List<Dictionary<string, object>> leaderboardJSON = new(); // Created to convert to JSON later through google's miniJSON due to JsonUtility limitations (can't convert list to JSON directly)

            Debug.Log(newLeaderboard);

            if (leaderboard.Count == 0)
            {
                Debug.Log("nothing in leaderboard");
                newLeaderboard.Add(info);
                leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", info.Username}, {"Time", info.Time} });
            }
            else
            {
                foreach (LeaderboardInfo leaderboardInfo in leaderboard)
                {
                    if (newLeaderboard.Count > 4)
                    {
                        break;
                    }

                    if (leaderboardInfo.Time < info.Time) // Add the leaderboard run if shorter than the run being compared to
                    {
                        newLeaderboard.Add(leaderboardInfo);
                        leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", leaderboardInfo.Username}, {"Time", leaderboardInfo.Time} });
                    }
                    else if (leaderboardInfo.Time >= info.Time) // Add the current run then the remaining leaderboard runs
                    {
                        if (!statsAddedToLeaderboard) // Add the current run
                        {
                            Debug.Log("Adding this");
                            newLeaderboard.Add(info);
                            leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", info.Username}, {"Time", info.Time} });
                            statsAddedToLeaderboard = true;
                        }

                        newLeaderboard.Add(leaderboardInfo); // Add the remaining leaderboard runs
                        leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", leaderboardInfo.Username}, {"Time", leaderboardInfo.Time} });
                    }
                }

                if (newLeaderboard.Count < 5 && !statsAddedToLeaderboard) // Add the current run if leaderboard is still less than 5 and not yet added
                {
                    newLeaderboard.Add(info);
                    leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", info.Username}, {"Time", info.Time} });
                }
            }
            
            leaderboard = newLeaderboard;
            DatabaseManager.SetValueInPath("Leaderboard", Json.Serialize(leaderboardJSON)); // Set leaderboard in database
        }
    }
}

[System.Serializable]
public class LeaderboardInfo
{
    public string Username;
    public int Time;
}