using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TasklistPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject taskPanel;
    [SerializeField]
    private GameObject taskPanelParent;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private TMP_InputField createAmount;
    [SerializeField]
    private Button resetSomeTasksButton;

    void Awake()
    {
        createButton.onClick.AddListener(CreateTasks);
        resetSomeTasksButton.onClick.AddListener(ForgetTasks);
    }

    private void CreateTasks()
    {
        GameTasks.CreateGameTasks(int.Parse(createAmount.text));

        foreach (string gameTaskName in GameTasks.GetGameTasks().Keys)
        {
            Debug.Log(gameTaskName);
            GameObject newTaskPanel = Instantiate(taskPanel, taskPanelParent.transform);
            newTaskPanel.GetComponent<TaskPanelUI>().SetGameTask(gameTaskName);
            Debug.Log(newTaskPanel.GetComponent<TaskPanelUI>());
        }
    }

    private void ForgetTasks()
    {
        GameTasks.ForgetCompletedGameTasks(4);
    }
}
