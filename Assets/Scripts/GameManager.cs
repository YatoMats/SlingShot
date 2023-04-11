using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] int maxHP = 3; //初期HP
    int crntHP; //現在のHP

    [SerializeField] GameObject heartPref;  //ハートオブジェクトプレハブ
    GameObject[] hearts;    //ハートオブジェクト配列  

    Vector3 firstHeartPos;  //1番目のハートオブジェクトの位置
    float offsetPos = 150.0f;    //2番目以降のハートオブジェクトの位置のずらし幅

    [SerializeField] Sprite crossSprite;    //×マーク用スプライト情報

    AudioSource aud;
    [SerializeField] AudioClip pointSE;
    [SerializeField] AudioClip fanfareSE;

    //BGM再生用コンポーネント
    AudioSource bgmAud;
    [SerializeField] AudioClip victoryBGM;
    [SerializeField] AudioClip resultBGM;
    [SerializeField] AudioClip sadBGM;

    SceneFadeOutController sceneFader;

    [SerializeField] float pitchIncre = 0.1f;   //コンボ時の効果音の高さの上がり具合

    int points = 0; //ゲームスコアポイント
    public int Poinsts
    {
        get { return points; }
    }
    int defeated = 0;   //総撃破数
    public int Defeated
    {
        get { return defeated; }
    }
    int combo = 0;  //1つの弾での連続撃破数 //次のプレイヤー弾生成時に0に戻す
    public int Combo
    {
        get { return combo; }
    }

    Text scoreText;

    [SerializeField] GameObject pausePanel;

    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    GameObject canvas;

    [SerializeField] AudioMixer audioMixer;

    bool isNewScore = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneFader = GameObject.Find("SceneFadeOutController").GetComponent<SceneFadeOutController>();

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        aud = GetComponent<AudioSource>();
        //aud.volume = 1.0f;

        bgmAud = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();

        //初期HP設定
        crntHP = maxHP;

        //一番目のハートオブジェクトの位置座標を設定
        firstHeartPos = new Vector3(100.0f, 100.0f);        

        //最大HPの分だけ、ハートオブジェクトを生成
        hearts = new GameObject[maxHP];

        canvas = GameObject.Find("Canvas");

        for(int i = 0; i < maxHP; i++)
        {
            hearts[i] = Instantiate(heartPref) as GameObject;
            //Canvasの子要素に
            hearts[i].transform.SetParent(canvas.transform);
            //ハートオブジェクトそれぞれの位置決定
            RectTransform rectTrans = hearts[i].GetComponent<RectTransform>();
            rectTrans.position = new Vector3(firstHeartPos.x + offsetPos * i, firstHeartPos.y);
            
            //ハートオブジェクトを描画順序を奥側に
            hearts[i].transform.SetAsFirstSibling();
        }

        /*Dictionary<string, int> scores = new Dictionary<string, int>();

        //要素を追加する時はキーと値を一緒に渡す。
        scores.Add("Default User", 0);
        //dict["Default User"] = 0; //<- 添字の様に使うこともできる。

        //要素数とキーの確認
        var c = scores.Count;
        var b = scores.ContainsKey("Default User");

        //要素の値にアクセスする時はキーを添字がわりに使う。
        var n = scores["Default User"]; 
        scores["Default User"] = 0; // <- 値を上書きしている。

        //要素の削除にはキーを使う。
        //scores.Remove("Default User");

        //辞書型の要素のループにはforeachを使う
        foreach (KeyValuePair<string, int> pair in scores)
        {
            var key = pair.Key; // <- キー
            var value = pair.Value; // <- ペアとなる値
        }
        //キーだけをループしたい時はKeysを使う。
        //foreach (var key in scores.Keys) { }
        //値だけをループしたい時はValuesを使う。
        //foreach (var value in scores.Values) { }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ダメージ計算処理
    public void Damage(int val = 1)
    {
        //HPを減らす
        crntHP -= val;

        if (crntHP < 0) return;

        for(int i = crntHP; i < maxHP; i++)
        {
            //ハートオブジェクトのイメージ画像を×に書き換え
            hearts[i].GetComponent<Image>().sprite = crossSprite;

            //大きさ、位置補正
            RectTransform rectTrans = hearts[i].GetComponent<RectTransform>();
            rectTrans.position = new Vector3(rectTrans.position.x, firstHeartPos.y + 5.0f, rectTrans.position.z);
            rectTrans.localScale = new Vector3(0.75f, 0.75f);
        }

        //HPが0以下になったら、ゲームオーバー
        if (crntHP <= 0)
        {
            //bgmを止める
            bgmAud.Pause();

            //すべての敵オブジェクトを破壊処理
            GameObject[] objects = GameObject.FindGameObjectsWithTag("bomb");
            foreach (GameObject bomb in objects)
            {
                Destroy(bomb);
            }
            //ゴムパチンコ弾オブジェクトを破棄
            GameObject ball = GameObject.FindGameObjectWithTag("ball");
            Destroy(ball);

            //ResultPanelをactiveに
            ToResult();
            //スコアを表示
            ShowScore();

            //リザルト画面時BGMを再生
            if (points <= 0)
                bgmAud.clip = sadBGM;
            else if (points > 1000)
                bgmAud.clip = victoryBGM;
            else
                bgmAud.time = 0.0f;
                //bgmAud.clip = victoryBGM;//resultBGM;

            if(isNewScore)
            {
                audioMixer.SetFloat("PitchShiftSEPitch", 1.0f);
                aud.volume = 0.1f;
                aud.PlayOneShot(fanfareSE);
                bgmAud.clip = victoryBGM;
            }

            bgmAud.Play();
        }
    }

    //ポイント加算計算処理
    public void AddPoints(int val)
    {
        //点数計算
        defeated++;
        combo++;
        points += val * combo;

        //スコア表示を更新
        scoreText.text = "撃破数 : " + defeated + "\nSCORE : " + points + " pt";
        if (combo > 1)
        {
            scoreText.text += "\nCOMBO : x" + combo.ToString();
        }

        //コンボ数に応じて、効果音の高さを上げる
        float pitch = 1.0f + pitchIncre * (combo - 1);
        audioMixer.SetFloat("PitchShiftSEPitch", pitch);

        //ポイント加算時効果音再生
        aud.PlayOneShot(pointSE);
    }
    //コンボを1に戻す
    public void ResetCombo()
    {
        combo = 0;
    }

    //ポーズ画面表示の切替
    public void SwitchPause()
    {
        if(!pausePanel)
        {
            Debug.Log("PausePanelが未割り当てです。");
            return;
        }
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (pausePanel.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    //リザルトパネルのボタン用関数
    public void LoadNextScene(string sceneName = "MainScene")
    {
        //時間停止を戻す
        Time.timeScale = 1.0f;
        //シーンフェード
        sceneFader.SetTrigger(sceneName);
    }

    //リザルト画面への切替
    void ToResult()
    {
        if (resultPanel==null)
        {
            Debug.Log("ResultPanelが未割り当てです。");
            return;
        }

        resultPanel.SetActive(!resultPanel.activeSelf);

        if (resultPanel.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
            Time.timeScale = 1.0f;

    }
    //スコア表示処理
    void ShowScore()
    {
        if (resultText==null)
        {
            Debug.Log("ResultTextが未割り当てです。");
            return;
        }

        string txt = "";
        //今回のスコア
        txt += "Score : " + points + " pt";
        //ハイスコア読み込み
        SaveData data = SaveDataManager.Load();
        //前回ハイスコア
        txt += "\nHighScore : " + data.HighScore + " pt";
        //今回のスコアがハイスコアを上回っていたら、
        if(points > data.HighScore)
        {
            //データ書き換え
            data.HighScore = points;
            //ハイスコア保存   
            SaveDataManager.Save(data);
            //New Record !
            txt += "\nNew Record !";
            //ハイスコア更新を知らせるフラグ
            isNewScore = true;
        }
        //スコア文を反映
        resultText.text = txt;
    }
}
