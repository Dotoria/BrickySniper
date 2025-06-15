using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Common
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PopUp(GameObject ui)
        {
            StartCoroutine(Wait(ui, 2f));
        }

        IEnumerator Wait(GameObject ui, float second)
        {
            OpenUI(ui);
            yield return new WaitForSeconds(second);
            CloseUI(ui);
        }

        public void OpenUI(GameObject ui)
        {
            ui.SetActive(true);
        }

        public void CloseUI(GameObject ui)
        {
            ui.SetActive(false);
        }

        public void OpenOrCloseUI(GameObject ui)
        {
            if (ui.activeSelf) CloseUI(ui);
            else OpenUI(ui);
        }

        public void SetLanguage(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}