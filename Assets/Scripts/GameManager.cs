using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

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

    void Start()
    {
        if (instance != this)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        StartCoroutine(LoadGameInfoVariables());
    }

    private IEnumerator LoadGameInfoVariables()
    {
        Task gameInfoLoading = GameInfo.FetchGameInfoDatabase();
        yield return new WaitUntil(() => gameInfoLoading.IsCompleted);

        if (gameInfoLoading.IsFaulted)
        {
            loadingErrorText.text = gameInfoLoading.Exception.Message;
            loadingErrorLogObject.SetActive(true);
            mainMenuObjectGroup.SetActive(false);
        } else
        {
            loadingErrorLogObject.SetActive(false);
            mainMenuObjectGroup.SetActive(true);
        }

        
    }
}
 