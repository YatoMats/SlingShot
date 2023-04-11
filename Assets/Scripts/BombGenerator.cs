using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerator : MonoBehaviour
{
    [SerializeField] GameObject bombPref;   //爆弾オブジェクトプレハブ
    [SerializeField] float genSpan = 1.0f;   //生成時間間隔
    float genTiemr = 0.0f;
    [SerializeField] float minGenSpan = 0.25f;  //生成spanのmax値

    float bombGenRate = 90.0f; //爆弾生成百分率
    [SerializeField] GameObject homingBombPrefab;    //追尾爆弾オブジェクトプレハブ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //一定時間ごとに生成
        genTiemr += Time.deltaTime;
        if(genTiemr > genSpan)
        {
            genTiemr = 0.0f;

            //genSpanをminGenSpanになるまで、時間が経つごとに狭めていく
            if(genSpan > minGenSpan)
            {
                genSpan -= 0.01f;
            }
            //時々追尾弾を生成。時間が経つごとにミサイルの割合を増やす
            //（ Bomb 90% : Missile 10%  →  B 70% : M 30% ）
            if (bombGenRate < 70)
            {
                bombGenRate -= 0.1f;
            }

            float rnd = Random.Range(0, 100);   //0～99までの乱数を返す
            //ランダムに通常爆弾か追尾爆弾を生成
            if (rnd < bombGenRate)
            {
                //爆弾生成
                GenerateBomb();
            }
            else
            {
                //追尾爆弾生成
                GenerateHomingBomb();
            }    
        }
    }

    void GenerateBomb()
    {
        //爆弾オブジェクト生成
        GameObject bomb = Instantiate(bombPref) as GameObject;

        //画面端ワールド座標取得
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
        //生成位置設定
        float minx = leftBottom.x - 1.0f;
        float maxx = rightTop.x + 1.0f;
        float x = Random.Range(minx, maxx);
        float y = rightTop.y - 0.1f;
        Vector3 pos = new Vector3(x, y, 0);
        bomb.transform.position = pos;

        //速さ設定
        BombCtrler ctrler = bomb.GetComponent<BombCtrler>();
        float speed = ctrler.Speed = 0.8f;
        //向き設定
        ctrler.Velocity = new Vector3(0, -1.0f * speed, 0);
        Vector3 veloc = ctrler.Velocity;
        //ランダムに回転処理
        float maxr = 15.0f;
        float deg = Random.Range(-maxr, maxr);
        float rad = deg * Mathf.Deg2Rad;
        Vector3 tmpVeloc;
        tmpVeloc.x = veloc.x * Mathf.Cos(rad) - veloc.y * Mathf.Sin(rad);
        tmpVeloc.y = veloc.x * Mathf.Sin(rad) + veloc.y * Mathf.Cos(rad);
        tmpVeloc.z = veloc.z;
        ctrler.Velocity = tmpVeloc;
    }

    void GenerateHomingBomb()
    {
        //爆弾オブジェクト生成
        GameObject homingBomb = Instantiate(homingBombPrefab) as GameObject;

        //画面端ワールド座標取得
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
        //生成位置設定
        float minx = leftBottom.x - 1.0f;
        float maxx = rightTop.x + 1.0f;
        float x = Random.Range(minx, maxx);
        float y = rightTop.y - 0.1f;
        Vector3 pos = new Vector3(x, y, 0);
        homingBomb.transform.position = pos;

        //速さ設定
        BombCtrler ctrler = homingBomb.GetComponent<BombCtrler>();
        float speed = ctrler.Speed = 1.25f;
        //向き設定
        ctrler.Velocity = new Vector3(0, -1.0f * speed, 0);
        Vector3 veloc = ctrler.Velocity;
    }
}