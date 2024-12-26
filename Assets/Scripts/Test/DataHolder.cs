using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataHolder : MonoBehaviour
{
    public TextMeshProUGUI text;

    public Button saveButton;
    public Button deleteButton;
    public TextMeshProUGUI nickname;

    private void Awake()
    {
        // DataManager.Instance.DeleteData();
        
        saveButton.onClick.AddListener(SaveButton);
        deleteButton.onClick.AddListener(DeleteButton);
    }

    void SaveButton()
    {
        DataManager.Instance.GameData.Name = nickname.text;
        DataManager.Instance.SaveData();
        SetText();
    }

    void DeleteButton()
    {
        DataManager.Instance.DeleteData();
        SetText();
    }

    void SetText()
    {
        text.text = $"name: {DataManager.Instance.GameData.Name}\n" +
                    $"Version: {DataManager.Instance.GameData.Version}\n" +
                    $"coin: {DataManager.Instance.GameData.Coin}\n" +
                    $"gem: {DataManager.Instance.GameData.Gem}\n" +
                    $"highscore: {DataManager.Instance.GameData.HighScore}\n";
    }
}