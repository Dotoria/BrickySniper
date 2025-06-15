using System.Collections.Generic;
using Common;
using Data;
using Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
    public class LobbyScene : MonoBehaviour
    {
        public List<GameObject> canvasObject;

        [Header("Cellquad")]
        [SerializeField] private List<CellScriptableObject> allCell;
        public GameObject contentPrefab;
        public GameObject parentObject;
        [SerializeField] private List<CellScriptableObject> cellquad;
        [SerializeField] private Image[] imagesCellquad;
        [SerializeField] private Sprite defaultImage;
        private CellScriptableObject _currentCell;

        [Header("Home")]
        [SerializeField] private List<EnemyScriptableObject> allEnemy;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Slider expSlider;
        [SerializeField] private Image[] imagesHome;
        public GameObject bookPrefab;
        public List<Transform> parentBook;

        [Header("Custom")]
        [SerializeField] private List<SkinScriptableObject> allSkin;
        public GameObject skinPrefab;
        public GameObject skinParent;
        [SerializeField] private GameObject player;
        public AnimatorOverrideController targetController;
        public Animator playerAnimator;

        [Header("Lottery")]
        public GameObject MyeloidLottery;
        public GameObject LymphoidLottery;

        private enum CurrentCanvas
        {
            CellquadCanvas,
            HomeCanvas,
            CustomCanvas,
            LotteryCanvas,
        }

        private void Awake()
        {
            Time.timeScale = 1f;
            allCell = DataManager.Instance.BasicData.AllCell;
            allEnemy = DataManager.Instance.BasicData.AllEnemy;
            allSkin = DataManager.Instance.BasicData.AllSkin;

            for (int i = 0; i < allCell.Count; i++)
            {
                // Cellquad Canvas
                Instantiate(contentPrefab, parentObject.transform).TryGetComponent(out ContentCell cell);
                cell.cellSO = allCell[i];
                cell.image.sprite = allCell[i].prefabSprite;
                cell.textName.text = allCell[i].prefabName;
                int capturedIndex = i;
                if (cell.cellSO.NewGet && !cell.cellSO.Get)
                {
                    // 첫 획득 효과 추가
                    DataManager.Instance.GameData.GettableList.Add(cell.cellSO);
                    DataManager.Instance.SaveData();
                }

                if (!cell.cellSO.Get) continue;
                cell.button.onClick.AddListener(() => { _currentCell = allCell[capturedIndex]; });

                // Home Canvas
                GameObject book = Instantiate(bookPrefab, parentBook[0]);
                book.TryGetComponent(out ContentBook content);
                content.bookSO = allCell[i];
            }

            for (int i = 0; i < allEnemy.Count; i++)
            {
                // Home Canvas
                GameObject book = Instantiate(bookPrefab, parentBook[1]);
                book.TryGetComponent(out ContentBook content);
                content.bookSO = allEnemy[i];
            }

            for (int i = 0; i < allSkin.Count; i++)
            {
                // Custom Canvas
                GameObject skin = Instantiate(skinPrefab, skinParent.transform);
                skin.GetComponent<ContentSkin>().skinSO = allSkin[i];
                skin.GetComponent<ContentSkin>().image.sprite = allSkin[i].prefabSprite;
                skin.GetComponent<ContentSkin>().skinName.text = allSkin[i].name;
                int capturedIndex = i;
                skin.GetComponent<ContentSkin>().button.onClick.AddListener(() => SetSkin(capturedIndex));
            }

            cellquad = DataManager.Instance.GameData.Cellquad;

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
            for (int i = 0; i < canvasObject.Count; i++)
            {
                var canvasObj = canvasObject[i];
                if (i != index && canvasObj.activeSelf) UIManager.Instance.CloseUI(canvasObj);
                else if (i == index && !canvasObj.activeSelf) UIManager.Instance.OpenUI(canvasObj);
            }

            CurrentCanvas canvas = (CurrentCanvas)index;
            switch (canvas)
            {
                case CurrentCanvas.CellquadCanvas:
                    player.SetActive(false);
                    break;
                case CurrentCanvas.HomeCanvas:
                    player.SetActive(true);
                    player.transform.position = Vector3.zero;
                    break;
                case CurrentCanvas.CustomCanvas:
                    player.SetActive(true);
                    player.transform.position = Vector3.zero + new Vector3(0, 7, 0);
                    break;
                case CurrentCanvas.LotteryCanvas:
                    player.SetActive(false);
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
            SpriteRenderer playerCustom =
                player.transform.Find("PlayerCustom").gameObject.GetComponent<SpriteRenderer>();
            if (allSkin[index].prefabName != "없음")
            {
                playerCustom.sprite = allSkin[index].prefabSprite;
            }
            else
            {
                playerCustom.sprite = null;
            }

            targetController["DefaultCustom"] = allSkin[index].prefabClip;
            player.SetActive(true);
        }
        
        public void OpenLottery(int index)
        {
            if (index == 0)
            {
                // Myeloid Lottery
                int num = Random.Range(0, DataManager.Instance.BasicData.MyeloidCount);
                CellScriptableObject cellSO = DataManager.Instance.BasicData.AllCell[num];
                DataManager.Instance.GainItem("GettableList", cellSO, null);
            }
            else if (index == 1)
            {
                // Lymphoid Lottery
                int num = DataManager.Instance.BasicData.MyeloidCount;
                num += Random.Range(0, DataManager.Instance.BasicData.LymphoidCount);
                CellScriptableObject cellSO = DataManager.Instance.BasicData.AllCell[num];
                DataManager.Instance.GainItem("GettableList", cellSO, null);
            }
        }
    }
}