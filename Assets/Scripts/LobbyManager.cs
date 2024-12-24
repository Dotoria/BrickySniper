using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public List<CellScriptableObject> allCell;

    [Header("Cellquad")]
    public GameObject contentPrefab;
    public GameObject parentObject;
    [SerializeField] private CellScriptableObject[] cellquad;
    [SerializeField] private Image[] imagesCellquad;
    
    [Header("Home")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image[] imagesHome;
    private void Awake()
    {
        for (int i = 0; i < allCell.Count; i++)
        {
            GameObject content = Instantiate(contentPrefab, parentObject.transform);
            content.GetComponent<ContentCell>().cellSO = allCell[i];
            content.GetComponent<ContentCell>().image.sprite = allCell[i].prefabSprite;
            content.GetComponent<ContentCell>().name.text = allCell[i].name;
        }
        cellquad = DataManager.Instance.GameData.Cellquad;
        SetData();
    }

    void SetData()
    {
         nameText.text = DataManager.Instance.GameData.Name;
         highScoreText.text = DataManager.Instance.GameData.HighScore.ToString("N0");
         levelText.text = DataManager.Instance.GameData.Level.ToString();
         expSlider.value = DataManager.Instance.GameData.Exp * 1.0f / (DataManager.Instance.GameData.Level * 30);
         int i = 0;
         foreach (var cellSO in cellquad)
         {
             imagesHome[i].sprite = cellSO.prefabSprite;
             imagesCellquad[i].sprite = cellSO.prefabSprite;
             i++;
         }
    }
}