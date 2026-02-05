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

    void OnDestroy()
    {
        gameTask.onProgressChange -= UpdateUI;
    }
    
    public void CreateUI(string newGameTaskName)
    {
        gameTaskName = newGameTaskName;
        gameTask = GameTasks.GetGameTasks()[gameTaskName];
        gameTask.onProgressChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log($"Updating UI for {gameTaskName}");
        gameTaskText.text = gameTaskName;
        stepInfoText.text = gameTask.CurrentStepDescription;
        progressBar.localScale = new Vector3(gameTask.CurrentProgress / gameTask.MaxProgress, 1, 1);

        progressText.text = gameTask.CurrentProgress >= gameTask.MaxProgress ? "Completed" : $"{gameTask.CurrentProgress}/{gameTask.MaxProgress}";
    }
}
