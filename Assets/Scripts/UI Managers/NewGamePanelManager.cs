/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGamePanelManager : MonoBehaviour
{
    private List<DifficultySetting> defaultDifficultySettings = new List<DifficultySetting>();
    public DifficultySetting currentDifficultySetting = new DifficultySetting();
    private int difficultyMode = 0;

    // UI Mapping
    [SerializeField]
    private TextMeshProUGUI difficultyText;
    [SerializeField]
    private Slider taskAmountSlider;
    [SerializeField]
    private TextMeshProUGUI taskAmountHeader;
    [SerializeField]
    private TMP_InputField forgetFrequencyInput;
    [SerializeField]
    private Toggle changeTasklistOrderToggle;

    void Start()
    {
        // Future update: Put it inside database

        DifficultySetting easy = new DifficultySetting();
        easy.name = "Easy";
        easy.taskAmount = 4;
        easy.forgetFrequency = 120f;
        easy.changeTasklistOrder = false;

        defaultDifficultySettings.Add(easy);

        DifficultySetting normal = new DifficultySetting();
        normal.name = "Normal";
        normal.taskAmount = 6;
        normal.forgetFrequency = 90f;
        normal.changeTasklistOrder = true;

        defaultDifficultySettings.Add(normal);

        DifficultySetting hard = new DifficultySetting();
        hard.name = "Hard";
        hard.taskAmount = 7;
        hard.forgetFrequency = 60f;
        hard.changeTasklistOrder = true;

        defaultDifficultySettings.Add(hard);

        // Create new difficulty settng
        currentDifficultySetting = new DifficultySetting();

        SetNewDifficultyPreset(defaultDifficultySettings[0]);
        StartCoroutine(LoadValues());
    }

    public IEnumerator LoadValues()
    {
        yield return new WaitUntil(() => GameInfo.DataLoaded == true);
        yield return new WaitUntil(() => taskAmountSlider != null);
        Debug.Log(taskAmountSlider.maxValue);
        Debug.Log(GameInfo.GetGameTaskList().Count);
        taskAmountSlider.maxValue = GameInfo.GetGameTaskList().Count;
        ChangeDifficulty(0);
    }

    public void OnValueChanged()
    {
        if (difficultyMode == defaultDifficultySettings.Count)
        {
            currentDifficultySetting.taskAmount = (int)taskAmountSlider.value;
            currentDifficultySetting.forgetFrequency = float.Parse(forgetFrequencyInput.text);
            currentDifficultySetting.changeTasklistOrder = changeTasklistOrderToggle.isOn;
        }

        UpdateUI();
    }

    public void ChangeDifficulty(int change)
    {
        if (difficultyMode + change < 0)
        {
            difficultyMode = defaultDifficultySettings.Count;
        }
        else if (difficultyMode + change > defaultDifficultySettings.Count)
        {
            difficultyMode = 0;
        }
        else
        {
            difficultyMode += change;
        }

        if (difficultyMode == defaultDifficultySettings.Count)
        {
            currentDifficultySetting.name = "Custom";
            SetCustomDifficultyUIInteractable(true);
        }
        else
        {
            Debug.Log(JsonUtility.ToJson(defaultDifficultySettings[difficultyMode], true));
            SetNewDifficultyPreset(defaultDifficultySettings[difficultyMode]);
            SetCustomDifficultyUIInteractable(false);
        }

        UpdateUI();
    }

    private void SetNewDifficultyPreset(DifficultySetting newDifficultySetting)
    {
        Debug.Log("Setting new difficulty");
        currentDifficultySetting.name = newDifficultySetting.name;
        currentDifficultySetting.taskAmount = newDifficultySetting.taskAmount;
        currentDifficultySetting.forgetFrequency = newDifficultySetting.forgetFrequency;
        currentDifficultySetting.changeTasklistOrder = newDifficultySetting.changeTasklistOrder;
    }
    private void UpdateUI()
    {
        Debug.Log("Updating UI");
        difficultyText.text = currentDifficultySetting.name;
        taskAmountHeader.text = $"Task Amount: {currentDifficultySetting.taskAmount}";
        taskAmountSlider.value = currentDifficultySetting.taskAmount;
        forgetFrequencyInput.text = currentDifficultySetting.forgetFrequency.ToString();
        changeTasklistOrderToggle.isOn = currentDifficultySetting.changeTasklistOrder;
    }

    private void SetCustomDifficultyUIInteractable(bool value)
    {
        taskAmountSlider.interactable = value;
        forgetFrequencyInput.interactable = value;
        forgetFrequencyInput.interactable = value;
        changeTasklistOrderToggle.interactable = value;
    }
}

[System.Serializable]
public class DifficultySetting
{
    public string name;
    public int taskAmount;
    public float forgetFrequency;
    public bool changeTasklistOrder;
}
*/