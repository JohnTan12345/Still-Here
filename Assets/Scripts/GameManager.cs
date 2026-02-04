using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    // Game Data
    private bool loadingGame = false;
    private bool activeGame = false;
    private int time = 0;
    private Coroutine timeKeeper;
    private Coroutine forgetTimer;

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
    }

    public IEnumerator StartGame(DifficultySetting difficultySetting)
    {
        if (loadingGame)
        {
            yield break;
        } else
        {
            loadingGame = true;
        }
        
        gameSettings = difficultySetting;
        GameTasks.CreateGameTasks(difficultySetting.taskAmount);
        yield return new WaitUntil(() => GameTasks.GetGameTasks() != null || GameTasks.GetGameTasks().Count != 0);
        Debug.Log("Tasks Created Successfully");
        time = 0;
        
        activeGame = true;
        SceneManager.LoadScene(1);
        loadingGame = false;
        timeKeeper = StartCoroutine(StartTimer());
        forgetTimer = StartCoroutine(StartForgetTImer());
    }

    public void EndGame()
    {
        activeGame = false;
        timeKeeper = null;

        // Show end game UI
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
    
    public void RepositionObject(GameObject gameObject)
    {
        try
        {
            List<Vector3> positionList = GameInfo.GetObjectPositions(gameObject.name);
            
            Vector3 newPosition = positionList[Random.Range(0, positionList.Count -1)];

            gameObject.transform.position = newPosition;
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning("This object does not exist inside the list");
        }
    }
}
 