using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    [HideInInspector] public List<CellScriptableObject> cellSO = new();
    public GameObject CellButton;
    
    private int _poolSize = 10;
    private GameObject cellPrefab;
    private CellScriptableObject BCell;

    private readonly List<Image> _gaugeImages = new();
    [HideInInspector] public List<bool> reloading;
    
    void Start()
    {
        reloading = new();
        
        cellPrefab = Resources.Load<GameObject>("Cell");
        BCell = Resources.Load<CellScriptableObject>("BCell");
        cellSO.Add(BCell);
        cellSO.AddRange(DataManager.Instance.GameData.Cellquad);
        SetCell();
        
        ObjectPool.CreatePool("cell", cellPrefab, _poolSize);
    }

    void Update()
    {
        for (int i = 0; i < reloading.Count; i++)
        {
            if (reloading[i])
            {
                if (_gaugeImages != null)
                {
                    _gaugeImages[i].fillAmount -= Time.deltaTime / cellSO[i].respawnTime;

                    if (_gaugeImages[i].fillAmount <= 0f)
                    {
                        reloading[i] = false;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        ObjectPool.Instance["cell"].ReturnToPool();
    }

    public void CellButtonClick(int pos)
    {
        Debug.Log("l?? " + reloading.Count);
        GetCell(pos);
    }
    
    // 저장된 cellSO의 list를 가져와서 버튼에 적용하기
    public void SetCell()
    {
        Button[] cellButtons = CellButton.GetComponentsInChildren<Button>();
        for (int i = 0; i < cellButtons.Length; i++)
        {
            int index = i;
            cellButtons[i].onClick.AddListener(() => CellButtonClick(index));
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
                            if (cellSO[i] == null) continue;
                            childImage.sprite = cellSO[i].prefabSprite;
                            childImage.color = Color.white;
                        }
                        else
                        {
                            _gaugeImages.Add(childImage);
                            reloading.Add(false);
                            Debug.Log("r " + reloading.Count);
                        }
                    }
                }
            }
        }
    }
    
    // 버튼을 누르면 paddle에 장착하기
    public void GetCell(int pos)
    {
        Debug.Log("reloading " + reloading.Count);
        Debug.Log("pos " + pos);
        if (reloading[pos]) return;
        var newCell = ObjectPool.Instance["cell"].GetFromPool();

        Cell cell = newCell.GetComponent<Cell>();
        cell.cellSO = cellSO[pos];
        cell.Install();
    }
    
    // 게이지 만들기
    public void ReloadCell(int pos)
    {
        _gaugeImages[pos].fillAmount = 1f;
        reloading[pos] = true;
    }
}