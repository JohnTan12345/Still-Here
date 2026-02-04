using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public static class GameInfo
{
    private static string gameVersion = "1";
    private static Dictionary<string, List<GameTaskStep>> databaseGameTasklist = new Dictionary<string, List<GameTaskStep>>();
    private static Dictionary<string, List<Vector3>> allObjectsPositions = new Dictionary<string, List<Vector3>>();

    public static bool DataLoaded {get; private set;} = false;
    public static string GetGameVersion() => gameVersion;
    public static Dictionary<string, List<GameTaskStep>> GetGameTaskList() => databaseGameTasklist;
    public static List<Vector3> GetObjectPositions(string objectName) => allObjectsPositions[objectName];

    public static async Task FetchGameInfoDatabase()
    {
        DatabaseResult databaseResult = await DatabaseManager.GetValueFromPath("GameInfo");

        if (databaseResult.faulted)
        {
            Debug.LogError($"Something went wrong while fetching game info from database, {databaseResult.errorMessage}");
            return;
        }

        DataSnapshot snapshot = databaseResult.snapshot;
        Debug.Log(snapshot.GetRawJsonValue());

        // Check game version
        if (gameVersion != snapshot.Child("Version").GetValue(false).ToString())
        {
            throw new Exception("Please update the game to a newer version"); 
        }
        
        // Add available game tasks from database
        foreach (var gameTask in snapshot.Child("Tasklist").Children)
        {
            List<GameTaskStep> steps = new List<GameTaskStep>();

            foreach (var step in gameTask.Children)
            {
                GameTaskStep taskStep = JsonUtility.FromJson<GameTaskStep>(step.GetRawJsonValue());
                steps.Add(taskStep);
            }

            databaseGameTasklist.Add(gameTask.Key.ToString(),steps);
        }

        // Add all possible positions for items in database
        foreach (var objectName in snapshot.Child("ObjectInfo").Children)
        {
            List<Vector3> positions = new List<Vector3>();

            foreach (var position in objectName.Children)
            {

                positions.Add(JsonUtility.FromJson<Vector3>(position.GetRawJsonValue()));

            }

            allObjectsPositions.Add(objectName.Key, positions);
        }

        DataLoaded = true;
        Debug.Log("Loaded all data");
    }
}
