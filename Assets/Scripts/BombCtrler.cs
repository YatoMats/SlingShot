using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCtrler : MonoBehaviour
{
    protected Vector3 veloc;  //移動方向
    public Vector3 Velocity
    {
        get { return veloc; }
        set { veloc = value; }
    }

    //[SerializeField]
    protected float speed = 1.0f;
    public float Speed  //速さ
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] float searchRadius = 2.0f; //索敵用円半径

    [SerializeField] GameObject explosionPref;  //爆発エフェクト用オブジェクトプレハブ

    protected GameManager gameManager;

    GameObject ground;   //地面オブジェクト
    float groundTop;    //地面オブジェクトの上端位置座標

    protected GameObject sling;  //ゴムパチンコオブジェクト

    [SerializeField] float stageEdgeOffset = 0.2f;  //ステージ左右端位置の補正値 

    private void Awake()
    {
        //下向き速度ベクトル設定
        veloc = Vector3.down;
        veloc.y *= speed;

        //回転処理
        //ベクトル回転処理
        /*float deg = 15.0f;
        float rad = deg * Mathf.Deg2Rad;
        Vector3 tmpVeloc;
        tmpVeloc.x = veloc.x * Mathf.Cos(rad) - veloc.y * Mathf.Sin(rad);
        tmpVeloc.y = veloc.x * Mathf.Sin(rad) + veloc.y * Mathf.Cos(rad);
        tmpVeloc.z = veloc.z;
        veloc = tmpVeloc;*/
    }

    // Start is called before the first frame update
    protected void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //ゴムパチンコオブジェクトの取得
        sling = GameObject.Find("Y");

        //地面オブジェクトの上端を取得
        ground = GameObject.Find("Ground");
    }

    // Update is called once per frame
    protected void Update()
    {
        //爆弾オブジェクトの位置座標、サイズを取得
        Vector3 pos = transform.position;
        Vector2 size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        float radius = size.x / 2.0f / 2.0f;  //爆弾の当たり判定円半径

        //ゴムパチンコの位置座標、サイズを取得
        Vector3 slingPos = sling.transform.position;
        Vector2 slingSize = sling.GetComponent<SpriteRenderer>().bounds.size;
        slingSize /= 2.0f;

        //移動方向変更
        Vector3 dir = slingPos - pos;   //爆弾からゴムパチンコへ向かうベクトル
        float dist = Mathf.Sqrt(Mathf.Pow(dir.x, 2.0f) + Mathf.Pow(dir.y, 2.0f));   //爆弾とゴムパチンコとの距離
        Vector3 norm;   //爆弾からゴムパチンコへ向かう向きの正規化ベクトル
        norm.x = dir.x / dist;
        norm.y = dir.y / dist;
        norm.z = 0.0f;

        //索敵円範囲内にゴムパチンコがあるなら、
        if ((slingPos.x - slingSize.x / 2 < pos.x + searchRadius && pos.x - searchRadius < slingPos.x + slingSize.x / 2) &&
            (slingPos.y - slingSize.y / 2 < pos.y + searchRadius && pos.y - searchRadius < slingPos.y + slingSize.y / 2))
        {
            //ゴムパチンコが爆弾より下側にあるなら、
            //float searchOffset = 0.5f;
            //if (pos.y > slingPos.y + searchOffset)
            {
                //向き速度をゴムパチンコの方向へ変更
                veloc = norm * speed;   //速度ベクトル
            }
        }

        //回転処理
        /*Vector2 v = transform.up.normalized;
        Vector2 s = (slingPos - pos).normalized;
        float vs = Vector3.Dot(v, s);
        if (vs < -1.0f || 1.0f < vs) vs = 1.0f;
        float theta = Mathf.Acos(vs);
        float outer = v.x * s.y - v.y * s.x;
        float rotateTheta = theta * 1.0f;

        //向きが逆なら、回転方向も逆に
        //if (transform.localScale.x < 0)
        //rotateTheta *= -1;

        //プレイヤーと自身との角度差が正かつ規定値より大きいならば、正の向きに回転
        if (outer > 0)
        {
            transform.Rotate(0, 0, rotateTheta);
        }
        //負の向きに回転
        else
        {
            transform.Rotate(0, 0, -rotateTheta);

        }*/

        //ゴムパチンコと接触したら、
        if ((slingPos.x - slingSize.x / 2 < pos.x + radius && pos.x - radius < slingPos.x + slingSize.x / 2) &&
            (slingPos.y - slingSize.y / 2 < pos.y + radius && pos.y - radius < slingPos.y + slingSize.y / 2)) 
        {
            //ゴムパチンコへのダメージ処理
            gameManager.Damage();
            //このオブジェクトを破棄
            Burst();
        }

        //移動方向へ移動処理
        pos += veloc * Time.deltaTime;
        
        //画面端ワールド座標取得
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //画面左端到達なら、
        if(pos.x < leftBottom.x + stageEdgeOffset)
        {
            //位置補正
            pos.x = leftBottom.x + stageEdgeOffset;
            //移動方向反転
            veloc.x *= -1.0f;
        }
        //画面右端到達なら、
        else if(rightTop.x - stageEdgeOffset < pos.x)
        {
            //位置補正
            pos.x = rightTop.x - stageEdgeOffset;
            //移動方向反転
            veloc.x *= -1.0f;
        }

        //画面外なら、
        /*if ((pos.x < leftBottom.x || rightTop.x < pos.x)
            || (pos.y < leftBottom.y || rightTop.y < pos.y))
        {
            //オブジェクトを破棄
            Destroy(gameObject);
        }*/

        //地面オブジェクトに当たったら、
        Vector2 groundSize = ground.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 groundPos = ground.transform.position;
        float groundTop = groundPos.y + groundSize.y / 2.0f;//* groundSclae.y 
        if (pos.y - size.y / 2.0f < groundTop) 
        {
            //自身の爆発、破壊処理
            Burst();
        }

        //移動後位置反映
        transform.position = pos;
    }

    //オブジェクト破棄時処理
    protected void Burst()
    {
        //衝突時エフェクト生成
        GameObject explosion = Instantiate(explosionPref) as GameObject;
        explosion.transform.position = transform.position;
        Destroy(explosion, 2.0f);

        //このオブジェクトを破棄
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //パチンコ弾と接触したら、
        if (collision.gameObject.tag == "ball")
        {
            //スコアポイント加算
            gameManager.AddPoints(100);

            //自身の爆発、破壊処理
            Burst();
        }
    }
}
