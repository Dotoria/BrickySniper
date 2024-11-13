using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject endMenuUI;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
        Time.timeScale = 0f;
        endMenuUI.SetActive(true);
    }
}
