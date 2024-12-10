using System;
using System.Security.Cryptography;
using System.Text;
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
        GameData = new GameData();
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
    }
}