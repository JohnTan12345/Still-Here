using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(loadstuff());
    }

    private IEnumerator loadstuff()
    {
        Task loadTask = GameInfo.FetchGameInfoDatabase();
        yield return new WaitUntil(() => GameInfo.DataLoaded);
        GameTasks.CreateGameTasks(7);
    }   
}
