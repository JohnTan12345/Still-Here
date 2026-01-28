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
        
        UpdateUI();
    }

    private void ForgetTasks()
    {
        GameTasks.ForgetCompletedGameTasks(4, true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        int i = 0;
        while (taskPanelParent.transform.childCount > 0)
        {
            DestroyImmediate(taskPanelParent.transform.GetChild(0).gameObject);
            i++;
            if (i == 20)
            {
                Debug.LogError("Infinite loop detected");
                break;
            }
        }

        foreach (string gameTaskName in GameTasks.GetGameTasksOrder())
        {
            GameObject newTaskPanel = Instantiate(taskPanel, taskPanelParent.transform);
            newTaskPanel.GetComponent<TaskPanelUI>().SetGameTask(gameTaskName);
            
        }
    }
}
