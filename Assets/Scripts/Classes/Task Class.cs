using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

/* 
    TODO

    1. Implement Steps for a task
    2. Make the task have the task name as header, steps with max progress in a list under it.
    3. Somehow translate that to the game task.

*/
public static class GameTasks
{
    private static List<string> gameTaskList;
    private static Dictionary<string, GameTask> gameTasks;

    public static Dictionary<string, GameTask> GetGameTasks() => gameTasks;

    public static void CreateGameTasks()
    {
        
    }

    public async static Task FetchGameTasklistFromDatabase()
    {
        DatabaseResult databaseResult = await DatabaseManager.GetValueFromPath("GameInfo");
        
        if (databaseResult.faulted)
        {
            return;
        }
        
        List<string> databaseGameTaskList = new List<string>();

        foreach (DataSnapshot child in databaseResult.snapshot.Child("TaskList").Children)
        {
            if (child.Value != null)
            {
                databaseGameTaskList.Add(child.Value.ToString());
            }
        }

    }
}

public class GameTask
{
    private List<GameTaskStep> Steps = new List<GameTaskStep>();
    public void StartTask()
    {
        
    }

    public void AddTaskProgress()
    {
        
    }
}

public class GameTaskStep
{
    public string StepInfo;
    public float MaxProgreses;
}