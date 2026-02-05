using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform pourPoint;
    public TextMeshProUGUI text;
    void Update()
    {
        float angle = Vector3.Angle(pourPoint.up, Vector3.down);
        text.text = angle.ToString();
    }
}
