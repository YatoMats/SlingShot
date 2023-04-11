using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowCtrler : MonoBehaviour
{
    string titleText = "���b�Z�[�W�E�B���h�E";   //���b�Z�[�W�E�B���h�E�^�C�g����
    public string TitleText
    {
        get { return titleText; }
        set { titleText = value; }
    }
    string mainText = "�{��";    //���b�Z�[�W�E�B���h�E�{��
    public string MainText
    {
        get { return mainText; }
        set { mainText = value; }
    }
    //Yes�{�^������������
    public delegate void Func();
    public Func yesFunc;
    public Func decisionSEFunc;
    public Func cancelSEFunc;

    [SerializeField] AudioClip decisionSE;
    [SerializeField] AudioClip cancelSE;

    private void Awake()
    {
        //�Z�[�u�f�[�^��������������{�^���������֐��ɐݒ�
        yesFunc = Discard;//SaveDataManager.Delete;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�q�A���v�f�����ׂĎ擾
        List<GameObject> objs = GetAll(gameObject);
        foreach(GameObject obj in objs)
        {
            //�^�C�g���A�{���e�L�X�g�I�u�W�F�N�g�AYes,No�{�^���I�u�W�F�N�g���擾�B
            //���͏��A�֐������I�u�W�F�N�g�ɑ��
            if (obj.name == "MainText")
            {
                obj.GetComponent<Text>().text = mainText;
            }
            else if (obj.name == "TitleText")
            {
                obj.GetComponent<Text>().text = titleText;
            }
            else if (obj.name == "NoButton")
            {
                Button noBtn = obj.GetComponent<Button>();
                //���b�Z�[�W�E�B���h�E�I�u�W�F�N�g�j������
                noBtn.onClick.AddListener(Discard);
                //�{�^�������������Đ�����
                noBtn.onClick.AddListener(() => cancelSEFunc());
                //noBtn.onClick.AddListener(() => AudioSource.PlayClipAtPoint(cancelSE, transform.position));
            }
            else if (obj.name == "YesButton")
            {
                Button yesBtn = obj.GetComponent<Button>();
                //����{�^������������
                yesBtn.onClick.AddListener(() => yesFunc());
                //�{�^�������������Đ�����
                yesBtn.onClick.AddListener(() => decisionSEFunc());
                //yesBtn.onClick.AddListener(() => AudioSource.PlayClipAtPoint(decisionSE, transform.position));
            }
            else if (obj.name == "BackGround")
            {
                //�w�i�^�b�v���m�p�I�u�W�F�N�g�̃T�C�Y���X�N���[�������ς��ɕύX����
                RectTransform trans = obj.GetComponent<RectTransform>();
                trans.sizeDelta = new Vector2(Screen.width, Screen.height);

                //�E�B���h�E�O����������
                Button bgButton = obj.GetComponent<Button>();
                bgButton.onClick.AddListener(() => cancelSEFunc());
                bgButton.onClick.AddListener(Discard);
                //bgButton.onClick.AddListener(() => AudioSource.PlayClipAtPoint(decisionSE, trans.position));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���b�Z�[�W�E�B���h�E��j��
    public void Discard()
    {
        Destroy(gameObject);
    }

    //�q���v�f���ׂĂ��擾
    public List<GameObject> GetAll(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }
    public void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //�q�v�f�����Ȃ���ΏI��
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }
}
