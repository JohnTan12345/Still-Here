using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameTasks
{
    private static Dictionary<string, GameTask> gameTasks;
    private static List<string> gameTasksName;

    public static Dictionary<string, GameTask> GetGameTasks() => gameTasks;
    public static void AddGameTaskProgress(string gameTaskName, int stepNumber, float progresesAmount) => gameTasks[gameTaskName].AddTaskProgress(stepNumber, progresesAmount);
    public static void StartGameTask(string gameTaskName) => gameTasks[gameTaskName].StartTask();

    public static void CreateGameTasks(int taskAmount)
    {
        Dictionary<string, List<GameTaskStep>> gameTasklistAndSteps = GameInfo.GetGameTaskList();
        List<string> gameTaskListSelectables = new List<string>();

        gameTasks = new Dictionary<string, GameTask>();
        gameTasksName = new List<string>();

        // Load available tasks
        foreach (string gameTask in gameTasklistAndSteps.Keys)
        {
            gameTaskListSelectables.Add(gameTask);
            Debug.Log("Added");
        }

        // Input 
        for (int i = 0; i < taskAmount; i++)
        {
            int gameTaskIndex = Random.Range(0, gameTaskListSelectables.Count - 1);
            string gameTask = gameTaskListSelectables[gameTaskIndex];
            
            GameTask gameTaskObject = new GameTask();
            gameTaskObject.Steps = gameTasklistAndSteps[gameTask];
            Debug.Log(gameTaskObject.Steps.Count);
            
            gameTasks.Add(gameTask, gameTaskObject);
            gameTasksName.Add(gameTask);

            gameTaskListSelectables.RemoveAt(gameTaskIndex);
        }
    }

    public static void ForgetCompletedGameTasks(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("You cant just forget nothing");
            return;
        }
        else if (amount >= GetGameTasks().Count)
        {
            Debug.LogWarning("You cant just forget more than what you have");
            return;
        }

        List<GameTask> selectableGameTasks = GetGameTasks().Values.ToList();
        

        for (int i = 0; i < amount; i++)
        {
            int selected = Random.Range(0, selectableGameTasks.Count - 1);
            selectableGameTasks[selected].ForgetTaskCompleted();
            selectableGameTasks.RemoveAt(selected);
        }
    }
}

public class GameTask
{
    public int CurrentStepCount {get; private set;} = 0;
    public string CurrentStepDescription {get {return Steps[CurrentStepCount - 1].StepDescription;}}
    public float CurrentProgress {get; private set;} = 0;
    public float MaxProgress {get {return Steps[CurrentStepCount - 1].MaxProgress;}}
    public bool TaskComplete {get; private set;} = false;
    public int TaskCompletionCount {get; private set;} = 0;
    public List<GameTaskStep> Steps = new List<GameTaskStep>();
    public void CompleteTask(bool value) => TaskComplete = value;
    public void StartTask()
    {
        if (CurrentStepCount == 0)
        {
           CurrentStepCount++; 
        }
    }

    public void AddTaskProgress(int stepNumber, float progresesAmount)
    {
        if (stepNumber != CurrentStepCount && stepNumber != 0)
        {
            if (Steps[stepNumber - 1].GameEndingStep)
            {
                // END THAT GAME
            }

            return;
        } else if (stepNumber == 0)
        {
            return;
        }

        CurrentProgress += progresesAmount;
        
        if (CurrentProgress >= MaxProgress)
        {
            if (CurrentStepCount < Steps.Count)
            {
                CurrentStepCount++;
            }
            else
            {
                TaskComplete = true;
                TaskCompletionCount++;
            }

            CurrentProgress = 0f;
            
        }

    }

    public void ForgetTaskCompleted()
    {
        CurrentStepCount = 0;
        TaskComplete = false;
    }
}

[System.Serializable]
public class GameTaskStep
{
    public bool GameEndingStep = false;
    public string StepDescription;
    public float MaxProgress;
}