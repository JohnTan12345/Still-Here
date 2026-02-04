using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    // Game Settings
    private int taskAmount = 4;

    // Game Data
    private bool activeGame = false;
    private int time = 0;
    private Coroutine timer;

    // Game Data Accessors
    public int GetTime() => time;

    void Start()
    {
        if (Instance != this)
        {
            if (Instance != null)
            {
                Destroy(Instance);
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

    public void StartGame()
    {
        GameTasks.CreateGameTasks(taskAmount);
        time = 0;

        SceneManager.LoadScene(1);
        activeGame = true;
        timer = StartCoroutine(StartTimer());
        // Task UI Manager start
    }

    public void EndGame()
    {
        activeGame = false;
        timer = null;

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
 