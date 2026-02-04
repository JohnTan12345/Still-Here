using TMPro;
using UnityEngine;

public class TasklistPrefabManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI taskBameText;
    [SerializeField]
    private TextMeshProUGUI stepInfoText;
    [SerializeField]
    private TextMeshProUGUI progressText;
    [SerializeField]
    private Transform progressBar;

    public void CreateTasklistElement(GameTask gameTask)
    {
        
    }
}
