using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance = null;
    
    private Animator animator;

    [Header("Panels")]
    [SerializeField]
    private GameObject LeaderboardPreviousRunPanel;
    [SerializeField]
    private GameObject PreviousRunButton;
    [SerializeField]
    private GameObject AccountPanel;

    [Header("Main Menu Panels")]
    [SerializeField]
    private GameObject MainMenuPanel;
    [SerializeField]
    private GameObject AccountLoginPanel;
    [SerializeField]
    private GameObject NewGamePanel;

    private bool gameLoaded = false;

    private bool ShowPreviousRuns() => DatabaseAccountManager.isAuthenticated() && Player.currentPlayer.playerData.PreviousRuns.Count > 0;

    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void SwitchPanel(GameObject panel)
    {
        animator.Play("From main");
        MainMenuPanel.SetActive(false);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ReturnToMainPanel()
    {
        if (gameLoaded)
        {
            animator.Play("Back to main");
        } else
        {
            gameLoaded = true;
        }
        PreviousRunButton.SetActive(ShowPreviousRuns());
        MainMenuPanel.SetActive(true);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);
    }

    public void LoadMainPanel()
    {
        LeaderboardPreviousRunPanel.SetActive(false);
        AccountPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);

        Debug.Log(JsonUtility.ToJson(Player.currentPlayer, true));
        Debug.Log(ShowPreviousRuns());
        ReturnToMainPanel();

        animator.Play("Loading");
    }
}
