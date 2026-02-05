using UnityEngine;

public class TasklistUIManager : MonoBehaviour
{
    public static TasklistUIManager Instance {get; private set;}

    [SerializeField]
    private GameObject tasklistScrollView;
    private Transform tasklistFrame;
    private GameObject tasklistFrameCopy;
    private Transform tasklistViewport;
    [SerializeField]
    private GameObject tasklistPrefab;

    [SerializeField]
    private GameObject tasklistUI;

    // Someething here is breaking when tasklist is released
    void Awake()
    {
        Instance = this;

        tasklistViewport = tasklistScrollView.transform.Find("Viewport");
        tasklistFrame = tasklistViewport.Find("Content");

        tasklistFrameCopy = Instantiate(tasklistFrame.gameObject);

        tasklistUI.SetActive(false);
    }

    public void CreateGameTasks()
    {
        Destroy(tasklistFrame.gameObject);
        tasklistFrame = Instantiate(tasklistFrameCopy, tasklistViewport).transform;
        foreach (string gameTaskName in GameTasks.GetGameTasksOrder())
        {
            GameObject newTasklistPrefab = Instantiate(tasklistPrefab, tasklistFrame);
            
            TasklistPrefabManager tasklistPrefabManager = newTasklistPrefab.GetComponent<TasklistPrefabManager>();
            tasklistPrefabManager.CreateUI(gameTaskName);
        }
    }
    public void OnGrabRelease()
    {
        tasklistUI.SetActive(false);
        tasklistUI.transform.localPosition = Vector3.zero;
    }
}
