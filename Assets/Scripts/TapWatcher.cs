using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapWatcher : MonoBehaviour
{
    /*AudioSource aud;
    [SerializeField] AudioClip tapSE;
    [SerializeField] string nextSceneName = "TitleScene";
    SceneFadeOutController sceneFader;
    bool buttonIsPushed = false;*/

    //Button button;

    // Start is called before the first frame update
    void Start()
    {
        //button = GetComponent<Button>();

        //ボタンオブジェクトを画面いっぱいに広げる
        RectTransform rect = GetComponent<RectTransform>();
        float width = Screen.width;
        float height = Screen.height;
        rect.position = new Vector3(width / 2.0f, height / 2.0f, 0);
        rect.sizeDelta = new Vector2(width, height);

        //aud = GetComponent<AudioSource>();
        //sceneFader = GameObject.Find("SceneFadeOutController").GetComponent<SceneFadeOutController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //他のゲームオブジェクトをタップしていないか
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("PointerOverGameObject");
            return;
        }
#else 
    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
        return;
    }
#endif
        //画面がタップされた or 左クリックされたら、
        if (Input.GetMouseButtonUp(0))
        {
            //効果音再生
            aud.PlayOneShot(tapSE, 1.0f);
            //次のシーンへのフェードアウト
            sceneFader.SetTrigger(nextSceneName);
        }*/
    }
}
