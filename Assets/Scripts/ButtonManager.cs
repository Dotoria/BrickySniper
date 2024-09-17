using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button ReloadButton;
    public Button SkillButton;
    
    void Start()
    {
        ReloadButton.onClick.AddListener(ReloadButtonClick);
        SkillButton.onClick.AddListener(SkillButtonClick);
    }

    void Update()
    {
        
    }

    void ReloadButtonClick()
    {
        
    }

    void SkillButtonClick()
    {
        
    }
}
