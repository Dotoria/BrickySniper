using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button ReloadButton;
    public Button SkillButton;
    public Button PauseButton;
    public Button ResumeButton;
    public Button TryAgainButton;
    public Button[] ExitButton;
    
    public GameObject pauseMenuUI;

    private Arrow arrow;
    
    void Start()
    {
        arrow = GameObject.FindWithTag("Player").GetComponent<Arrow>();
        
        ReloadButton.onClick.AddListener(ReloadButtonClick);
        SkillButton.onClick.AddListener(SkillButtonClick);
        PauseButton.onClick.AddListener(PauseButtonClick);
        ResumeButton.onClick.AddListener(ResumeButtonClick);
        TryAgainButton.onClick.AddListener(TryAgainButtonClick);
        foreach (var exit in ExitButton)
        {
            exit.onClick.AddListener(ExitButtonClick);
        }
    }

    void Update()
    {
        
    }

    // 탄약 장전하기
    void ReloadButtonClick()
    {
        if (arrow.remainBall < 1) return;
        arrow.GameObject().SetActive(true);
    }

    // 스킬 사용하기
    void SkillButtonClick()
    {
        
    }

    // 게임 일시정지
    void PauseButtonClick()
    {
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        PauseButton.gameObject.SetActive(false);
    }

    // 게임 재개
    void ResumeButtonClick()
    {
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        PauseButton.gameObject.SetActive(true);
    }
    
    // 게임 처음부터
    void TryAgainButtonClick()
    {
        SceneLoader.LoadSceneByName("Game");
    }

    // 게임 나가기
    void ExitButtonClick()
    {
        SceneLoader.LoadSceneByName("Lobby");
    }
}
