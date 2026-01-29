using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance() => instance;

    [Header("Main Menu Error Logger")]
    [SerializeField]
    private GameObject loadingErrorLogObject;
    [SerializeField]
    private TextMeshProUGUI loadingErrorText;

    [Header("Loading Screen")]
    [SerializeField]
    private GameObject loadingScreenObject;

    [Header("Main Menu Objects")]
    [SerializeField]
    private GameObject mainMenuObjectGroup;

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
        if (instance != this)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        DatabaseAccountManager.Initialize();

        StartCoroutine(LoadGameInfoVariables());
    }

    private IEnumerator LoadGameInfoVariables()
    {
        Task gameInfoLoading = GameInfo.FetchGameInfoDatabase();
        yield return new WaitUntil(() => gameInfoLoading.IsCompleted);

        if (gameInfoLoading.IsFaulted)
        {
            LoadingErrorLog(gameInfoLoading.Exception.Message);
        }

        Task defaultPlayer = new Player().CreatePlayer();
        yield return new WaitUntil(() => defaultPlayer.IsCompleted);
        
        if (defaultPlayer.IsFaulted)
        {
            LoadingErrorLog(defaultPlayer.Exception.Message);
        }

        yield return new WaitUntil(() => MainMenuUIManager.instance != null);
        MainMenuUIManager.instance.ReturnToMainPanel();
    }

    private void LoadingErrorLog(string message)
    {
        loadingErrorText.text = message;
        loadingErrorLogObject.SetActive(true);
        mainMenuObjectGroup.SetActive(false);
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
 