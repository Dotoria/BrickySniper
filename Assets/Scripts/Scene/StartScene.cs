using Common;
using Data;
using TMPro;
using UnityEngine;

namespace Scene
{
    public class StartScene : MonoBehaviour
    {
        [SerializeField] private GameObject _checkIDUI;
        
        public void CheckData(GameObject ui)
        {
            if (DataManager.Instance.GameData.Name == "")
            {
                UIManager.Instance.OpenUI(ui);
            }
            else
            {
                DataManager.Instance.LoadData();
                SceneLoader.LoadSceneByName("Lobby");
            }
        }

        public void StartTutorial(TextMeshProUGUI tmp)
        {
            tmp.text = tmp.text.Trim();
            if (tmp.text.Length > 8 || tmp.text.Length < 2)
            {
                UIManager.Instance.PopUp(_checkIDUI);
                return;
            }
            DataManager.Instance.GameData.Name = tmp.text;
            DataManager.Instance.SaveData();
            SceneLoader.LoadSceneByName("Tutorial");
        }
    }
}