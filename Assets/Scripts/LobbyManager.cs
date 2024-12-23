using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Awake()
    {
        SetText();
    }

    void SetText()
    {
         nameText.text = DataManager.Instance.GameData.Name;
         highScoreText.text = DataManager.Instance.GameData.HighScore.ToString("N0");
    }
}