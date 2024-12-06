using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject cellButton;
    
    public GameObject paddle;
    
    void Start()
    {
        foreach (var button in cellButton.GetComponentsInChildren<Button>())
        {
            if (!paddle.GetComponent<Paddle>().canDrag) return;
            
            var pos = button.name[^1] - '1';
            button.onClick.AddListener(() => CellManager.Instance.GetCell(pos));
        }
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
    }

    public void SetLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
