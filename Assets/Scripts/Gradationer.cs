using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradationer : MonoBehaviour
{
    //グラデーションの設定
    /*[SerializeField]
    private Gradient _gradient = default;

    //色を変える時間と現在の時間
    private readonly float FADE_COLOR_TIME = 4.0f;
    private float _currentTime = 0;*/

    //色を変える対象の画像
    //[SerializeField]
    private Text text = default;

    //[SerializeField]
    Image image;

    [SerializeField] string componentType = "Image";

    float timer = 0.0f;
    [SerializeField] float span = 3.0f;
    float freaquency;
    float color1 = 1.0f;
    float color2 = 0.0f;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        freaquency = 1.0f / span;

        if (componentType == "Text")
        {
            text = GetComponent<Text>();
        }
        else
        {
            image = GetComponent<Image>();
        }

        //色の設定
        /*var colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        //透明度の設定
        var alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        //Gradientのインスタンスを作成し、値を設定
        _gradient = new Gradient();
        _gradient.SetKeys(colorKey, alphaKey);

        //最初の色設定
        _text.color = _gradient.Evaluate(0);*/
    }

    //=================================================================================
    //更新
    //=================================================================================

    private void Update()
    {
        //時間を進める
       // _currentTime += Time.deltaTime;
        //var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        float offset = 0.5f;   //0.25f
        float offset2 = 0.5f;   //0.5f
        //色をグラデーション状に変化させる
        timer += Time.deltaTime * 2.0f * Mathf.PI * freaquency;
        color1 = Mathf.Sin(timer) * offset + offset2;
        color2 = 1.0f - color1;

        //色を反映
        Color c;
        if (componentType == "Text")
        {
            c = text.color;
            text.color = new Color(color1, color2, c.b, c.a);
        }else
        {
            c = image.color;
            image.color = new Color(color1, color2, c.b, c.a);
        }

        //色を変更
        //_text.color = _gradient.Evaluate(timeRate);
    }
}
