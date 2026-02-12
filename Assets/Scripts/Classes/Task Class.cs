// Created by: John
// Description: Game Tasks handler

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameTasks
{
    private static Dictionary<string, GameTask> gameTasks;
    private static List<string> gameTasksOrder;
    public static List<string> GetGameTasksOrder() => gameTasksOrder;
    public static Dictionary<string, GameTask> GetGameTasks() => gameTasks;

    // Creates a new list of game tasks with specified length
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
            
            gameTasks.Add(gameTask, gameTaskObject); // Point the game task name to the actual game task
            gameTasksOrder.Add(gameTask); // Saves the order the game tasks are in

            gameTaskListSelectables.RemoveAt(gameTaskIndex);
        }
    }

    // Changes set amount to incomplete (if completed) and reorder the tasks if enabled
    public static void ForgetCompletedGameTasks(int amount, bool shuffleOrder = false)
    {
        if (amount <= 0) // If there is no game tasks to begin with
        {
            Debug.LogWarning("You cant just forget nothing");
            return;
        }
        else if (amount >= GetGameTasks().Count) // If trying to reset more than currently implemented
        {
            Debug.LogWarning("You cant just forget more than what you have");
            return;
        }

        List<GameTask> selectableGameTasks = GetGameTasks().Values.ToList(); // Create new available game tasks to choose from to prevent a task getting resetted twice
        
        // Loops through the selectable game tasks to forget
        for (int i = 0; i < amount; i++)
        {
            int selected = Random.Range(0, selectableGameTasks.Count - 1);
            selectableGameTasks[selected].ForgetTaskCompleted();
            selectableGameTasks.RemoveAt(selected);
        }

        // Reorders the game task list
        if (shuffleOrder)
        {
            Debug.Log("Randomizing order");
            List<string> oldTaskOrder = GetGameTasksOrder();
            gameTasksOrder = new List<string>();

            int i = 0; // Counter for infinite loop prevention

            // Loop through the old task order to create a new order
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

    // Start a specific game task
    public static void StartGameTask(string gameTaskName)
    {
        try
        {
            gameTasks[gameTaskName].StartTask(); // Start the game task
        }
        catch (KeyNotFoundException) // Game task is not part of the list
        {
            Debug.Log("Not part of the tasklist");
        }
        catch (System.Exception exception) // Other unforseen errors to be logged
        {
            Debug.LogError(exception);
        }
    }

    // Add progress to a specific step in a specific game task.
    public static void AddGameTaskProgress(string gameTaskName, int stepNumber, float progresesAmount)
    {
        try
        {
            gameTasks[gameTaskName].AddTaskProgress(stepNumber, progresesAmount); // Add progress to a step
        }
        catch (KeyNotFoundException) // Game task is not part of the list
        {
            Debug.LogWarning("Not part of the tasklist");
        }
        catch (System.NullReferenceException) // No task list created
        {
            Debug.LogError("There is no tasklist at all! please create one");
        }
        catch (System.Exception exception) // Other unforseen errors to be logged
        {
            Debug.LogError(exception);
        }
    } 
}

public class GameTask
{
    public event System.Action onProgressChange; // Force the tasklist UI to update when there is a change
    public int CurrentStepCount {get; private set;} = 0;
    public string CurrentStepDescription {get {return Steps[CurrentStepCount].StepDescription;}}
    private float currentProgress = 0; 
    public float CurrentProgress {get {return currentProgress;} private set {currentProgress = value; Debug.Log("Detected change"); onProgressChange?.Invoke();}} // Invoke onProgressChange if there is a change in progress
    public float MaxProgress {get {return Steps[CurrentStepCount].MaxProgress;}} // Returns the max progress for that step
    public bool TaskComplete {get; private set;} = false;
    public int TaskCompletionCount {get; private set;} = 0;
    public List<GameTaskStep> Steps = new List<GameTaskStep>();
    public void CompleteTask(bool value) => TaskComplete = value;

    // Starts the game task
    public void StartTask()
    {
        if (CurrentStepCount == 0)
        {
            CurrentStepCount++;
            CurrentProgress = 0; 
        }
    }

    // Adds progress to a step in the game task
    public void AddTaskProgress(int stepNumber, float progresesAmount)
    {
        if (stepNumber != CurrentStepCount) // Check if the task step to be updated is the current task step
        {
            Debug.Log($"CurrentStepCount > stepNumber: {CurrentStepCount > stepNumber}\nTaskCompletionCount > 0: {TaskCompletionCount > 0}\nBoth: {CurrentStepCount > stepNumber || TaskCompletionCount > 0}\nSteps[stepNumber].GameEndingStep: {Steps[stepNumber].GameEndingStep}\nAll: {(CurrentStepCount > stepNumber || TaskCompletionCount > 0) && Steps[stepNumber].GameEndingStep}");
            
            // End the current run because of overdoing this step
            if ((CurrentStepCount > stepNumber || TaskCompletionCount > 0) && Steps[stepNumber].GameEndingStep)
            {
                EndGameManager.Instance.EndGameSpecial("Overdose"); // Currently only for meds, will add more in the future
            }

            return;
        } else if (stepNumber == 0) // Start the task if step 0
        {
            StartTask();
            return;
        }

        // Add progress if progress is less than max
        if (CurrentProgress < MaxProgress)
        {
            Debug.Log($"Adding task progress of {progresesAmount}");
            CurrentProgress += progresesAmount;
        }
        
        // Increments the task step or complete the game task.
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
                CurrentStepCount = -1;
            }
            
        }

    }

    // Resets the game task completion only
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
