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

    void Awake()
    {
        viewport = tasklistScrollView.transform.Find("Viewport");
        content = viewport.Find("Content");
        contentCopy = Instantiate(content);

        LoadCurrentRunStats();
    }

    private void LoadCurrentRunStats()
    {
        Destroy(content.gameObject);
        content = Instantiate(contentCopy, viewport);
        tasklistScrollView.content = content.GetComponent<RectTransform>();

        bool allTasksComplete = true;

        foreach (TaskInfo task in Player.currentPlayer.currentRun.Tasklist)
        {
            if (allTasksComplete)
            {
                allTasksComplete = task.CompletionCount > 0;
            }
            GameObject taskUI = Instantiate(endGameTaskUIPrefab, content);
            taskUI.GetComponent<EndGameTaskUIVariables>().UpdateUI(task);
        }

        completionStatusText.text = allTasksComplete ? "You finished all your tasks!" : "You did not finish all your tasks...";
        timeText.text = $"Time: {Player.currentPlayer.currentRun.GetTimeString()}";
    }

    public void GoMainMenu()
    {
        LeaderboardManager.UpdateLeaderboard(Player.currentPlayer.currentRun);

        if (Player.currentPlayer.playerData.PreviousRuns.Count >= 5)
        {
            Player.currentPlayer.playerData.PreviousRuns.RemoveAt(0);
        }

        Player.currentPlayer.playerData.PreviousRuns.Add(Player.currentPlayer.currentRun);
        DatabaseManager.SaveUserDataAsync(Player.currentPlayer);

        Player.currentPlayer.currentRun = null;

        GameManager.Instance.ReturnMainMenu();
    }
}
