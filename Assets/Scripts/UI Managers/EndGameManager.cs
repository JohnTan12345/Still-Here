// Done by: John
// Description: Ends the game when bed is clicked

using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public Transform playerCamera;
    public Transform endGameUIInfo;
    public GameObject loadingScreenObject;
    void Update()
    {
        endGameUIInfo.LookAt(playerCamera);
    }

    public void EndGame()
    {
        GameManager.Instance.EndGame(loadingScreenObject);
    }

    public void AnimationEnd()
    {
        GameManager.Instance.LoadEndScene();
    }
}
