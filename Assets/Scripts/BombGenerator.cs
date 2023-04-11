using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerator : MonoBehaviour
{
    [SerializeField] GameObject bombPref;   //���e�I�u�W�F�N�g�v���n�u
    [SerializeField] float genSpan = 1.0f;   //�������ԊԊu
    float genTiemr = 0.0f;
    [SerializeField] float minGenSpan = 0.25f;  //����span��max�l

    float bombGenRate = 90.0f; //���e�����S����
    [SerializeField] GameObject homingBombPrefab;    //�ǔ����e�I�u�W�F�N�g�v���n�u

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��莞�Ԃ��Ƃɐ���
        genTiemr += Time.deltaTime;
        if(genTiemr > genSpan)
        {
            genTiemr = 0.0f;

            //genSpan��minGenSpan�ɂȂ�܂ŁA���Ԃ��o���Ƃɋ��߂Ă���
            if(genSpan > minGenSpan)
            {
                genSpan -= 0.01f;
            }
            //���X�ǔ��e�𐶐��B���Ԃ��o���ƂɃ~�T�C���̊����𑝂₷
            //�i Bomb 90% : Missile 10%  ��  B 70% : M 30% �j
            if (bombGenRate < 70)
            {
                bombGenRate -= 0.1f;
            }

            float rnd = Random.Range(0, 100);   //0�`99�܂ł̗�����Ԃ�
            //�����_���ɒʏ픚�e���ǔ����e�𐶐�
            if (rnd < bombGenRate)
            {
                //���e����
                GenerateBomb();
            }
            else
            {
                //�ǔ����e����
                GenerateHomingBomb();
            }    
        }
    }

    void GenerateBomb()
    {
        //���e�I�u�W�F�N�g����
        GameObject bomb = Instantiate(bombPref) as GameObject;

        //��ʒ[���[���h���W�擾
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
        //�����ʒu�ݒ�
        float minx = leftBottom.x - 1.0f;
        float maxx = rightTop.x + 1.0f;
        float x = Random.Range(minx, maxx);
        float y = rightTop.y - 0.1f;
        Vector3 pos = new Vector3(x, y, 0);
        bomb.transform.position = pos;

        //�����ݒ�
        BombCtrler ctrler = bomb.GetComponent<BombCtrler>();
        float speed = ctrler.Speed = 0.8f;
        //�����ݒ�
        ctrler.Velocity = new Vector3(0, -1.0f * speed, 0);
        Vector3 veloc = ctrler.Velocity;
        //�����_���ɉ�]����
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
        //���e�I�u�W�F�N�g����
        GameObject homingBomb = Instantiate(homingBombPrefab) as GameObject;

        //��ʒ[���[���h���W�擾
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
        //�����ʒu�ݒ�
        float minx = leftBottom.x - 1.0f;
        float maxx = rightTop.x + 1.0f;
        float x = Random.Range(minx, maxx);
        float y = rightTop.y - 0.1f;
        Vector3 pos = new Vector3(x, y, 0);
        homingBomb.transform.position = pos;

        //�����ݒ�
        BombCtrler ctrler = homingBomb.GetComponent<BombCtrler>();
        float speed = ctrler.Speed = 1.25f;
        //�����ݒ�
        ctrler.Velocity = new Vector3(0, -1.0f * speed, 0);
        Vector3 veloc = ctrler.Velocity;
    }
}