// Created by: John
// Description: Manages the UI in the end game scene

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI completionStatusText;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private ScrollRect tasklistScrollView;

    private Transform viewport;
    private Transform content;
    private Transform contentCopy;
    [SerializeField]
    private GameObject endGameTaskUIPrefab;

    private bool allTasksComplete = true;

    // Set variables for when game task stats are being added later
    void Awake()
    {
        viewport = tasklistScrollView.transform.Find("Viewport");
        content = viewport.Find("Content");
        contentCopy = Instantiate(content);

        LoadCurrentRunStats();
    }

    // Loads the current run's game tasks stats into the UI
    private void LoadCurrentRunStats()
    {
        Destroy(content.gameObject);
        content = Instantiate(contentCopy, viewport);
        tasklistScrollView.content = content.GetComponent<RectTransform>();

        // Add the prefab UI into the content group
        foreach (TaskInfo task in Player.currentPlayer.currentRun.Tasklist)
        {
            if (allTasksComplete)
            {
                allTasksComplete = task.CompletionCount > 0;
            }
            GameObject taskUI = Instantiate(endGameTaskUIPrefab, content);
            taskUI.GetComponent<EndGameTaskUIVariables>().UpdateUI(task);
        }

        string endText = "";

        // Update end day status based on how the day ended
        switch (GameManager.Instance.specialEnding)
        {
            case 0:
                endText = allTasksComplete ? "You finished all your tasks!" : "You did not finish all your tasks...";
                break;
            case 1:
                endText = "You overdosed on pills...";
                break;
        }

        completionStatusText.text = endText;
        timeText.text = $"Time: {Player.currentPlayer.currentRun.GetTimeString()}";
    }

    // Updates leaderboard, previous runs, endings, player data if logged in and returns to main menu
    public void GoMainMenu()
    {
        if (GameManager.Instance.specialEnding == 0 && allTasksComplete)
        {
            LeaderboardManager.UpdateLeaderboard(Player.currentPlayer.currentRun);
        }
        
        if (Player.currentPlayer.playerData.PreviousRuns.Count >= 5)
        {
            Player.currentPlayer.playerData.PreviousRuns.RemoveAt(0);
        }

        Player.currentPlayer.playerData.PreviousRuns.Add(Player.currentPlayer.currentRun);

        if (allTasksComplete && !Player.currentPlayer.playerData.Endings.Contains("Ending 1"))
        {
            Player.currentPlayer.playerData.Endings.Add("Ending 1");
        }
        else if (GameManager.Instance.specialEnding != 0 && !Player.currentPlayer.playerData.Endings.Contains($"Ending {GameManager.Instance.specialEnding + 1}"))
        {
            Player.currentPlayer.playerData.Endings.Add($"Ending {GameManager.Instance.specialEnding + 1}");
        }

        DatabaseManager.SaveUserDataAsync(Player.currentPlayer);

        Player.currentPlayer.currentRun = null;

        GameManager.Instance.ReturnMainMenu();
    }
}
