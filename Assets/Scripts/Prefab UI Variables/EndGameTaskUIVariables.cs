using TMPro;
using UnityEngine;

public class EndGameTaskUIVariables : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TaskNameText;
    [SerializeField]
    private TextMeshProUGUI CompletionCountText;

    public void UpdateUI(TaskInfo taskInfo)
    {
        TaskNameText.text = taskInfo.TaskName;
        if (taskInfo.CompletionCount > 0)
        {
            CompletionCountText.text = taskInfo.CompletionCount == 1 ? "Completed 1 time!" : $"Completed {taskInfo.CompletionCount} times!";
        }
        else
        {
            CompletionCountText.text = "Incomplete";
        }
    }
}
