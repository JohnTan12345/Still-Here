using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameTasks
{
    private static Dictionary<string, GameTask> gameTasks;
    private static List<string> gameTasksOrder;
    public static List<string> GetGameTasksOrder() => gameTasksOrder;
    public static Dictionary<string, GameTask> GetGameTasks() => gameTasks;
    public static void AddGameTaskProgress(string gameTaskName, int stepNumber, float progresesAmount) => gameTasks[gameTaskName].AddTaskProgress(stepNumber, progresesAmount);
    public static void StartGameTask(string gameTaskName) => gameTasks[gameTaskName].StartTask();

    public static void CreateGameTasks(int taskAmount)
    {
        Dictionary<string, List<GameTaskStep>> gameTasklistAndSteps = GameInfo.GetGameTaskList();
        List<string> gameTaskListSelectables = new List<string>();

        gameTasks = new Dictionary<string, GameTask>();
        gameTasksOrder = new List<string>();

        // Load available tasks
        foreach (string gameTask in gameTasklistAndSteps.Keys)
        {
            gameTaskListSelectables.Add(gameTask);
        }

        // Input 
        for (int i = 0; i < taskAmount; i++)
        {
            int gameTaskIndex = Random.Range(0, gameTaskListSelectables.Count - 1);
            string gameTask = gameTaskListSelectables[gameTaskIndex];
            
            GameTask gameTaskObject = new GameTask();
            gameTaskObject.Steps = gameTasklistAndSteps[gameTask];
            
            gameTasks.Add(gameTask, gameTaskObject);
            gameTasksOrder.Add(gameTask);

            gameTaskListSelectables.RemoveAt(gameTaskIndex);
        }
    }

    public static void ForgetCompletedGameTasks(int amount, bool shuffleOrder = false)
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

        if (shuffleOrder)
        {
            Debug.Log("Randomizing order");
            List<string> oldTaskOrder = GetGameTasksOrder();
            gameTasksOrder = new List<string>();

            int i = 0; // Counter

            while (oldTaskOrder.Count > 0)
            {
                int oldPosition = Random.Range(0, oldTaskOrder.Count - 1);
                gameTasksOrder.Add(oldTaskOrder[oldPosition]);
                oldTaskOrder.RemoveAt(oldPosition);

                // Just in case theres a infinite loop for whatever reason
                i++;
                if (i == 50)
                {
                    Debug.LogError("Infinite loop detected. Stopping game");
                    throw new System.Exception("Infinite loop detected. Stopping game");
                }
            }

            Debug.Log("Finished randomizing order");
        }
    }
}

public class GameTask
{
    public int CurrentStepCount {get; private set;} = 0;
    public string CurrentStepDescription {get {return Steps[CurrentStepCount].StepDescription;}}
    public float CurrentProgress {get; private set;} = 0;
    public float MaxProgress {get {return Steps[CurrentStepCount].MaxProgress;}}
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
            if (Steps[stepNumber].GameEndingStep)
            {
                // END THAT GAME
            }

            return;
        } else if (stepNumber == 0)
        {
            StartTask();
            return;
        }

        if (CurrentProgress < MaxProgress)
        {
            CurrentProgress += progresesAmount;
        }
        
        if (CurrentProgress >= MaxProgress)
        {
            if (CurrentStepCount < Steps.Count - 1)
            {
                CurrentStepCount++;
                CurrentProgress = 0f;
            }
            else
            {
                TaskComplete = true;
                TaskCompletionCount++;
            }
            
        }

    }

    public void ForgetTaskCompleted()
    {
        if (TaskComplete)
        {
           CurrentStepCount = 0;
            TaskComplete = false;
            CurrentProgress = 0f; 
        }
        
    }
}

[System.Serializable]
public class GameTaskStep
{
    public bool GameEndingStep = false;
    public string StepDescription;
    public float MaxProgress;
}