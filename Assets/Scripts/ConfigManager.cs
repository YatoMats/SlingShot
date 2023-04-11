using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] GameObject configPanel;    //�Q�[���ݒ�p�p�l��

    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] Slider masterSlider;

    [SerializeField] GameObject messageWindowPrefab; //�m�F�p���b�Z�[�W�E�B���h�E�v���n�u
    GameObject yesOrNoWnd;
    [SerializeField] GameObject okWindowPrefab; //OK�{�^�����b�Z�[�W�E�B���h�E�v���n�u
    GameObject canvas;

    AudioSource aud;
    [SerializeField] AudioClip decisionSE;
    [SerializeField] AudioClip cancelSE;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        canvas = GameObject.Find("Canvas");

        //���ʒ��ߗp�X���C�_�[�ڐ��ω��������ݒ�
        bgmSlider.onValueChanged.AddListener(value => SetBGMVol(value));
        seSlider.onValueChanged.AddListener(value => SetSEVol(value));
        masterSlider.onValueChanged.AddListener(value => SetMasterVol(value));

        //TODO:���ʒ��ߗp�X���C�_�[�ڐ����A����̕\���p�e�L�X�g�ɂ����f
        //

        //���ʏ����l��ݒ�A�X���C�_�[�ɐݒ�l�𔽉f
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

    //�ݒ�p���j���[�p�l���̊J��
    public void SwitchConfigPanel()
    {
        //�ݒ�p���j���[�p�l�����J��
        configPanel.SetActive(!configPanel.activeSelf);

        //�p�l�����J���ꍇ�́A�|�[�Y����
        if (configPanel.activeSelf)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    //0�`1 �̒l�� -80�`0 ��dB�P�ʂɕϊ�
    float ConvertVolume2dB(float volume) 
        => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);

    /// <summary>
    /// ���ʐݒ�p�֐� 
    /// </summary>
    /// <param name="vol">0�`1�܂ł̐��K���ς݉��ʐݒ�p�l</param>
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
    /// -80�`0�@�܂ł�dB�l���󂯎���� 0�`1�܂ł̐��K���l��Ԃ�
    /// </summary>
    /// <param name="dB"> -80�`0 �܂ł�dB�l</param>
    /// <returns> 0�`1 �܂ł̐��K���l</returns>
    float DB2Norm(float dB)
    {
        return Mathf.Pow(2.0f, dB / 10.0f);
    }

    //�f�[�^�����m�F�E�B���h�E���o��
    public void PopResetWindow()
    {
        yesOrNoWnd = Instantiate(messageWindowPrefab) as GameObject;
        yesOrNoWnd.transform.SetParent(canvas.transform);
        yesOrNoWnd.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        //�E�B���h�E�ݒ�
        MessageWindowCtrler msgWndCtrler = yesOrNoWnd.GetComponent<MessageWindowCtrler>();
        msgWndCtrler.MainText = "�{���Ƀn�C�X�R�A�f�[�^���폜���܂����H";
        msgWndCtrler.TitleText = "<color=red>�m�F</color>";
        msgWndCtrler.yesFunc = ResetHighScore;
        msgWndCtrler.decisionSEFunc = () => aud.PlayOneShot(decisionSE);
        msgWndCtrler.cancelSEFunc = () => aud.PlayOneShot(cancelSE);

        //TODO:Yes�{�^���������E�B���h�E�j������
    }

    //�n�C�X�R�A�f�[�^��������
    public void ResetHighScore()
    {
        //YesOrNo�E�B���h�E�j������
        Destroy(yesOrNoWnd);

        //�Z�[�u�f�[�^��������
        SaveDataManager.Delete();

        //�f�[�^�폜�����ʒm�E�B���h�E����
        GameObject msgWnd = Instantiate(okWindowPrefab) as GameObject;
        msgWnd.transform.SetParent(canvas.transform);
        msgWnd.GetComponent<RectTransform>().position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

        ConfirmWindowCtrler msgWndCtrler = msgWnd.GetComponent<ConfirmWindowCtrler>();
        msgWndCtrler.MainText = "�n�C�X�R�A�f�[�^���폜���܂���";
        msgWndCtrler.TitleText = "<color=red>����</color>";
        msgWndCtrler.decisionSEFunc = () => aud.PlayOneShot(decisionSE);
    }
}
