using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeOutController : MonoBehaviour
{
    [SerializeField] float m_fadeSpan = 0.45f;    //フェードにかける時間
    float m_fadeTimer = 0.0f;
    float m_fadeAlpha = 0.0f;   //最初は完全に透明

    string m_nextSceneName = "MainScene";   //次に読み込むシーン名
    bool m_trigger = false; //シーン読み込み待ちフラグ

    GameObject faderPanel;  //フェード用パネル
    Image m_image;          //パネル画像

    // Start is called before the first frame update
    void Start()
    {
        //パネル情報取得
        faderPanel = GameObject.Find("SceneFadePanel");
        //パネル画像取得
        m_image = faderPanel.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //シーン読み込みフラグが立っていたら、
        if (m_trigger)
        {
            //徐々にパネルを不透明に
            m_fadeTimer += Time.deltaTime;
            m_fadeAlpha = m_fadeTimer / m_fadeSpan;

            var c = m_image.color;
            m_image.color = new Color(c.r, c.g, c.b, m_fadeAlpha);

            //完全に不透明になったら、
            if (m_fadeAlpha >= 1.0f)
            {   //シーンを読み込み
                SceneManager.LoadScene(m_nextSceneName);
                m_trigger = false;
            }
        }
    }

    //シーンフェードでシーンを読み込む
    public void SetTrigger(string sceneName)
    {
        //他のシーンへのフェード中なら、戻る
        if (m_trigger) return;

        m_nextSceneName = sceneName;
        m_trigger = true;

        //フェード用パネルを前に
        faderPanel.transform.SetAsLastSibling();
    }
}
