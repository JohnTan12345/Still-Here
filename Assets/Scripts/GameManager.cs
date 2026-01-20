using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Task createGameTasksTask = GameTasks.FetchGameTasklistFromDatabase();
    }
}
