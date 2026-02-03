using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardPrevRunsPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardErrorMessage;
    [SerializeField]
    private GameObject placementGameObjectPrefab;
    [SerializeField]
    private Transform placementGameObjectParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadLeaderboard());
    }

    private IEnumerator LoadLeaderboard()
    {
        Task<List<LeaderboardInfo>> getLeaderboardTask = LeaderboardManager.GetLeaderboard();
        yield return new WaitUntil(() => getLeaderboardTask.IsCompleted);

        if (getLeaderboardTask.IsFaulted || getLeaderboardTask.Result.Count < 1)
        {
            leaderboardErrorMessage.SetActive(true);
            Debug.LogError(getLeaderboardTask.Exception);
        }
        else
        {
            leaderboardErrorMessage.SetActive(false);
            
            List<LeaderboardInfo> leaderboard = getLeaderboardTask.Result;

            for (int i = 0; i < leaderboard.Count; i++)
            {
                GameObject leaderboardPlacement = Instantiate(placementGameObjectPrefab, placementGameObjectParent);
                LeaderboardPlacementVariables leaderboardPlacementVariables = leaderboardPlacement.GetComponent<LeaderboardPlacementVariables>();
                leaderboardPlacementVariables.namePlacement.text = $"#{i+1}: {leaderboard[i].Username}";
                leaderboardPlacementVariables.timeText.text = $"Time: {leaderboard[i].Time}";
            }
        }
    }
}
