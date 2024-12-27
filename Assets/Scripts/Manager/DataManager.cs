using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.Localization.Platform.Android;
using UnityEngine;

public class GameData
{
    public string Version;
    public string Name;
    public int HighScore;
    public int Coin;
    public int Gem;
    public int Level;
    public int Exp;
    public List<CellScriptableObject> Cellquad;

    public object GetData(string item)
    {
        return item.ToLower() switch
        {
            "highscore" => HighScore,
            "coin" => Coin,
            "gem" => Gem,
            "level" => Level,
            "exp" => Exp,
            "cellquad" => Cellquad,
            _ => null
        };
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private static readonly byte[] key = Encoding.UTF8.GetBytes("Oa!SbrnfVs4a_g3U");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("QAG2d02WK2!aXZX-");

    public GameData GameData;
    private string keyName = "PlayerData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CreateData();
        DontDestroyOnLoad(gameObject);
        
        if (PlayerPrefs.HasKey(keyName))
        {
            LoadData();
        }
        else
        {
            SaveData();
        }
    }

    public void SaveData()
    {
        GameData data = GameData;
        if (GameData == null) return;
        
        data.Version = PlayerSettings.bundleVersion;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            byte[] encryptedData = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            
            PlayerPrefs.SetString(keyName, Convert.ToBase64String(encryptedData));
            PlayerPrefs.Save();
        }
    }

    public void LoadData()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(keyName)))
        {
            if (PlayerSettings.bundleVersion != GameData.Version)
            {
                // 버전 미일치
            }
            
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] bytes = Convert.FromBase64String(PlayerPrefs.GetString(keyName));
                byte[] decryptedData = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);

                GameData = JsonUtility.FromJson<GameData>(Encoding.UTF8.GetString(decryptedData));
            }
        }
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey(keyName);
        CreateData();
    }

    private void CreateData()
    {
        GameData = new GameData
        {
            Name = "",
            Version = "0.0",
            Coin = 0,
            Gem = 0,
            HighScore = 0,
            Level = 1,
            Exp = 0,
            Cellquad = new(),
        };
    }

    public void GainItem(string itemName, object amount, TextMeshProUGUI text)
    {
        object item = GameData.GetData(itemName);
        
        if (item is int intItem &&  amount is int intAmount)
        {
            intItem += intAmount;
            text.text = intItem.ToString("N0");
        }
        else if (item is List<CellScriptableObject> cellArray)
        {
            int index = cellArray.FindIndex(cell => cell != null);
            if (index >= 0)
            {
                // cellArray[index] = null;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (cellArray[i] == null)
                    {
                        cellArray[i] = (CellScriptableObject)amount;
                        break;
                    }
                }
            }
        }
        
        SaveData();
    }

    public void EndTutorial(CellScriptableObject cell)
    {
        GainItem("Cellquad", cell, null);
        SaveData();
    }
}