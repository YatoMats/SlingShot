using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] int maxHP = 3; //����HP
    int crntHP; //���݂�HP

    [SerializeField] GameObject heartPref;  //�n�[�g�I�u�W�F�N�g�v���n�u
    GameObject[] hearts;    //�n�[�g�I�u�W�F�N�g�z��  

    Vector3 firstHeartPos;  //1�Ԗڂ̃n�[�g�I�u�W�F�N�g�̈ʒu
    float offsetPos = 150.0f;    //2�Ԗڈȍ~�̃n�[�g�I�u�W�F�N�g�̈ʒu�̂��炵��

    [SerializeField] Sprite crossSprite;    //�~�}�[�N�p�X�v���C�g���

    AudioSource aud;
    [SerializeField] AudioClip pointSE;
    [SerializeField] AudioClip fanfareSE;

    //BGM�Đ��p�R���|�[�l���g
    AudioSource bgmAud;
    [SerializeField] AudioClip victoryBGM;
    [SerializeField] AudioClip resultBGM;
    [SerializeField] AudioClip sadBGM;

    SceneFadeOutController sceneFader;

    [SerializeField] float pitchIncre = 0.1f;   //�R���{���̌��ʉ��̍����̏オ��

    int points = 0; //�Q�[���X�R�A�|�C���g
    public int Poinsts
    {
        get { return points; }
    }
    int defeated = 0;   //�����j��
    public int Defeated
    {
        get { return defeated; }
    }
    int combo = 0;  //1�̒e�ł̘A�����j�� //���̃v���C���[�e��������0�ɖ߂�
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

        //����HP�ݒ�
        crntHP = maxHP;

        //��Ԗڂ̃n�[�g�I�u�W�F�N�g�̈ʒu���W��ݒ�
        firstHeartPos = new Vector3(100.0f, 100.0f);        

        //�ő�HP�̕������A�n�[�g�I�u�W�F�N�g�𐶐�
        hearts = new GameObject[maxHP];

        canvas = GameObject.Find("Canvas");

        for(int i = 0; i < maxHP; i++)
        {
            hearts[i] = Instantiate(heartPref) as GameObject;
            //Canvas�̎q�v�f��
            hearts[i].transform.SetParent(canvas.transform);
            //�n�[�g�I�u�W�F�N�g���ꂼ��̈ʒu����
            RectTransform rectTrans = hearts[i].GetComponent<RectTransform>();
            rectTrans.position = new Vector3(firstHeartPos.x + offsetPos * i, firstHeartPos.y);
            
            //�n�[�g�I�u�W�F�N�g��`�揇����������
            hearts[i].transform.SetAsFirstSibling();
        }

        /*Dictionary<string, int> scores = new Dictionary<string, int>();

        //�v�f��ǉ����鎞�̓L�[�ƒl���ꏏ�ɓn���B
        scores.Add("Default User", 0);
        //dict["Default User"] = 0; //<- �Y���̗l�Ɏg�����Ƃ��ł���B

        //�v�f���ƃL�[�̊m�F
        var c = scores.Count;
        var b = scores.ContainsKey("Default User");

        //�v�f�̒l�ɃA�N�Z�X���鎞�̓L�[��Y�������Ɏg���B
        var n = scores["Default User"]; 
        scores["Default User"] = 0; // <- �l���㏑�����Ă���B

        //�v�f�̍폜�ɂ̓L�[���g���B
        //scores.Remove("Default User");

        //�����^�̗v�f�̃��[�v�ɂ�foreach���g��
        foreach (KeyValuePair<string, int> pair in scores)
        {
            var key = pair.Key; // <- �L�[
            var value = pair.Value; // <- �y�A�ƂȂ�l
        }
        //�L�[���������[�v����������Keys���g���B
        //foreach (var key in scores.Keys) { }
        //�l���������[�v����������Values���g���B
        //foreach (var value in scores.Values) { }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�_���[�W�v�Z����
    public void Damage(int val = 1)
    {
        //HP�����炷
        crntHP -= val;

        if (crntHP < 0) return;

        for(int i = crntHP; i < maxHP; i++)
        {
            //�n�[�g�I�u�W�F�N�g�̃C���[�W�摜���~�ɏ�������
            hearts[i].GetComponent<Image>().sprite = crossSprite;

            //�傫���A�ʒu�␳
            RectTransform rectTrans = hearts[i].GetComponent<RectTransform>();
            rectTrans.position = new Vector3(rectTrans.position.x, firstHeartPos.y + 5.0f, rectTrans.position.z);
            rectTrans.localScale = new Vector3(0.75f, 0.75f);
        }

        //HP��0�ȉ��ɂȂ�����A�Q�[���I�[�o�[
        if (crntHP <= 0)
        {
            //bgm���~�߂�
            bgmAud.Pause();

            //���ׂĂ̓G�I�u�W�F�N�g��j�󏈗�
            GameObject[] objects = GameObject.FindGameObjectsWithTag("bomb");
            foreach (GameObject bomb in objects)
            {
                Destroy(bomb);
            }
            //�S���p�`���R�e�I�u�W�F�N�g��j��
            GameObject ball = GameObject.FindGameObjectWithTag("ball");
            Destroy(ball);

            //ResultPanel��active��
            ToResult();
            //�X�R�A��\��
            ShowScore();

            //���U���g��ʎ�BGM���Đ�
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

    //�|�C���g���Z�v�Z����
    public void AddPoints(int val)
    {
        //�_���v�Z
        defeated++;
        combo++;
        points += val * combo;

        //�X�R�A�\�����X�V
        scoreText.text = "���j�� : " + defeated + "\nSCORE : " + points + " pt";
        if (combo > 1)
        {
            scoreText.text += "\nCOMBO : x" + combo.ToString();
        }

        //�R���{���ɉ����āA���ʉ��̍������グ��
        float pitch = 1.0f + pitchIncre * (combo - 1);
        audioMixer.SetFloat("PitchShiftSEPitch", pitch);

        //�|�C���g���Z�����ʉ��Đ�
        aud.PlayOneShot(pointSE);
    }
    //�R���{��1�ɖ߂�
    public void ResetCombo()
    {
        combo = 0;
    }

    //�|�[�Y��ʕ\���̐ؑ�
    public void SwitchPause()
    {
        if(!pausePanel)
        {
            Debug.Log("PausePanel�������蓖�Ăł��B");
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

    //���U���g�p�l���̃{�^���p�֐�
    public void LoadNextScene(string sceneName = "MainScene")
    {
        //���Ԓ�~��߂�
        Time.timeScale = 1.0f;
        //�V�[���t�F�[�h
        sceneFader.SetTrigger(sceneName);
    }

    //���U���g��ʂւ̐ؑ�
    void ToResult()
    {
        if (resultPanel==null)
        {
            Debug.Log("ResultPanel�������蓖�Ăł��B");
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
    //�X�R�A�\������
    void ShowScore()
    {
        if (resultText==null)
        {
            Debug.Log("ResultText�������蓖�Ăł��B");
            return;
        }

        string txt = "";
        //����̃X�R�A
        txt += "Score : " + points + " pt";
        //�n�C�X�R�A�ǂݍ���
        SaveData data = SaveDataManager.Load();
        //�O��n�C�X�R�A
        txt += "\nHighScore : " + data.HighScore + " pt";
        //����̃X�R�A���n�C�X�R�A�������Ă�����A
        if(points > data.HighScore)
        {
            //�f�[�^��������
            data.HighScore = points;
            //�n�C�X�R�A�ۑ�   
            SaveDataManager.Save(data);
            //New Record !
            txt += "\nNew Record !";
            //�n�C�X�R�A�X�V��m�点��t���O
            isNewScore = true;
        }
        //�X�R�A���𔽉f
        resultText.text = txt;
    }
}
