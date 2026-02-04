// Done by: John
// Description: Ends the game when bed is clicked

using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public Transform playerCamera;
    void Update()
    {
        transform.LookAt(playerCamera);
    }

    public void EndGame()
    {
        GameManager.Instance.EndGame();
    }
}
