using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockText;
    void FixedUpdate()
    {
        if (GameManager.Instance != null)
        {
            clockText.text = GameManager.Instance.GetTimeString();
        }
        
    }
}
