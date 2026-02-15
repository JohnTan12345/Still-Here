// Created by: John
// Description: In-game tasklist UI reference point

using TMPro;
using UnityEngine;

public class TasklistPrefabManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameTaskText;
    [SerializeField]
    private TextMeshProUGUI stepInfoText;
    [SerializeField]
    private TextMeshProUGUI progressText;
    [SerializeField]
    private Transform progressBar;
    
    private string gameTaskName;
    private GameTask gameTask;

    void OnDestroy() // Remove listener when destroyed (stops memory leaks)
    {
        gameTask.onProgressChange -= UpdateUI;
    }
    
    public void CreateUI(string newGameTaskName) // Create UI based on the game task info
    {
        gameTaskName = newGameTaskName;
        gameTask = GameTasks.GetGameTasks()[gameTaskName];
        gameTask.onProgressChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI() // Updates the game task UI to reflect the game task's info
    {
        Debug.Log($"Updating UI for {gameTaskName}");
        gameTaskText.text = gameTaskName;
        stepInfoText.text = gameTask.CurrentStepDescription;
        progressBar.localScale = new Vector3(gameTask.CurrentProgress / gameTask.MaxProgress, 1, 1);

        progressText.text = gameTask.CurrentProgress >= gameTask.MaxProgress ? "Completed" : $"{gameTask.CurrentProgress}/{gameTask.MaxProgress}";
    }
}
