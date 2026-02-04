using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AccountPanelManager : MonoBehaviour
{
    public static AccountPanelManager instance;

    [SerializeField]
    private GameObject notLoggedInPanel;
    [SerializeField]
    private GameObject loggedInPanel;
    [SerializeField]
    private TextMeshProUGUI loggedInMessage;
    
    public UnityEvent onAccountPanelEnable;

    void Start()
    {
        if (instance != this)
        {
            instance = this;
        }  
    }

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

        onAccountPanelEnable.Invoke();
    }

    public void OnSignOut()
    {
        DatabaseAccountManager.SignOutAccount();
        notLoggedInPanel.SetActive(true);
        loggedInPanel.SetActive(false);
    }
}
