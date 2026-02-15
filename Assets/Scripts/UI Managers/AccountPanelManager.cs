// Created by: John
// Description: Account UI Panel Manager

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AccountPanelManager : MonoBehaviour
{
    public static AccountPanelManager Instance {get; set;}

    [SerializeField]
    private GameObject notLoggedInPanel;
    [SerializeField]
    private GameObject loggedInPanel;
    [SerializeField]
    private TextMeshProUGUI loggedInMessage;
    
    public UnityEvent onAccountPanelEnable;

    void Start()
    {
        if (Instance != this)
        {
            Instance = this;
        }  
    }

    // Check if the player is logged in and update accordingly
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

    // Set the panel back to "not logged in"
    public void OnSignOut()
    {
        DatabaseAccountManager.SignOutAccount();
        notLoggedInPanel.SetActive(true);
        loggedInPanel.SetActive(false);
    }
}
