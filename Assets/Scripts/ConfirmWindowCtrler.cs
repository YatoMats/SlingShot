using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//OKボタンのみのメッセージウィンドウ作成

public class ConfirmWindowCtrler : MonoBehaviour
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
    public Func decisionSEFunc;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //子、孫要素をすべて取得
        List<GameObject> objs = GetAll(gameObject);
        foreach (GameObject obj in objs)
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
            else if (obj.name == "OKButton")
            {
                Button okBtn = obj.GetComponent<Button>();
                //ボタン押下時音声再生処理
                okBtn.onClick.AddListener(() => decisionSEFunc());
                okBtn.onClick.AddListener(Discard);
            }
            else if (obj.name == "BackGround")
            {
                //背景タップ感知用オブジェクトのサイズをスクリーンいっぱいに変更する
                RectTransform trans = obj.GetComponent<RectTransform>();
                trans.sizeDelta = new Vector2(Screen.width, Screen.height);

                //ウィンドウ外押下時処理
                Button bgButton = obj.GetComponent<Button>();
                bgButton.onClick.AddListener(() => decisionSEFunc());
                bgButton.onClick.AddListener(Discard);
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
