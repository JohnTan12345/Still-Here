using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class LeaderboardManager
{
    private static List<LeaderboardInfo> leaderboard = new List<LeaderboardInfo>();
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
            leaderboard = JsonUtility.FromJson<List<LeaderboardInfo>>(result.snapshot.GetRawJsonValue());
            return leaderboard;
        }
    }
    
    public static void UpdateLeaderboard(Run currentRun)
    {
        float runTime = currentRun.runTime;
        int leaderboardPosition = -1;

        for (int i = 0; i < leaderboard.Count; i++)
        {
            if (runTime < leaderboard[i].Time)
            {
                leaderboardPosition = i;
                break;
            }
        }

        if (leaderboardPosition != 1)
        {
            LeaderboardInfo info = new LeaderboardInfo();
            info.Username = "";
            info.Time = currentRun.runTime;
            List<LeaderboardInfo> newLeaderboard = new List<LeaderboardInfo>();

            for (int i = 0; i < leaderboardLength; i++)
            {
                if (i > leaderboardPosition)
                {
                    newLeaderboard[i] = leaderboard[i-1];
                }
                else if (i == leaderboardPosition)
                {
                    newLeaderboard[i] = info;
                }
                else
                {
                    newLeaderboard[i] = leaderboard[i];
                }
            }
            leaderboard = newLeaderboard;
            DatabaseManager.SetValueInPath("Leaderboard", JsonUtility.ToJson(newLeaderboard));
        }
    }
}

public class LeaderboardInfo
{
    public string Username;
    public float Time;
}