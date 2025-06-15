using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class SceneLoader : MonoBehaviour
    {
        public static void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}