// Done by: John
// Description: Ends the game when bed is clicked

using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance {get; private set;}
    public Transform playerCamera;
    public Transform endGameUIInfo;
    public GameObject loadingScreenObject;
    public List<string> specialEndingNames;
    public List<AudioSource> specialEndingEndSFX;
    private Dictionary<string, AudioSource> specialEndings;

    void Awake()
    {
        Instance = this;

        specialEndings = new Dictionary<string, AudioSource>();
        for (int i = 0; i < specialEndingNames.Count; i++)
        {
            AudioSource endSFX;
            try
            {
                endSFX = specialEndingEndSFX[i];
            }
            catch (KeyNotFoundException)
            {
                endSFX = null;
            }
            specialEndings.Add(specialEndingNames[i], endSFX);
        }
    }

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

    public void EndGameSpecial(string endReason)
    {
        GameManager.Instance.EndGameSpecial(endReason, loadingScreenObject);
    }

    public void ScreenBlackout(string endingName)
    {
        Debug.Log(endingName);
        if (specialEndings[endingName] != null)
        {
            specialEndings[endingName].Play();
        }
    }
}