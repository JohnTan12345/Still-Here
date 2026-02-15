// Created by: John
// Description: End Game Task UI reference point

using TMPro;
using UnityEngine;

public class EndGameTaskUIVariables : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TaskNameText;
    [SerializeField]
    private TextMeshProUGUI CompletionCountText;

    public void UpdateUI(TaskInfo taskInfo) // Update the game task UI
    {
        TaskNameText.text = taskInfo.TaskName;
        if (taskInfo.CompletionCount > 0) // If task is complete, show how many completion
        {
            CompletionCountText.text = taskInfo.CompletionCount == 1 ? "Completed 1 time!" : $"Completed {taskInfo.CompletionCount} times!";
        }
        else // Else show incomplete
        {
            CompletionCountText.text = "Incomplete";
        }
    }
}
