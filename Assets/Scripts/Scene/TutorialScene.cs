using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Scene
{
    public class TutorialScene : MonoBehaviour
    {
        [Header("LobbyUI")] [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private TextMeshProUGUI highscore;

        [Header("GameUI")] [SerializeField] private TextMeshProUGUI score;
        private float _score = 0;
        private bool _scoring;
        [SerializeField] private Slider manaSlider;
        [SerializeField] private TextMeshProUGUI manaText;

        [Header("Tutorial")] public GameObject ui;
        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI text;

        public string tableCode;
        [SerializeField] private LocalizedStringTable table;

        [Serializable]
        public class SpritePair
        {
            public char name;
            public List<string> cellName;
            public List<Sprite> sprite;
        }

        [SerializeField] private List<SpritePair> _pairs = new();
        private Dictionary<char, List<Sprite>> _spriteDict;
        private Dictionary<char, string> _nameDict;
        private List<Sprite> _tellSprites;
        private List<string> _tellNames;

        [Serializable]
        public class TutorialConditions
        {
            public List<GameObject> scriptUis;
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
            _tellNames = new();
            _spriteDict = new();
            _nameDict = new();
            foreach (var pair in _pairs)
            {
                _spriteDict.Add(pair.name, pair.sprite);
                _nameDict.Add(pair.name,
                    pair.cellName[
                        LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale)]);
            }

            LoadScript();

            nickname.text = DataManager.Instance.GameData.Name;
            level.text = DataManager.Instance.GameData.Level.ToString("D");
            highscore.text = DataManager.Instance.GameData.HighScore.ToString("N0");

            foreach (var button in FindObjectsOfType<Button>())
            {
                if (button.gameObject.name is "DialogueButton" or "SkipTutorial") continue;
                button.interactable = false;
            }
        }

        private void Update()
        {
            if (_scoring)
            {
                _score += Time.deltaTime;
                score.text = _score.ToString("N0");
            }
        }

        public void SetScoring()
        {
            StartCoroutine(Wait2Seconds());
        }

        public void SetMana(int amount)
        {
            float percent = (40 - amount * 1f) / 40;
            manaSlider.value = percent;
            manaText.text = $"{40 - amount} / 40";
        }

        IEnumerator Wait2Seconds()
        {
            _scoring = true;
            yield return new WaitForSeconds(2f);
            _scoring = false;
            highscore.text = _score.ToString("N0");
            DataManager.Instance.GameData.HighScore = (int)_score;
            DataManager.Instance.SaveData();
        }

        public void LoadScript()
        {
            if (_chap > 1)
            {
                TutorialConditions condition = _conditions[_chap - 2];
                foreach (var UI in condition.uis)
                {
                    UIManager.Instance.CloseUI(UI);
                }

                foreach (var scriptUI in condition.scriptUis)
                {
                    UIManager.Instance.CloseUI(scriptUI);
                }

                foreach (var button in condition.buttons)
                {
                    button.interactable = false;
                }

                condition = _conditions[_chap - 1];
                foreach (var scriptUI in condition.scriptUis)
                {
                    UIManager.Instance.OpenUI(scriptUI);
                }
            }

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
                            _tellSprites.Add(_spriteDict[entry.Key[^2]][int.Parse(entry.Key[^1].ToString())]);
                            _tellNames.Add(_nameDict[entry.Key[^2]]);
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

            _script = _script.Replace("==", name);
            _script = _script.Replace("/+", "</size>");
            _script = _script.Replace("+", "<size=124>");
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
            nameText.text = _tellNames[0];
            _tellSprites.RemoveAt(0);
            _tellNames.RemoveAt(0);

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
                    text.text += nextChar;
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
            UIManager.Instance.CloseUI(ui);
            // 특정 버튼만 chap 별로 활성화
            TutorialConditions condition = _conditions[_chap - 1];

            foreach (var UI in condition.uis)
            {
                UIManager.Instance.OpenUI(UI);
            }

            foreach (var button in condition.buttons)
            {
                button.interactable = true;
            }

            _chap++;
        }
    }
}