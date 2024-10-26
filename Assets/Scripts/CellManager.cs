using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public List<CellScriptableObject> cellSO;
    public List<GameObject> cellList;
    public GameObject cellPrefab;
    
    public GameObject CellButton;

    void Awake()
    {
    }
    
    public void SetCell(List<CellScriptableObject> cellSO)
    {
        Button[] cellButtons = CellButton.GetComponentsInChildren<Button>();
        for (int i = 0; i < cellButtons.Length; i++)
        {
            if (!cellSO[i])
            {
                // 엑스 스프라이트, 버튼 클릭 막기
            }
            else
            {
                // 셀 스프라이트 적용, cellSO 적용, cellList 적용
            }
        }
        this.cellSO = cellSO;
    }

    public void GetCell(int pos)
    {
        Cell cell = cellPrefab.GetComponent<Cell>();
        cell.cellSO = cellSO[pos];
    }
}