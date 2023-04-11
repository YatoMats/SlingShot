using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] GameObject configPanel;    //ゲーム設定用パネル

    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] Slider masterSlider;

    [SerializeField] GameObject messageWindowPrefab; //確認用メッセージウィンドウプレハブ
    GameObject yesOrNoWnd;
    [SerializeField] GameObject okWindowPrefab; //OKボタンメッセージウィンドウプレハブ
    GameObject canvas;

    AudioSource aud;
    [SerializeField] AudioClip decisionSE;
    [SerializeField] AudioClip cancelSE;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        canvas = GameObject.Find("Canvas");

        //音量調節用スライダー目盛変化時処理設定
        bgmSlider.onValueChanged.AddListener(value => SetBGMVol(value));
        seSlider.onValueChanged.AddListener(value => SetSEVol(value));
        masterSlider.onValueChanged.AddListener(value => SetMasterVol(value));

        //TODO:音量調節用スライダー目盛を、それの表示用テキストにも反映
        //

        //音量初期値を設定、スライダーに設定値を反映
        float masterDB;
        audioMixer.GetFloat("Master", out masterDB);
        float masterVol = DB2Norm(masterDB);
        masterSlider.value = masterVol;

        float seDB;
        audioMixer.GetFloat("SE", out seDB);
        float seVol = DB2Norm(seDB);
        seSlider.value = seVol;

        float bgmDB;
        audioMixer.GetFloat("BGM", out bgmDB);
        float bgmVol = DB2Norm(bgmDB);
        bgmSlider.value = bgmVol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //設定用メニューパネルの開閉
    public void SwitchConfigPanel()
    {
        //設定用メニューパネルを開閉
        configPanel.SetActive(!configPanel.activeSelf);

        //パネルを開く場合は、ポーズ処理
        if (configPanel.activeSelf)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    //0～1 の値を -80～0 のdB単位に変換
    float ConvertVolume2dB(float volume) 
        => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);

    /// <summary>
    /// 音量設定用関数 
    /// </summary>
    /// <param name="vol">0～1までの正規化済み音量設定用値</param>
    public void SetMasterVol(float vol)
    {
        if(vol <= 0.0f)
        {
            audioMixer.SetFloat("Master", -80.0f);
            return;
        }
        else if(vol >= 1.0f)
        {
            audioMixer.SetFloat("Master", 0.0f);
        }
        //float dB = ConvertVolume2dB(vol);
        //audioMixer.SetFloat("Master", dB);
        float dB = -10.0f * Mathf.Log(1.0f / vol, 2.0f);
        audioMixer.SetFloat("Master", dB);
    }
    public void SetBGMVol(float vol)
    {
        if (vol <= 0.0f)
        {
            audioMixer.SetFloat("BGM", -80.0f);
            return;
        }
        else if (vol >= 1.0f)
        {
            audioMixer.SetFloat("BGM", 0.0f);
        }
        float dB = -10.0f * Mathf.Log(1.0f / vol, 2.0f);
        audioMixer.SetFloat("BGM", dB);
    }
    public void SetSEVol(float vol)
    {
        if (vol <= 0.0f)
        {
            audioMixer.SetFloat("SE", -80.0f);
            return;
        }
        else if (vol >= 1.0f)
        {
            audioMixer.SetFloat("SE", 0.0f);
        }
        float dB = -10.0f * Mathf.Log(1.0f / vol, 2.0f);
        audioMixer.SetFloat("SE", dB);
    }

    /// <summary>
    /// -80～0　までのdB値を受け取って 0～1までの正規化値を返す
    /// </summary>
    /// <param name="dB"> -80～0 までのdB値</param>
    /// <returns> 0～1 までの正規化値</returns>
    float DB2Norm(float dB)
    {
        return Mathf.Pow(2.0f, dB / 10.0f);
    }

    //データ消去確認ウィンドウを出す
    public void PopResetWindow()
    {
        yesOrNoWnd = Instantiate(messageWindowPrefab) as GameObject;
        yesOrNoWnd.transform.SetParent(canvas.transform);
        yesOrNoWnd.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        //ウィンドウ設定
        MessageWindowCtrler msgWndCtrler = yesOrNoWnd.GetComponent<MessageWindowCtrler>();
        msgWndCtrler.MainText = "本当にハイスコアデータを削除しますか？";
        msgWndCtrler.TitleText = "<color=red>確認</color>";
        msgWndCtrler.yesFunc = ResetHighScore;
        msgWndCtrler.decisionSEFunc = () => aud.PlayOneShot(decisionSE);
        msgWndCtrler.cancelSEFunc = () => aud.PlayOneShot(cancelSE);

        //TODO:Yesボタン押下時ウィンドウ破棄処理
    }

    //ハイスコアデータ消去処理
    public void ResetHighScore()
    {
        //YesOrNoウィンドウ破棄処理
        Destroy(yesOrNoWnd);

        //セーブデータ消去処理
        SaveDataManager.Delete();

        //データ削除完了通知ウィンドウ生成
        GameObject msgWnd = Instantiate(okWindowPrefab) as GameObject;
        msgWnd.transform.SetParent(canvas.transform);
        msgWnd.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        ConfirmWindowCtrler msgWndCtrler = msgWnd.GetComponent<ConfirmWindowCtrler>();
        msgWndCtrler.MainText = "ハイスコアデータを削除しました";
        msgWndCtrler.TitleText = "<color=red>完了</color>";
        msgWndCtrler.decisionSEFunc = () => aud.PlayOneShot(decisionSE);
    }
}
