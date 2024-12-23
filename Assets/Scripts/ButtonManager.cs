using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }
    [HideInInspector] public float currentSpeed;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentSpeed = 1f;
    }

    public void CellButton(int pos)
    {
        CellManager.Instance.GetCell(pos);
    }

    public void CheckData(GameObject ui)
    {
        if (DataManager.Instance.GameData.Name == "")
        {
            OpenUI(ui);
        }
        else
        {
            DataManager.Instance.LoadData();
            SceneLoader.LoadSceneByName("Lobby");
        }
    }

    public void StartTutorial(TextMeshProUGUI tmp)
    {
        tmp.text = tmp.text.Trim(' ');
        if (tmp.text.Length > 8 && tmp.text.Length < 1) return;
        DataManager.Instance.GameData.Name = tmp.text;
        DataManager.Instance.SaveData();
        SceneLoader.LoadSceneByName("Tutorial");
    }

    public void PopUp(GameObject ui)
    {
        OpenUI(ui);
        StartCoroutine(Wait(2f));
        CloseUI(ui);
    }

    IEnumerator Wait(float second)
    {
        yield return new WaitForSeconds(second);
    }

    public void OpenUI(GameObject ui)
    {
        ui.SetActive(true);
    }

    public void CloseUI(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
        currentSpeed = speed;
    }

    public void SetLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
