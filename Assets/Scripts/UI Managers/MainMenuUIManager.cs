// Created by: John
// Description: Manages the Main Menu UI Panels

using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance {get; private set;}
    
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

    // Set instance to this and get animator
    void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
        
        animator = GetComponent<Animator>();
    }

    // switches the panel
    public void SwitchPanel(GameObject panel)
    {
        animator.Play("From main");
        MainMenuPanel.SetActive(false);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);

        panel.SetActive(true);
    }

    // Switches the panel back to main menu and enable the account panel and leaderboard panel
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

    // First load of the main panel
    public void LoadMainPanel()
    {
        LeaderboardPreviousRunPanel.SetActive(false);
        AccountPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);
        
        ReturnToMainPanel();

        animator.Play("Loading");
    }
}
