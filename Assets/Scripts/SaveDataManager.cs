using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaveData
{
    /*string userName;
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }*/
    int highScore;
    public int HighScore
    {
        get { return highScore; }
        set { highScore = value; }
    }
}

public class SaveDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Save(SaveData data)
    {
        //セーブデータ保存
        //PlayerPrefs.SetString("UserName", data.UserName);
        PlayerPrefs.SetInt("HighScore", data.HighScore);
    }

    public static SaveData Load()
    {
        //セーブデータ読み込み
        SaveData saveData = new SaveData();
        //saveData.UserName = PlayerPrefs.GetString("UserName");
        saveData.HighScore = PlayerPrefs.GetInt("HighScore");
        return saveData;
    }

    public static void Delete()
    {
        //セーブデータ削除
        SaveData data = new SaveData();
        data.HighScore = 0;
        Save(data);
    }
}
