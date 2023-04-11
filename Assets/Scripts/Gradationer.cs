using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradationer : MonoBehaviour
{
    //�O���f�[�V�����̐ݒ�
    /*[SerializeField]
    private Gradient _gradient = default;

    //�F��ς��鎞�Ԃƌ��݂̎���
    private readonly float FADE_COLOR_TIME = 4.0f;
    private float _currentTime = 0;*/

    //�F��ς���Ώۂ̉摜
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
    //������
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

        //�F�̐ݒ�
        /*var colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        //�����x�̐ݒ�
        var alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        //Gradient�̃C���X�^���X���쐬���A�l��ݒ�
        _gradient = new Gradient();
        _gradient.SetKeys(colorKey, alphaKey);

        //�ŏ��̐F�ݒ�
        _text.color = _gradient.Evaluate(0);*/
    }

    //=================================================================================
    //�X�V
    //=================================================================================

    private void Update()
    {
        //���Ԃ�i�߂�
       // _currentTime += Time.deltaTime;
        //var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        float offset = 0.5f;   //0.25f
        float offset2 = 0.5f;   //0.5f
        //�F���O���f�[�V������ɕω�������
        timer += Time.deltaTime * 2.0f * Mathf.PI * freaquency;
        color1 = Mathf.Sin(timer) * offset + offset2;
        color2 = 1.0f - color1;

        //�F�𔽉f
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

        //�F��ύX
        //_text.color = _gradient.Evaluate(timeRate);
    }
}
