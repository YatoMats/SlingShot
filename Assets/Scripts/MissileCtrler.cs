using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrler : BombCtrler
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        //base.Velocity
    }

    // Update is called once per frame
    new void Update()
    {
        //最初からプレイヤー側に向かって進む

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

        //向き速度をゴムパチンコの方向へ変更
        veloc = norm * speed;   //速度ベクトル

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //パチンコ弾と接触したら、
        if (collision.gameObject.tag == "ball")
        {
            //スコアポイント加算
            gameManager.AddPoints(200);

            //自身の爆発、破壊処理
            Burst();
        }
    }
}
