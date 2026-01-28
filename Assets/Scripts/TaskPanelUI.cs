using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanelUI : MonoBehaviour
{
    private GameTask gameTask;
    [SerializeField]
    private string gameTaskName = null;
    [SerializeField]
    public void SetGameTask(string newGameTaskString) {if (gameTaskName == null || gameTaskName == "") {gameTaskName = newGameTaskString; OnGameTaskAssign();}}
    [SerializeField]
    private TextMeshProUGUI taskText;
    [SerializeField]
    private TextMeshProUGUI stepDescriptionText;
    [SerializeField]
    private GameObject progressBar;
    [SerializeField]
    private TextMeshProUGUI progressTrackerText;
    [SerializeField]
    private GameObject completeTaskPanel;
    [SerializeField]
    private Button addProgressButon;
    [SerializeField]
    private Button completeTaskButton;
    [SerializeField]
    private Button resetTask;

    private void OnGameTaskAssign()
    {
        gameTask = GameTasks.GetGameTasks()[gameTaskName];
        addProgressButon.onClick.AddListener(AddTaskProgress);
        completeTaskButton.onClick.AddListener(CompleteTask);
        resetTask.onClick.AddListener(ResetTask);

        UpdateStepInfoUI();

        CheckComplete();
    }

    private void AddTaskProgress()
    {
        if (gameTask.CurrentStepCount == 0)
        {
            gameTask.StartTask();
        }
        else
        {
            gameTask.AddTaskProgress(gameTask.CurrentStepCount, 1);
        }
        
        CheckComplete();
    }

    private void CompleteTask()
    {
        gameTask.CompleteTask(true);
        CheckComplete();
    }
    
    private void ResetTask()
    {
        gameTask.ForgetTaskCompleted();
        CheckComplete();
    }

    private void CheckComplete()
    {
        UpdateStepInfoUI();
        if (gameTask.TaskComplete)
        {
            completeTaskPanel.SetActive(true);
        }
        else
        {
            completeTaskPanel.SetActive(false);
        }
    }

    private void UpdateStepInfoUI()
    {
        taskText.text = gameTaskName;
        stepDescriptionText.text = gameTask.CurrentStepDescription;
        
        progressTrackerText.text = $"{gameTask.CurrentProgress}/{gameTask.MaxProgress}";
        progressBar.transform.GetChild(0).localScale = new Vector3(gameTask.CurrentProgress/gameTask.MaxProgress, 1, 1);
    }
}
