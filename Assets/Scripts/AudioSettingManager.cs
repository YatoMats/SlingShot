using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    // Start is called before the first frame update
    void Start()
    {
        //音量調節用スライダー目盛変化時処理設定
        masterSlider.onValueChanged.AddListener(value => SetAudioVol(value, "Master"));
        bgmSlider.onValueChanged.AddListener(value => SetAudioVol(value, "BGM"));
        seSlider.onValueChanged.AddListener(value => SetAudioVol(value, "SE"));

        //音量初期値を設定、スライダーに設定値を反映
        float masterDB;
        audioMixer.GetFloat("Master", out masterDB);
        float masterVol = DB2Norm(masterDB);
        masterSlider.value = masterVol;

        float bgmDB;
        audioMixer.GetFloat("BGM", out bgmDB);
        float bgmVol = DB2Norm(bgmDB);
        bgmSlider.value = bgmVol;

        float seDB;
        audioMixer.GetFloat("SE", out seDB);
        float seVol = DB2Norm(seDB);
        seSlider.value = seVol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 音量設定用関数
    /// </summary>
    /// <param name="vol">0～1までの正規化済み音量設定用値</param>
    /// <param name="audType">AudioMixer側で設定した識別名</param>
    void SetAudioVol(float vol, string audType)
    {
        if (vol <= 0.0f)
        {
            if (!audioMixer.SetFloat(audType, -80.0f)) Debug.Log("AudioMixerへの音量設定に失敗しました");
            return;
        }
        else if (vol >= 1.0f)
        {
            if (!audioMixer.SetFloat(audType, 0.0f)) Debug.Log("AudioMixerへの音量設定に失敗しました");
            return;
        }
        float dB = -10.0f * Mathf.Log(1.0f / vol, 2.0f);
        if (!audioMixer.SetFloat(audType, dB)) Debug.Log("AudioMixerへの音量設定に失敗しました");
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

    /// <summary>
    /// 音量設定用関数 
    /// </summary>
    /// <param name="vol">0～1までの正規化済み音量設定用値</param>
    public void SetMasterVol(float vol)
    {
        if (vol <= 0.0f)
        {
            audioMixer.SetFloat("Master", -80.0f);
            return;
        }
        else if (vol >= 1.0f)
        {
            audioMixer.SetFloat("Master", 0.0f);
        }
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
}
