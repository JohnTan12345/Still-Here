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
    
    // Unity inspector bugging out when trying to use a dictionary
    public List<string> specialEndingNames;
    public List<AudioSource> specialEndingEndSFX;
    private Dictionary<string, AudioSource> specialEndings;
    

    // Create a new dictionary that links the special ending name to it's sound effect
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

    // End the game through the game manager
    public void EndGame()
    {
        GameManager.Instance.EndGame(loadingScreenObject);
    }

    // Animation end event listener
    public void AnimationEnd()
    {
        GameManager.Instance.LoadEndScene();
    }

    // End the game through the game manager for special endings
    public void EndGameSpecial(string endReason)
    {
        GameManager.Instance.EndGameSpecial(endReason, loadingScreenObject);
    }
    
    // Play the animation for the special ending
    public void ScreenBlackout(string endingName)
    {
        Debug.Log(endingName);
        if (specialEndings[endingName] != null)
        {
            specialEndings[endingName].Play();
        }
    }
}