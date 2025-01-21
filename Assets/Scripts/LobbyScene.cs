using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private CellScriptableObject _currentCell;
    
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
    public AnimatorOverrideController targetController;
    public Animator playerAnimator;
    
    public enum CurrentCanvas
    {
        CellquadCanvas,
        HomeCanvas,
        CustomCanvas,
    }
    
    private void Awake()
    {
        Time.timeScale = 1f;
        
        for (int i = 0; i < allCell.Count; i++)
        {
            GameObject content = Instantiate(contentPrefab, parentObject.transform);
            content.GetComponent<ContentCell>().cellSO = allCell[i];
            content.GetComponent<ContentCell>().image.sprite = allCell[i].prefabSprite;
            content.GetComponent<ContentCell>().textName.text = allCell[i].name;
            int capturedIndex = i;
            content.GetComponent<ContentCell>().button.onClick.AddListener(()=> { _currentCell = allCell[capturedIndex]; });
        }

        for (int i = 0; i < allSkin.Count; i++)
        {
            GameObject skin = Instantiate(skinPrefab, skinParent.transform);
            skin.GetComponent<ContentSkin>().skinSO = allSkin[i];
            skin.GetComponent<ContentSkin>().image.sprite = allSkin[i].prefabSprite;
            skin.GetComponent<ContentSkin>().skinName.text = allSkin[i].name;
            int capturedIndex = i;
            skin.GetComponent<ContentSkin>().button.onClick.AddListener(()=>SetSkin(capturedIndex));
        }
        
        cellquad = DataManager.Instance.GameData.Cellquad;
        // _canvas = CurrentCanvas.HomeCanvas;

        playerAnimator.runtimeAnimatorController = targetController;
        
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
             if (cellSO != default || cellSO != null)
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

    public void SetSquad(int index)
    {
        while (cellquad.Count < 3)
        {
            cellquad.Add(null);
        }

        if (_currentCell == null)
        {
            SquadUpdate(index, null);
            cellquad[index] = null;
            DataManager.Instance.GameData.Cellquad = cellquad;
            DataManager.Instance.SaveData();
            return;
        }
        
        int findIndex = cellquad.FindIndex(cell => cell == _currentCell);
        if (findIndex != -1)
        {
            CellScriptableObject so = cellquad[index];
            cellquad[index] = _currentCell;
            cellquad[findIndex] = so;
            SquadUpdate(findIndex, so);
        }
        else
        {
            cellquad[index] = _currentCell;
        }
        
        SquadUpdate(index, _currentCell);

        DataManager.Instance.GameData.Cellquad = cellquad;
        DataManager.Instance.SaveData();
        _currentCell = null;
    }

    private void SquadUpdate(int index, CellScriptableObject cell)
    {
        Sprite sprite = cell == null ? defaultImage : cell.prefabSprite;
        imagesHome[index].sprite = sprite;
        imagesCellquad[index].sprite = sprite;
    }
    
    public void SetSkin(int index)
    {
        player.SetActive(false);
        if (allSkin[index].prefabName != "없음")
        {
            player.transform.Find("PlayerCustom").gameObject.GetComponent<SpriteRenderer>().sprite =
                allSkin[index].prefabSprite;
        }
        else
        {
            player.transform.Find("PlayerCustom").gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
        targetController["DefaultCustom"] = allSkin[index].prefabClip;
        player.SetActive(true);
    }
}