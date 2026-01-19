using System.Collections.Generic;

public static class Leaderboard
{
    private static List<LeaderboardInfo> leaderboard = new List<LeaderboardInfo>();
    
    public static List<LeaderboardInfo> GetLeaderboard() => leaderboard;
}

public class LeaderboardInfo
{
    public string Username;
    public string Time;
}