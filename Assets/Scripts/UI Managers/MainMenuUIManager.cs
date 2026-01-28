using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
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

    private bool ShowPreviousRuns() => DatabaseAccountManager.isAuthenticated() && Player.currentPlayer.playerData.PreviousRuns.Count > 0;

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

    public void SignOut()
    {
        DatabaseAccountManager.SignOutAccount();
        ReturnToMainPanel();
    }
}
