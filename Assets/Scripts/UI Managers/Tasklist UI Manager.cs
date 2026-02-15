// Created by: John
// Description: In-game tasklist UI manager

using UnityEngine;
using UnityEngine.UI;

public class TasklistUIManager : MonoBehaviour
{
    public static TasklistUIManager Instance;

    [SerializeField]
    private GameObject tasklistScrollView;
    private Transform tasklistFrame;
    private GameObject tasklistFrameCopy;
    private Transform tasklistViewport;
    [SerializeField]
    private GameObject tasklistPrefab;

    [SerializeField]
    private GameObject tasklistUI;

    // Set variables
    void Awake()
    {
        Instance = this;
        Debug.Log("Instance set");

        tasklistViewport = tasklistScrollView.transform.Find("Viewport");
        tasklistFrame = tasklistViewport.Find("Content");

        tasklistFrameCopy = Instantiate(tasklistFrame.gameObject);

        tasklistUI.SetActive(false);

        CreateGameTasks();
    }

    // Create the game task's UI and add it to the content group
    public void CreateGameTasks()
    {
        Destroy(tasklistFrame.gameObject);
        tasklistFrame = Instantiate(tasklistFrameCopy, tasklistViewport).transform;
        tasklistScrollView.GetComponent<ScrollRect>().content = tasklistFrame.GetComponent<RectTransform>();
        foreach (string gameTaskName in GameTasks.GetGameTasksOrder())
        {
            GameObject newTasklistPrefab = Instantiate(tasklistPrefab, tasklistFrame);
            
            TasklistPrefabManager tasklistPrefabManager = newTasklistPrefab.GetComponent<TasklistPrefabManager>();
            tasklistPrefabManager.CreateUI(gameTaskName);
        }
    }

    // Sets the tasklist UI back to the head
    public void OnGrabRelease()
    {
        transform.localPosition = Vector3.zero;
        Debug.Log("Position set");
        tasklistUI.SetActive(false);
    }
}
