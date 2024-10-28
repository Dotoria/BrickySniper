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
        _cellPool = ObjectPool.Instance["cell"];
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
                        childImage.sprite = cellSO[i].prefabSprite;
                        childImage.color = Color.white;
                    }
                }
            }
        }
    }
    
    // 버튼을 누르면 paddle에 장착하기
    public void GetCell(int pos)
    {
        var newCell = _cellPool.GetFromPool();

        Cell cell = newCell.GetComponent<Cell>();
        cell.cellSO = cellSO[pos];
        cell.Install();
    }
}