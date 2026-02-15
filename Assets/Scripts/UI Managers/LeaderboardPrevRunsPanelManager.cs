// Created by: John
// Description: Updates the leaderboard page and previous runs page

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPrevRunsPanelManager : MonoBehaviour
{
    [Header("Leaderboard")]
    [SerializeField]
    private GameObject leaderboardErrorMessage;
    [SerializeField]
    private GameObject placementGameObjectPrefab;
    [SerializeField]
    private Transform placementGameObjectParent;

    [Header("Previous Runs")]
    [SerializeField]
    private TextMeshProUGUI previousRunHeader;
    [SerializeField]
    private TextMeshProUGUI previousRunTimeText;
    [SerializeField]
    private GameObject tasklistPanel;
    [SerializeField]
    private GameObject previousButtonObject;
    [SerializeField]
    private GameObject nextButtonObject;
    [SerializeField]
    private GameObject recentButtonObject;
    [SerializeField]
    private GameObject viewPreviousRunsButtonObject;
    [SerializeField]
    private GameObject viewLeaderboardButtonObject;
    [SerializeField]
    private GameObject previousTaskUIPrefab;

    private GameObject tasklistViewport;
    private GameObject tasklistFrameCopy;
    private GameObject tasklistFrame;
    private int currentRunPage;
    private int totalPages;

    // Resets the whole UI to starting state
    void Start()
    {
        previousButtonObject.SetActive(false);
        nextButtonObject.SetActive(false);
        recentButtonObject.SetActive(false);
        viewPreviousRunsButtonObject.SetActive(true);
        viewLeaderboardButtonObject.SetActive(true);
        StartCoroutine(LoadLeaderboard());

        tasklistViewport = tasklistPanel.transform.Find("Viewport").gameObject;
        tasklistFrameCopy = Instantiate(tasklistViewport.transform.GetChild(0).gameObject);
    }

    // Loads the leaderboard panel's leaderboard
    private IEnumerator LoadLeaderboard()
    {
        Task<List<LeaderboardInfo>> getLeaderboardTask = LeaderboardManager.GetLeaderboard();
        yield return new WaitUntil(() => getLeaderboardTask.IsCompleted);

        
        if (getLeaderboardTask.IsFaulted || getLeaderboardTask.Result.Count < 1) // If there was an error in fetching leaderboard data or there is no leaderboard
        {
            leaderboardErrorMessage.SetActive(true);
            Debug.LogError(getLeaderboardTask.Exception);
        }
        else // Add in all leaderboard runs and put it inside the leaderboard content group
        {
            leaderboardErrorMessage.SetActive(false);
            
            List<LeaderboardInfo> leaderboard = getLeaderboardTask.Result;

            for (int i = 0; i < leaderboard.Count; i++)
            {
                GameObject leaderboardPlacement = Instantiate(placementGameObjectPrefab, placementGameObjectParent);
                LeaderboardPlacementVariables leaderboardPlacementVariables = leaderboardPlacement.GetComponent<LeaderboardPlacementVariables>();
                leaderboardPlacementVariables.namePlacement.text = $"#{i+1}: {leaderboard[i].Username}";

                // Turns the time from int to XX:XX string format
                string timeText = "";

                if (leaderboard[i].Time < 60)
                {
                    timeText = $"00:{(leaderboard[i].Time >= 10 ? leaderboard[i].Time : "0" + leaderboard[i].Time.ToString())}";
                }
                int TimeSeconds = leaderboard[i].Time % 60;
                int TimeMinutes = (int)Mathf.Floor(leaderboard[i].Time/60);

                timeText =  $"{(TimeMinutes >=10 ? TimeMinutes : "0"+ TimeMinutes)}:{(TimeSeconds >= 10 ? TimeSeconds : "0" + TimeSeconds.ToString())}";

                leaderboardPlacementVariables.timeText.text = $"Time: {timeText}";
            }
        }

        Debug.LogWarning(AccountPanelManager.Instance != null);

        AccountPanelManager.Instance.onAccountPanelEnable.AddListener(LoadPlayerPreviousRuns);
        LoadPlayerPreviousRuns();
    }

    // Loads the previous runs inside the current player
    private void LoadPlayerPreviousRuns()
    {
        if (!DatabaseAccountManager.isAuthenticated())
        {
            Debug.Log("No user logged in");
        }
        Debug.Log("Loading player data");
        currentRunPage = 0;

        List<Run> previousRuns = Player.currentPlayer.playerData.PreviousRuns;

        if (previousRuns.Count != 0)
        {
            if (previousRuns.Count == 1) // Sets previous runs button to uninteractable if theres only one run
            {
                viewPreviousRunsButtonObject.GetComponent<Button>().interactable = false;
            }
            else // Sets previous runs button to interactable if theres more than one run
            {
                viewPreviousRunsButtonObject.GetComponent<Button>().interactable = true;
                totalPages = previousRuns.Count - 1;
            }

            UpdatePreviousRunPanel(true);
        }
    }

    // View next run
    public void NextRunPage()
    {
        if (currentRunPage + 1 > totalPages)
        {
            currentRunPage = 0;
        }
        else
        {
            currentRunPage++;
        }

        UpdatePreviousRunPanel(false);
    }

    // View previous run
    public void PreviousRunPage()
    {
        if (currentRunPage - 1 < 0)
        {
            currentRunPage = totalPages;
        }
        else
        {
            currentRunPage--;
        }

        UpdatePreviousRunPanel(false);
    }

    // Goes back to the most recent run panel
    public void ViewRecentRunPage()
    {
        currentRunPage = 0;
        UpdatePreviousRunPanel(true);
        nextButtonObject.SetActive(false);
        previousButtonObject.SetActive(false);
        recentButtonObject.SetActive(false);
        viewLeaderboardButtonObject.SetActive(true);
        viewPreviousRunsButtonObject.SetActive(true);
    }

    // Goes to the most recent run panel
    public void ViewPreviousRunPage()
    {
        UpdatePreviousRunPanel(false);
        nextButtonObject.SetActive(true);
        previousButtonObject.SetActive(true);
        recentButtonObject.SetActive(true);
        viewLeaderboardButtonObject.SetActive(false);
        viewPreviousRunsButtonObject.SetActive(false);
    }

    // Adds in the recent runs UI into the Previous Run Panel content group
    private void UpdatePreviousRunPanel(bool loadOnPreviousRunPanel)
    {
        if (loadOnPreviousRunPanel)
        {
            previousRunHeader.text = "Most Recent Run";
        }
        else
        {
            previousRunHeader.text = $"Run #{currentRunPage + 1}";
        }
        
        previousRunTimeText.text = $"Time: {Player.currentPlayer.playerData.PreviousRuns[currentRunPage].GetTimeString()}";

        // Removes the content group then reinitalizes it again
        Destroy(tasklistViewport.transform.GetChild(0).gameObject);
        tasklistFrame = Instantiate(tasklistFrameCopy, tasklistViewport.transform);
        tasklistPanel.GetComponent<ScrollRect>().content = tasklistFrame.GetComponent<RectTransform>();

        // Adds in the recent runs UI and update the UI with the stats
        foreach (TaskInfo task in Player.currentPlayer.playerData.PreviousRuns[currentRunPage].Tasklist)
        {
            GameObject newPreviousTaskUI = Instantiate(previousTaskUIPrefab, tasklistFrame.transform);
            PreviousRunTaskVariables previousRunTaskVariables = newPreviousTaskUI.GetComponent<PreviousRunTaskVariables>();
            previousRunTaskVariables.taskNameText.text = task.TaskName;
            previousRunTaskVariables.completionCountText.text = $"{(task.CompletionCount != 0 ? $"Completed {task.CompletionCount} {(task.CompletionCount == 1 ? "time" : "times")}" : "Not completed")}";
        }
    }
}
