using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance = null;
    [Header("Panels")]
    [SerializeField]
    private GameObject PreviousRunPanel;
    [SerializeField]
    private GameObject AccountPanel;

    [Header("Main Menu Panels")]
    [SerializeField]
    private GameObject MainMenuPanel;
    [SerializeField]
    private GameObject AccountLoginPanel;
    [SerializeField]
    private GameObject NewGamePanel;
    [SerializeField]
    private AccountPanelManager accountPanelManager;

    private bool ShowPreviousRuns() => DatabaseAccountManager.isAuthenticated() && Player.currentPlayer.playerData.PreviousRuns.Count > 0;

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void SwitchPanel(GameObject panel)
    {
        PreviousRunPanel.SetActive(false);
        AccountPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ReturnToMainPanel()
    {
        PreviousRunPanel.SetActive(ShowPreviousRuns());
        AccountPanel.SetActive(true);
        MainMenuPanel.SetActive(true);
        AccountLoginPanel.SetActive(false);
        NewGamePanel.SetActive(false);
    }
}
