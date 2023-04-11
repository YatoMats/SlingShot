using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCtrler : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;    //オブジェクトの移動速度
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private Vector3 veloc = Vector3.up;  //オブジェクトの移動方向、速度
    public Vector3 Velocity
    {
        get { return veloc; }
        set { veloc = value; }
    }

    SlingShotCtrler slingCtrler;    //ゴムパチンコ情報
    GameManager gameManager;

    private void Awake()
    {
        slingCtrler = GameObject.Find("Y").GetComponent<SlingShotCtrler>();

        //上向き速度ベクトル設定
        veloc = Vector3.up;
        veloc.y *= speed;

        //ベクトル回転処理
        float deg = -15.0f;
        float rad = deg * Mathf.Deg2Rad;
        Vector3 tmpVeloc;
        tmpVeloc.x = veloc.x * Mathf.Cos(rad) - veloc.y * Mathf.Sin(rad);
        tmpVeloc.y = veloc.x * Mathf.Sin(rad) + veloc.y * Mathf.Cos(rad);
        tmpVeloc.z = veloc.z;
        veloc = tmpVeloc;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //このオブジェクトの位置座標を取得
        Vector3 pos = transform.position;

        //移動方向へ移動処理
        pos += veloc * Time.deltaTime;
        
        //画面端ワールド座標取得
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //画面外なら、    //TODO:offset値を入れる
        if ( (pos.x < leftBottom.x || rightTop.x < pos.x) 
            || (pos.y < leftBottom.y || rightTop.y < pos.y) )
        {
            //発射中フラグを折る
            slingCtrler.IsShooting = false;
            //コンボ数初期化
            gameManager.ResetCombo();
            //オブジェクトを破棄
            Destroy(gameObject);
        }

        //移動後位置反映
        transform.position = pos;
    }
}
