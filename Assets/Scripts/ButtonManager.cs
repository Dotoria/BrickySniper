using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }
    public float currentSpeed;
    
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
