using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    public enum ComponentType
    {
        Image, Text
    }

    [SerializeField] float blinkSpan = 1.75f;    //周期

    Image image;
    Text text;
    //TextMesh txtMesh;

    float alpha = 1.0f;
    float freaquency;   //振動数
    float timer = 0.0f;

    [SerializeField] ComponentType type = ComponentType.Image;

    // Start is called before the first frame update
    void Start()
    {
        freaquency = 1.0f / blinkSpan; //振動数

        if (type == ComponentType.Text) 
            text = GetComponent<Text>();
        else
            image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float offset = 0.25f;   //0.5f
        float offset2 = 0.5f;   //0.5f

        //徐々に透明に
        timer += Time.deltaTime * 2.0f * Mathf.PI * freaquency;
        alpha = Mathf.Sin(timer) * offset + offset2;

        //透明度を反映
        Color c;
        if (type == ComponentType.Text) 
        {
            c = text.color;
            text.color = new Color(c.r, c.g, c.b, alpha);
        }
        else
        {
            c = image.color;
            image.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
