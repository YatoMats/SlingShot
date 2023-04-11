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
        //���ʒ��ߗp�X���C�_�[�ڐ��ω��������ݒ�
        masterSlider.onValueChanged.AddListener(value => SetAudioVol(value, "Master"));
        bgmSlider.onValueChanged.AddListener(value => SetAudioVol(value, "BGM"));
        seSlider.onValueChanged.AddListener(value => SetAudioVol(value, "SE"));

        //���ʏ����l��ݒ�A�X���C�_�[�ɐݒ�l�𔽉f
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
    /// ���ʐݒ�p�֐�
    /// </summary>
    /// <param name="vol">0�`1�܂ł̐��K���ς݉��ʐݒ�p�l</param>
    /// <param name="audType">AudioMixer���Őݒ肵�����ʖ�</param>
    void SetAudioVol(float vol, string audType)
    {
        if (vol <= 0.0f)
        {
            if (!audioMixer.SetFloat(audType, -80.0f)) Debug.Log("AudioMixer�ւ̉��ʐݒ�Ɏ��s���܂���");
            return;
        }
        else if (vol >= 1.0f)
        {
            if (!audioMixer.SetFloat(audType, 0.0f)) Debug.Log("AudioMixer�ւ̉��ʐݒ�Ɏ��s���܂���");
            return;
        }
        float dB = -10.0f * Mathf.Log(1.0f / vol, 2.0f);
        if (!audioMixer.SetFloat(audType, dB)) Debug.Log("AudioMixer�ւ̉��ʐݒ�Ɏ��s���܂���");
    }

    /// <summary>
    /// -80�`0�@�܂ł�dB�l���󂯎���� 0�`1�܂ł̐��K���l��Ԃ�
    /// </summary>
    /// <param name="dB"> -80�`0 �܂ł�dB�l</param>
    /// <returns> 0�`1 �܂ł̐��K���l</returns>
    float DB2Norm(float dB)
    {
        return Mathf.Pow(2.0f, dB / 10.0f);
    }

    /// <summary>
    /// ���ʐݒ�p�֐� 
    /// </summary>
    /// <param name="vol">0�`1�܂ł̐��K���ς݉��ʐݒ�p�l</param>
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
