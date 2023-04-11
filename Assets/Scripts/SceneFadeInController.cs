using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeInController : MonoBehaviour
{
    float fadeAlpha = 1.0f;    //パネル透過度
    float fadeTimer = 0.0f;
    [SerializeField] float fadeSpan = 0.45f;     //フェードにかかる時間
    GameObject faderPanel;  //フェード用パネル
    Image panelImg;    //フェード用パネル画像データ

    // Start is called before the first frame update
    void Start()
    {
        //フェード用パネルを取得
        faderPanel = GameObject.Find("SceneFadePanel");
        panelImg = faderPanel.GetComponent<Image>();
        //フェード用パネル初期設定
        panelImg.color = Color.white;
        //フェード用パネルを前に
        faderPanel.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        //時間経過で透過度を上げる
        fadeTimer += Time.deltaTime;
        fadeAlpha = 1.0f - fadeTimer / fadeSpan;
        //パネルに透過度を反映
        Color c = panelImg.color;
        panelImg.color = new Color(c.r, c.g, c.b, fadeAlpha);

        //フェードが完了したら、
        if (fadeAlpha <= 0.0f)
        {   //フェードイン用オブジェクトを破棄
            Destroy(gameObject);
            //パネルの透明度を0.0fに
            panelImg.color = new Color(c.r, c.g, c.b, 0.0f);
            //パネルを奥側に
            faderPanel.transform.SetSiblingIndex(0);
        }
    }
}
