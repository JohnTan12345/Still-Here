// Created by: John
// Description: Clock UI Manager

using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockText;

    // Updates the clock UI with the current game time
    void FixedUpdate()
    {
        if (GameManager.Instance != null)
        {
            clockText.text = GameManager.Instance.GetTimeString();
        }
        
    }
}
