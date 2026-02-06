using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [Header("Testing Parameters")]
    public bool testing;
    public int taskAmount_test = 7;
    public int forgetFrequency_test = 60;
    public bool changeTasklistOrder_test = true;

    // Game Data
    private bool loadingGame = false;
    private bool activeGame = false;
    private int time = 0;

    private DifficultySetting gameSettings;

    // Game Data Accessors
    public int GetTime() => time;

    void Start()
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
            DifficultySetting difficultySetting = new DifficultySetting();
            difficultySetting.taskAmount = taskAmount_test;
            difficultySetting.forgetFrequency = forgetFrequency_test;
            difficultySetting.changeTasklistOrder = changeTasklistOrder_test;

            StartCoroutine(StartGame(difficultySetting));
        }
    }

    public IEnumerator StartGame(DifficultySetting difficultySetting)
    {
        Debug.Log("Starting new game");
        if (loadingGame)
        {
            yield break;
        } else
        {
            loadingGame = true;
        }
        
        gameSettings = difficultySetting;
        GameTasks.CreateGameTasks(difficultySetting.taskAmount);
        Debug.Log("Tasks Created Successfully");
        time = 0;
        
        activeGame = true;

        if (!testing)
        {
            SceneManager.LoadSceneAsync(1);
        }
        
        yield return null;

        loadingGame = false;
        StartCoroutine(StartTimer());
        StartCoroutine(StartForgetTImer());
    }

    public void EndGame(GameObject loadingScreen)
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

        // Reset Game Data
        time = 0;
        

        Animator loadingScreenAnimator;

        loadingScreen.TryGetComponent(out loadingScreenAnimator);

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

    public void LoadEndScene()
    {
        SceneManager.LoadScene(2);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartTimer()
    {
        while (activeGame)
        {
            yield return new WaitForSecondsRealtime(1);
            time++;
        }
    }

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
 