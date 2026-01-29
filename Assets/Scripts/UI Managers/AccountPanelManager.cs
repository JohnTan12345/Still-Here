using TMPro;
using UnityEngine;

public class AccountPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject notLoggedInPanel;
    [SerializeField]
    private GameObject loggedInPanel;

    [SerializeField]
    private TextMeshProUGUI loggedInMessage;

    void OnEnable()
    {
        if (DatabaseAccountManager.isAuthenticated())
        {
            notLoggedInPanel.SetActive(false);
            loggedInPanel.SetActive(true);

            loggedInMessage.text = $"Welcome, {DatabaseAccountManager.user.Email}";
        }
        else
        {
            notLoggedInPanel.SetActive(true);
            loggedInPanel.SetActive(false);
        }
    }

    public void OnSignOut()
    {
        DatabaseAccountManager.SignOutAccount();
        notLoggedInPanel.SetActive(true);
        loggedInPanel.SetActive(false);
        MainMenuUIManager.instance.ReturnToMainPanel();
    }
}
