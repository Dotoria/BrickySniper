using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameData
{
    public BasicData BasicData;
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
            "basicdata" => BasicData,
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

public class BasicData
{
    public List<CellScriptableObject> AllCell;
    public List<EnemyScriptableObject> AllEnemy;
    public List<SkinScriptableObject> AllSkin;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private static readonly byte[] key = Encoding.UTF8.GetBytes("Oa!SbrnfVs4a_g3U");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("QAG2d02WK2!aXZX-");

    public GameData GameData;
    private string keyName = "PlayerData";

    public List<EnemyScriptableObject> infEnemiesData;
    public Dictionary<string, List<EnemyScriptableObject>> EnemiesData = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CreateData();
        EnemiesData["INF"] = infEnemiesData; // 무한모드 적 데이터
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
        if (GameData.Name == "Lemur")
        {
            foreach (var basic in GameData.BasicData.AllCell)
            {
                basic.Get = true;
            }
            foreach (var basic in GameData.BasicData.AllEnemy)
            {
                basic.Get = true;
            }
            foreach (var basic in GameData.BasicData.AllSkin)
            {
                basic.Get = true;
            }
        }
        
        //data.Version = PlayerSettings.bundleVersion;
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
            // if (PlayerSettings.bundleVersion != GameData.Version)
            // {
            //     // 버전 미일치
            // }
            
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
            BasicData = new BasicData
            {
                AllCell = Resources.LoadAll<CellScriptableObject>("ScriptableObject/Cell").ToList(),
                AllEnemy = Resources.LoadAll<EnemyScriptableObject>("ScriptableObject/Enemy").ToList(),
                AllSkin = Resources.LoadAll<SkinScriptableObject>("ScriptableObject/Skin").ToList(),
            },
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
        object item = DataManager.Instance.GameData.GetData(itemName);
        
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
                cellArray.Add((CellScriptableObject) amount);
            }
        }
        else if (item is BasicData data && amount is ScriptableObject obj)
        {
            INewGettable foundItem = obj switch
            {
                CellScriptableObject cell => data.AllCell.Find(c => c == cell),
                EnemyScriptableObject enemy => data.AllEnemy.Find(e => e == enemy),
                SkinScriptableObject skin => data.AllSkin.Find(s => s == skin),
                _ => null
            };

            if (foundItem != null)
            {
                foundItem.NewGet = true;
            }
        }
        
        Instance.SaveData();
    }

    public void EndTutorial(CellScriptableObject cell)
    {
        GainItem("Cellquad", cell, null);
        GainItem("basicdata", cell, null);
        Instance.SaveData();
    }
}