using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public GameObject ui;
    public Image image;
    public TextMeshProUGUI text;
    
    public string tableCode;
    [SerializeField] private LocalizedStringTable table;

    [Serializable]
    public class SpritePair
    {
        public char name;
        public Sprite sprite;
    }
    
    [SerializeField] private List<SpritePair> _pairs = new();
    private Dictionary<char, Sprite> _spriteDict;
    private List<Sprite> _tellSprites;

    [Serializable]
    public class TutorialConditions
    {
        public List<GameObject> uis;
        public List<Button> buttons;
    }

    [SerializeField] private List<TutorialConditions> _conditions = new();

    private bool _isScripting = false;
    private string _script = "";
    private int _chap;
    private Coroutine _currentCoroutine;

    [SerializeField] private List<GameObject> tutorialUIList;

    void Awake()
    {
        _chap = 1;
        text.text = "";
        _tellSprites = new();
        _spriteDict = new();
        foreach (var pair in _pairs)
        {
            _spriteDict.Add(pair.name, pair.sprite);
        }
        LoadScript();

        foreach (var button in FindObjectsOfType<Button>())
        {
            if (button.gameObject.name is "DialogueButton" or "SkipTutorial") continue;
            button.interactable = false;
        }
    }

    void LoadScript()
    {
        table.GetTableAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                StringTable stringTable = handle.Result;
                foreach (var entry in stringTable.SharedData.Entries)
                {
                    if (entry.Key[..3] == tableCode + _chap)
                    {
                        _script += stringTable.GetEntry(entry.Id).LocalizedValue + "\n";
                        _tellSprites.Add(_spriteDict[entry.Key[^1]]);
                    }
                }
                Scripting();
            }
        };
    }
    
    public void Scripting()
    {
        string name = "test";
        if (DataManager.Instance.GameData != null)
        {
            name = DataManager.Instance.GameData.Name;
        }
        _script = _script.Replace("=", name);
        if (_isScripting)
        {
            CompleteScript();
            return;
        }

        _isScripting = true;
        _currentCoroutine = StartCoroutine(StartScripting());
    }

    IEnumerator StartScripting()
    {
        if (_tellSprites.Count == 0)
        {
            CompleteScript();
            yield break;
        }
        
        image.sprite = _tellSprites[0];
        _tellSprites.RemoveAt(0);
        bool emphasize = false;
        for (int i = 0; i < _script.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            if (i == 0)
            {
                yield return new WaitForSeconds(0f);
                text.text = "";
            }
            char nextChar = _script[i];

            if (nextChar != '\n')
            {
                if (nextChar == '+' && !emphasize)
                {
                    text.text += "<size=24>";
                    emphasize = true;
                }
                else if (nextChar == '+' && emphasize)
                {
                    text.text += "</size>";
                    emphasize = false;
                }
                else
                {
                    text.text += nextChar;
                }
            }
            else
            {
                CompleteScript();
                break;
            }
        }
    }

    private void CompleteScript()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        
        int stopIndex = _script.IndexOf('\n');
        if (stopIndex != -1)
        {
            text.text = _script.Substring(0, stopIndex);
            _script = _script.Substring(stopIndex + 1);
        }
        else
        {
            text.text = _script;
            LoadAction();
        }

        _isScripting = false;
    }

    private void LoadAction()
    {
        ButtonManager.Instance.CloseUI(ui);
        // 특정 버튼만 chap 별로 활성화
        TutorialConditions condition = _conditions[_chap - 1];
        
        switch (_chap)
        {
            case 1:
                ButtonManager.Instance.OpenUI(condition.uis[0]);
                condition.buttons[0].interactable = true;
                break;
            default:
                break;
        }
        
        _chap++;
    }
}