// Created by: John
// Description: Manages the concurrent game and initiates stuff

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    // Testing purposes
    [Header("Testing Parameters")]
    public bool testing;
    public int taskAmount_test = 7;
    public int forgetFrequency_test = 60;
    public bool changeTasklistOrder_test = true;

    [Header("Game Settings not in UI")]
    public float ObjectRelocationTime = 30;

    // Game Data
    private bool loadingGame = false;
    public bool activeGame {get; private set;} = false;
    private int time = 0;

    public int specialEnding {get; private set;} = 0;

    private DifficultySetting gameSettings;

    // Game Data Accessors
    public int GetTime() => time;

    // Converts the time into to the XX:XX format
    public string GetTimeString()
    {
        if (time < 60)
        {
            return $"00:{(time >= 10 ? time : "0" + time.ToString())}";
        }
        int TimeSeconds = time % 60;
        int TimeMinutes = (int)Mathf.Floor(time/60);

        return $"{(TimeMinutes >=10 ? TimeMinutes : "0"+ TimeMinutes)}:{(TimeSeconds >= 10 ? TimeSeconds : "0" + TimeSeconds.ToString())}";
    }

    // Initialize scripts
    void Awake()
    {
        if (Instance != this)
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        DatabaseAccountManager.Initialize();

        StartCoroutine(LoadGameInfoVariables());
    }

    // Load game tasks list, creates a new player and loads main menu. If testing, starts a new game
    private IEnumerator LoadGameInfoVariables()
    {
        Task gameInfoLoadingTask = GameInfo.FetchGameInfoDatabase();
        yield return new WaitUntil(() => gameInfoLoadingTask.IsCompleted);

        if (gameInfoLoadingTask.IsFaulted)
        {
            Debug.LogError(gameInfoLoadingTask.Exception);
            if (MainMenuSceneUIManager.Instance != null)
            {
                MainMenuSceneUIManager.Instance.LoadingErrorLog(gameInfoLoadingTask.Exception.Message);
            }
            
        }

        Task CreatePlayerTask = new Player().CreatePlayer();
        yield return new WaitUntil(() => CreatePlayerTask.IsCompleted);
        
        if (CreatePlayerTask.IsFaulted)
        {
            Debug.LogError(CreatePlayerTask.Exception.Message);
            if (MainMenuSceneUIManager.Instance != null)
            {
                MainMenuSceneUIManager.Instance.LoadingErrorLog(CreatePlayerTask.Exception.Message);
            }
        }

        if (MainMenuUIManager.Instance != null)
        {
            MainMenuUIManager.Instance.LoadMainPanel();
        }

        if (testing)
        {
            Debug.Log("Testing Enabled");
            DifficultySetting difficultySetting = new()
            {
                taskAmount = taskAmount_test,
                forgetFrequency = forgetFrequency_test,
                changeTasklistOrder = changeTasklistOrder_test
            };

            StartCoroutine(StartGame(difficultySetting));

            if (TasklistUIManager.Instance != null)
            {
                TasklistUIManager.Instance.CreateGameTasks();
            }
        }
    }

    // Starts a new game with the given settings
    public IEnumerator StartGame(DifficultySetting difficultySetting)
    {
        Debug.Log("Starting new game");

        // Stop the function if the game is being loaded
        if (loadingGame)
        {
            yield break;
        } else
        {
            loadingGame = true;
        }
        
        // Resets the game data if there was a run before
        specialEnding = 0;
        gameSettings = difficultySetting;
        GameTasks.CreateGameTasks(difficultySetting.taskAmount); // Loads new set of game tasks
        Debug.Log("Tasks Created Successfully");
        time = 0;
        
        activeGame = true;

        if (!testing) // Load new scene
        {
            SceneManager.LoadSceneAsync(1);
        }
        
        yield return null;

        loadingGame = false;
        
        // Starts the in-game timer and "forget" mechanic timer
        StartCoroutine(StartTimer());
        StartCoroutine(StartForgetTImer());
    }

    // Ends the game
    public void EndGame(GameObject loadingScreen)
    {
        SaveGameData();

        Animator loadingScreenAnimator;

        loadingScreen.TryGetComponent(out loadingScreenAnimator);

        // Load the end scene if there is no animation
        if (loadingScreenAnimator != null)
        {
            Debug.Log("Animator found!");
            loadingScreenAnimator.Play("Fade In");
        }
        else
        {
            LoadEndScene();
        }
    }

    // Ends the game in a special way
    public void EndGameSpecial(string endReason, GameObject loadingScreen)
    {
        SaveGameData();

        Animator loadingScreenAnimator;

        loadingScreen.TryGetComponent(out loadingScreenAnimator);

        // Set the special ending and plays the animation, loads the scene if no animation at all
        switch (endReason)
        {
            case "Overdose":
            specialEnding = 1;
                if (loadingScreenAnimator != null)
                {
                    Debug.Log("Animator found!");
                    loadingScreenAnimator.Play("Pills Blackout");
                }
                else
                {
                    LoadEndScene();
                }
                break;
        }
    }

    // Saves the player data
    private void SaveGameData()
    {
        activeGame = false;
        
        List<TaskInfo> currentRunTasklist = new List<TaskInfo>();
        foreach (string gameTaskName in GameTasks.GetGameTasksOrder())
        {
            TaskInfo newTaskInfo = new TaskInfo
            {
                TaskName = gameTaskName,
                CompletionCount = GameTasks.GetGameTasks()[gameTaskName].TaskCompletionCount
            };

            currentRunTasklist.Add(newTaskInfo);
        }

        Player.currentPlayer.currentRun = new Run()
        {
            Tasklist = currentRunTasklist,
            Time = time
        };
    }

    // Loads the end scene
    public void LoadEndScene()
    {
        SceneManager.LoadScene(2);
    }

    // Loads the main menu scene
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Starts the in-game timer while game is active
    private IEnumerator StartTimer()
    {
        while (activeGame)
        {
            yield return new WaitForSecondsRealtime(1);
            time++;
        }
    }

    // Starts the in-game timer for the "forget" mechanic while the game is active
    private IEnumerator StartForgetTImer()
    {
        int forgetTime = 0;
        while (activeGame)
        {
            yield return new WaitForSecondsRealtime(1);
            forgetTime++;

            if (forgetTime >= gameSettings.forgetFrequency)
            {
                GameTasks.ForgetCompletedGameTasks(Random.Range(1, GameTasks.GetGameTasks().Count - 1), gameSettings.changeTasklistOrder);
                forgetTime = 0;
            }
        }
    }
}
 