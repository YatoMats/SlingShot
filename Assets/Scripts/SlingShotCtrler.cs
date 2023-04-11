using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotCtrler : MonoBehaviour
{
    Vector2 swipeStartPos;  //スワイプ開始位置
    Vector2 swipeEndPos;    //スワイプ終了位置

    Vector3 slingEdge1; //パチンコ紐端位置
    Vector3 slingEdge2; //パチンコ紐端位置
    Vector3 slingMidPos;    //パチンコ紐中央位置
    Vector3 startSlingMidPos;   //パチンコ紐中央初期位置   
    LineRenderer lineRenderer;  //パチンコ紐描画用

    bool isShooting = false;    //弾を発射中かどうか
    public bool IsShooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }

    [SerializeField] GameObject ballPref;    //パチンコ弾プレハブ
    GameObject ball;    //パチンコ弾インスタンス

    AudioSource aud;

    [SerializeField] float drawSlingRadius;  //パチンコ紐を引ける円の半径
    
    // Start is called before the first frame update
    void Start()
    {
        //音声再生用コンポーネントを取得
        aud = GetComponent<AudioSource>();

        Vector2 pos = transform.position;
        //パチンコ紐端位置設定
        slingEdge1 = new Vector3(pos.x - 0.4f, pos.y + 0.4f, -1.0f);
        slingEdge2 = new Vector3(pos.x + 0.4f, pos.y + 0.4f, -1.0f);
        slingMidPos = new Vector3(pos.x, pos.y - 0.25f, -1.0f);
        startSlingMidPos = slingMidPos;
        //※z軸位置座標が他のオブジェクトより奥側だったり、カメラ位置より手前側だと表示されない
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // 線の幅
        renderer.startWidth = 0.05f;
        renderer.endWidth = 0.05f;
        // 頂点の数
        renderer.positionCount = 3;
        //紐を描画
        renderer.SetPosition(0, slingEdge1);
        renderer.SetPosition(1, slingMidPos);
        renderer.SetPosition(2, slingEdge2);

        //TOOD:lineRendererをまとめる
    }

    // Update is called once per frame
    void Update()
    {
        //画面上に既に弾があるなら、ゴムパチンコの操作を受け付けない
        if (isShooting) return;

        //スワイプ距離に応じて、とばす弾の勢いを変える
        //タップ始め
        if (Input.GetMouseButtonDown(0)){
            //スワイプ開始位置を取得
            swipeStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //ゴムパチンコの位置座標、サイズを取得
            Vector3 slingPos = transform.position;
            Vector2 slingSize = GetComponent<SpriteRenderer>().bounds.size;
            //slingSize /= 2.0f;

            //スワイプ開始位置がゴムパチンコから離れていたら、反応しない
            if ((slingPos.x - slingSize.x / 2 < swipeStartPos.x && swipeStartPos.x < slingPos.x + slingSize.x / 2) &&
                (slingPos.y - slingSize.y / 2 < swipeStartPos.y && swipeStartPos.y < slingPos.y + slingSize.y / 2))
            {
                //タップ位置に弾を生成
                ball = Instantiate(ballPref, new Vector3(swipeStartPos.x, swipeStartPos.y, 0.0f), Quaternion.identity);
                ball.GetComponent<BallCtrler>().Velocity = new Vector3(0, 5.0f, 0); //0,0,0

                //弾の当たり判定を発射まで切る
                ball.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        //タップ終わり
        else if (Input.GetMouseButtonUp(0))
        {
            if (ball == null) return;

            //発射中フラグを立てる
            isShooting = true;

            //スワイプ終了位置を取得
            swipeEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //当たり判定を戻す
            ball.GetComponent<CircleCollider2D>().enabled = true;

            //スワイプ向き、距離、正規化ベクトル計算
            Vector2 s = new Vector2(transform.position.x, slingEdge1.y);
            Vector2 dir = s - swipeEndPos;
            float dist = Mathf.Sqrt((Mathf.Pow(dir.x, 2.0f) + Mathf.Pow(dir.y, 2.0f)));
            Vector2 dirNorm = new Vector2(dir.x / dist, dir.y / dist);
            
            //弾速度ベクトル設定
            BallCtrler ctrler = ball.GetComponent<BallCtrler>();
            //設定した最低速度未満なら、範囲内になるように
            float speed = dist * 3.0f;
            float minSpeed = 7.0f;
            if(speed < minSpeed)
            {
                speed = minSpeed;
            }
            //弾速度反映
            ctrler.Velocity = dirNorm * speed;

            //Lerpで紐を元の位置に戻す
            slingMidPos = startSlingMidPos;
        }
        //タップ中、
        //タップ位置を取得し、ゴムパチンコ紐の中央点をタップ位置に
        else if (Input.GetMouseButton(0))
        {
            if (ball == null) return;

            //スクリーン上のマウス位置から、ワールド上のその位置を取得
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0.0f;

            //スクリーン端のワールド座標を取得
            float depth = 5.0f;
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
            //タップ位置が範囲外なら、戻す
            //左端超え
            if (mouseWorldPos.x < leftBottom.x)
            {
                mouseWorldPos.x = leftBottom.x;
            }
            //右端越え
            if(rightTop.x < mouseWorldPos.x)
            {
                mouseWorldPos.x = rightTop.x;
            }
            //下端越え
            if(mouseWorldPos.y < leftBottom.y)
            {
                mouseWorldPos.y = leftBottom.y;
            }
            //タップ位置が上過ぎたら戻す
            float offsety = 3.0f;
            if (mouseWorldPos.y > transform.position.y + offsety)
            {
                mouseWorldPos.y = transform.position.y + offsety;
            }

            //弾位置をタップ位置に
            ball.transform.position = mouseWorldPos;

            //紐位置をタップ位置に
            slingMidPos = mouseWorldPos;
        }
        //タップしていないなら、紐位置を戻す
        else
        {
            slingMidPos = startSlingMidPos;
        }

        Vector2 pos = transform.position;
        //パチンコ紐端位置設定
        slingEdge1 = new Vector3(pos.x - 0.4f, pos.y + 0.4f, -1.0f);
        slingEdge2 = new Vector3(pos.x + 0.4f, pos.y + 0.4f, -1.0f);
        //※z軸位置座標が他のオブジェクトより奥側だったり、カメラ位置より手前側だと表示されない
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // 線の幅
        renderer.startWidth = 0.05f;
        renderer.endWidth = 0.05f;
        // 頂点の数
        renderer.positionCount = 3;
        //紐を描画
        renderer.SetPosition(0, slingEdge1);
        renderer.SetPosition(1, slingMidPos);
        renderer.SetPosition(2, slingEdge2);
    }
}
