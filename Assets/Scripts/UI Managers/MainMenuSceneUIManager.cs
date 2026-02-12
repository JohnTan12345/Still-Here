// Created by: John
// Description: Where the error UI is handled

using TMPro;
using UnityEngine;

public class MainMenuSceneUIManager : MonoBehaviour
{
    public static MainMenuSceneUIManager Instance {get; private set;}

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
        if (Instance != this)
        {
            Instance = this;
        }
    }

    // Show the error message in game
    public void LoadingErrorLog(string message)
    {
        loadingErrorText.text = message;
        loadingErrorLogObject.SetActive(true);
        mainMenuObjectGroup.SetActive(false);
    }
}
