using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button ReloadButton;
    public Button SkillButton;

    private Arrow arrow;
    
    void Start()
    {
        arrow = GameObject.FindWithTag("Player").GetComponent<Arrow>();
        ReloadButton.onClick.AddListener(ReloadButtonClick);
        SkillButton.onClick.AddListener(SkillButtonClick);
    }

    void Update()
    {
        
    }

    void ReloadButtonClick()
    {
        if (arrow.remainBall < 1) return;
        arrow.GameObject().SetActive(true);
    }

    void SkillButtonClick()
    {
        
    }
}
