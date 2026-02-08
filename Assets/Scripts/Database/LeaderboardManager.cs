using System.Collections.Generic;
using System.Threading.Tasks;
using Google.MiniJSON;
using UnityEngine;

public static class LeaderboardManager
{
    public static List<LeaderboardInfo> leaderboard {get; private set;}
    private static int leaderboardLength = 5;
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
            foreach(var child in result.snapshot.Children)
            {
                LeaderboardInfo leaderboardInfo = JsonUtility.FromJson<LeaderboardInfo>(child.GetRawJsonValue());
                leaderboard.Add(leaderboardInfo);
            }
            return leaderboard;
        }
    }
    
    public static void UpdateLeaderboard(Run currentRun)
    {
        if (!DatabaseAccountManager.isAuthenticated())
        {
            Debug.Log("No user logged in");
            return;
        }

        int runTime = currentRun.Time;
        bool inLeaderboard = false;

        if (leaderboard.Count < 5)
        {
            inLeaderboard = true;
        }
        else
        {
            for (int i = 0; i < leaderboard.Count; i++)
            {
                if (runTime < leaderboard[i].Time)
                {
                    inLeaderboard = true;
                    break;
                }
            }
        }

        

        if (inLeaderboard)
        {
            Debug.Log("Inside leaderboard!");
            bool statsAddedToLeaderboard = false;
            string username = DatabaseAccountManager.user.Email[..DatabaseAccountManager.user.Email.IndexOf("@")];
            LeaderboardInfo info = new()
            {
                Username = username,
                Time = currentRun.Time
            };
            List<LeaderboardInfo> newLeaderboard = new();
            List<Dictionary<string, object>> leaderboardJSON = new();

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

                    if (leaderboardInfo.Time < info.Time)
                    {
                        newLeaderboard.Add(leaderboardInfo);
                        leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", leaderboardInfo.Username}, {"Time", leaderboardInfo.Time} });
                    }
                    else if (leaderboardInfo.Time >= info.Time)
                    {
                        if (!statsAddedToLeaderboard)
                        {
                            Debug.Log("Adding this");
                            newLeaderboard.Add(info);
                            leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", info.Username}, {"Time", info.Time} });
                            statsAddedToLeaderboard = true;
                        }

                        newLeaderboard.Add(leaderboardInfo);
                        leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", leaderboardInfo.Username}, {"Time", leaderboardInfo.Time} });
                    }
                }

                if (newLeaderboard.Count < 5 && !statsAddedToLeaderboard)
                {
                    newLeaderboard.Add(info);
                    leaderboardJSON.Add(new Dictionary<string, object>() { {"Username", info.Username}, {"Time", info.Time} });
                }
            }
            
            leaderboard = newLeaderboard;
            DatabaseManager.SetValueInPath("Leaderboard", Json.Serialize(leaderboardJSON));
        }
    }
}

[System.Serializable]
public class LeaderboardInfo
{
    public string Username;
    public int Time;
}