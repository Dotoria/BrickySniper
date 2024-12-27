using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    //private CurrentCanvas _canvas;
    public List<GameObject> canvasObject;

    [Header("Cellquad")]
    public List<CellScriptableObject> allCell;
    public GameObject contentPrefab;
    public GameObject parentObject;
    [SerializeField] private List<CellScriptableObject> cellquad;
    [SerializeField] private Image[] imagesCellquad;
    [SerializeField] private Sprite defaultImage;
    
    [Header("Home")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Image[] imagesHome;

    [Header("Custom")]
    public List<SkinScriptableObject> allSkin;
    public GameObject skinPrefab;
    public GameObject skinParent;
    [SerializeField] private GameObject player;
    public GameObject target;
    
    public enum CurrentCanvas
    {
        CellquadCanvas,
        HomeCanvas,
        CustomCanvas,
    }
    
    private void Awake()
    {
        for (int i = 0; i < allCell.Count; i++)
        {
            GameObject content = Instantiate(contentPrefab, parentObject.transform);
            content.GetComponent<ContentCell>().cellSO = allCell[i];
            content.GetComponent<ContentCell>().image.sprite = allCell[i].prefabSprite;
            content.GetComponent<ContentCell>().textName.text = allCell[i].name;
            content.GetComponent<ContentCell>().button.onClick.AddListener(()=>SetSkin(i));
        }

        for (int i = 0; i < allSkin.Count; i++)
        {
            GameObject skin = Instantiate(skinPrefab, skinParent.transform);
            skin.GetComponent<ContentSkin>().skinSO = allSkin[i];
            skin.GetComponent<ContentSkin>().image.sprite = allSkin[i].prefabSprite;
            skin.GetComponent<ContentSkin>().skinName.text = allSkin[i].name;
        }
        
        cellquad = DataManager.Instance.GameData.Cellquad;
        // _canvas = CurrentCanvas.HomeCanvas;
        SetCanvas(1);
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
             imagesHome[i].sprite = defaultImage;
             imagesCellquad[i].sprite = defaultImage;
             
             if (cellSO != default)
             {
                 imagesHome[i].sprite = cellSO.prefabSprite;
                 imagesCellquad[i].sprite = cellSO.prefabSprite;
             }
             i++;
         }
    }

    public void SetCanvas(int index)
    {
        foreach (var canvasObj in canvasObject)
        {
            UIManager.Instance.CloseUI(canvasObj);
        }

        CurrentCanvas canvas = (CurrentCanvas)index;
        switch (canvas)
        {
            case CurrentCanvas.CellquadCanvas:
                player.SetActive(false);
                UIManager.Instance.OpenUI(canvasObject[0]);
                break;
            case CurrentCanvas.HomeCanvas:
                player.SetActive(true);
                player.transform.position = Vector3.zero;
                UIManager.Instance.OpenUI(canvasObject[1]);
                break;
            case CurrentCanvas.CustomCanvas:
                player.SetActive(true);
                player.transform.position = Vector3.zero + new Vector3(0, 7, 0);
                UIManager.Instance.OpenUI(canvasObject[2]);
                break;
            default:
                break;
        }
    }

    public void SetSkin(int index)
    {
        RuntimeAnimatorController customAnim = player.transform.GetChild(3).GetComponent<SkinScriptableObject>().prefabAnimation;
        target.GetComponent<SpriteRenderer>().sprite = allSkin[index].prefabSprite;
        target.GetComponent<AnimatorController>().animationClips[0] = customAnim.animationClips[0];
    }
}