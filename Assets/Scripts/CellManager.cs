using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance { get; private set; }
    
    public List<CellScriptableObject> cellSO;
    public GameObject CellButton;
    
    private int _poolSize = 10;
    private ObjectPool _cellPool;
    public GameObject cellPrefab;

    private readonly List<Image> _gaugeImages = new();
    private List<bool> _reloading = new();
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        ObjectPool.CreatePool("cell", cellPrefab, _poolSize);
    }

    void Update()
    {
        for (int i = 0; i < _reloading.Count; i++)
        {
            if (_reloading[i])
            {
                if (_gaugeImages != null)
                {
                    _gaugeImages[i].fillAmount -= Time.deltaTime / cellSO[i].respawnTime;

                    if (_gaugeImages[i].fillAmount <= 0f)
                    {
                        _reloading[i] = false;
                    }
                }
            }
        }
    }
    
    // 저장된 cellSO의 list를 가져와서 버튼에 적용하기
    public void SetCell(List<CellScriptableObject> so)
    {
        cellSO = so;
        
        Button[] cellButtons = CellButton.GetComponentsInChildren<Button>();
        for (int i = 0; i < cellButtons.Length; i++)
        {
            if (i > cellSO.Count - 1)
            {
                // 엑스 스프라이트, 버튼 클릭 막기
            }
            else
            {
                // 스프라이트 적용
                Image[] childImages = cellButtons[i].GetComponentsInChildren<Image>(true);
                foreach (var childImage in childImages)
                {
                    if (childImage != cellButtons[i].GetComponent<Image>())
                    {
                        if (childImage.fillAmount != 0f)
                        {
                            childImage.sprite = cellSO[i].prefabSprite;
                            childImage.color = Color.white;
                        }
                        else
                        {
                            _gaugeImages.Add(childImage);
                            _reloading.Add(false);
                        }
                    }
                }
            }
        }
    }
    
    // 버튼을 누르면 paddle에 장착하기
    public void GetCell(int pos)
    {
        if (_reloading[pos]) return;
        
        var newCell = ObjectPool.Instance["cell"].GetFromPool();

        Cell cell = newCell.GetComponent<Cell>();
        cell.cellSO = cellSO[pos];
        cell.Install();
        Debug.Log("GetCell");
        
        ReloadCell(pos);
    }
    
    // 게이지 만들기
    public void ReloadCell(int pos)
    {
        _gaugeImages[pos].fillAmount = 1f;
        _reloading[pos] = true;
    }
    
    // 풀로 돌려놓기
    public void DestroyCell(GameObject obj)
    {
        Destroy(obj.GetComponent<PolygonCollider2D>());
        ObjectPool.Instance["cell"].ReturnToPool(obj);
    }
}