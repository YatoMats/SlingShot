using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowCtrler : MonoBehaviour
{
    string titleText = "メッセージウィンドウ";   //メッセージウィンドウタイトル文
    public string TitleText
    {
        get { return titleText; }
        set { titleText = value; }
    }
    string mainText = "本文";    //メッセージウィンドウ本文
    public string MainText
    {
        get { return mainText; }
        set { mainText = value; }
    }
    //Yesボタン押下時処理
    public delegate void Func();
    public Func yesFunc;
    public Func decisionSEFunc;
    public Func cancelSEFunc;

    [SerializeField] AudioClip decisionSE;
    [SerializeField] AudioClip cancelSE;

    private void Awake()
    {
        //セーブデータ消去処理を決定ボタン押下時関数に設定
        yesFunc = Discard;//SaveDataManager.Delete;
    }

    // Start is called before the first frame update
    void Start()
    {
        //子、孫要素をすべて取得
        List<GameObject> objs = GetAll(gameObject);
        foreach(GameObject obj in objs)
        {
            //タイトル、本文テキストオブジェクト、Yes,Noボタンオブジェクトを取得。
            //文章情報、関数情報をオブジェクトに代入
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
                //メッセージウィンドウオブジェクト破棄処理
                noBtn.onClick.AddListener(Discard);
                //ボタン押下時音声再生処理
                noBtn.onClick.AddListener(() => cancelSEFunc());
                //noBtn.onClick.AddListener(() => AudioSource.PlayClipAtPoint(cancelSE, transform.position));
            }
            else if (obj.name == "YesButton")
            {
                Button yesBtn = obj.GetComponent<Button>();
                //決定ボタン押下時処理
                yesBtn.onClick.AddListener(() => yesFunc());
                //ボタン押下時音声再生処理
                yesBtn.onClick.AddListener(() => decisionSEFunc());
                //yesBtn.onClick.AddListener(() => AudioSource.PlayClipAtPoint(decisionSE, transform.position));
            }
            else if (obj.name == "BackGround")
            {
                //背景タップ感知用オブジェクトのサイズをスクリーンいっぱいに変更する
                RectTransform trans = obj.GetComponent<RectTransform>();
                trans.sizeDelta = new Vector2(Screen.width, Screen.height);

                //ウィンドウ外押下時処理
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

    //メッセージウィンドウを破棄
    public void Discard()
    {
        Destroy(gameObject);
    }

    //子孫要素すべてを取得
    public List<GameObject> GetAll(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }
    public void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
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
