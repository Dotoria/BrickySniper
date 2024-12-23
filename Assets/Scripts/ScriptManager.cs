using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;

public class ScriptManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string tableCode;
    [SerializeField] private LocalizedStringTable table;

    private bool _isScripting = false;
    private string _script = "";
    private Coroutine _currentCoroutine;

    void Awake()
    {
        table.GetTableAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                StringTable stringTable = handle.Result;
                int chap = 1;
                int num = 1;
                Debug.Log(tableCode + chap + '-' + num);
                while (stringTable.TryGetValue(stringTable.SharedData.GetId(tableCode + chap + '-' + num), out var entry))
                {
                    _script += entry.LocalizedValue + '\n';
                    Debug.Log($"{tableCode + chap + '-' + num} : {entry.LocalizedValue}");
                    num++;
                }
            }
        };
    }
    
    public void Scripting()
    {
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
        text.text = "";

        for (int i = 0; i < _script.Length; i++)
        {
            char nextChar = _script[i];

            if (nextChar != '\n')
            {
                text.text += nextChar;
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.05f);

            if (!_isScripting)
                break;
        }

        CompleteScript();
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
            // SceneLoader.LoadSceneByName("MainScene");
        }

        _isScripting = false;
    }
}