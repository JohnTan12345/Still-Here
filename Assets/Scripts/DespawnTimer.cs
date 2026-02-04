// Done by: John
// Description: Removes object after a set amount of time

using System.Collections;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    public int despawnTime = 5;
    void Start()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        int timer = 0;
        while (timer < despawnTime)
        {
            yield return new WaitForSecondsRealtime(1);
            timer++;
        }

        Destroy(gameObject);
    }
}
