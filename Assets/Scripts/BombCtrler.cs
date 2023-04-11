using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCtrler : MonoBehaviour
{
    protected Vector3 veloc;  //�ړ�����
    public Vector3 Velocity
    {
        get { return veloc; }
        set { veloc = value; }
    }

    //[SerializeField]
    protected float speed = 1.0f;
    public float Speed  //����
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] float searchRadius = 2.0f; //���G�p�~���a

    [SerializeField] GameObject explosionPref;  //�����G�t�F�N�g�p�I�u�W�F�N�g�v���n�u

    protected GameManager gameManager;

    GameObject ground;   //�n�ʃI�u�W�F�N�g
    float groundTop;    //�n�ʃI�u�W�F�N�g�̏�[�ʒu���W

    protected GameObject sling;  //�S���p�`���R�I�u�W�F�N�g

    [SerializeField] float stageEdgeOffset = 0.2f;  //�X�e�[�W���E�[�ʒu�̕␳�l 

    private void Awake()
    {
        //���������x�x�N�g���ݒ�
        veloc = Vector3.down;
        veloc.y *= speed;

        //��]����
        //�x�N�g����]����
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

        //�S���p�`���R�I�u�W�F�N�g�̎擾
        sling = GameObject.Find("Y");

        //�n�ʃI�u�W�F�N�g�̏�[���擾
        ground = GameObject.Find("Ground");
    }

    // Update is called once per frame
    protected void Update()
    {
        //���e�I�u�W�F�N�g�̈ʒu���W�A�T�C�Y���擾
        Vector3 pos = transform.position;
        Vector2 size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        float radius = size.x / 2.0f / 2.0f;  //���e�̓����蔻��~���a

        //�S���p�`���R�̈ʒu���W�A�T�C�Y���擾
        Vector3 slingPos = sling.transform.position;
        Vector2 slingSize = sling.GetComponent<SpriteRenderer>().bounds.size;
        slingSize /= 2.0f;

        //�ړ������ύX
        Vector3 dir = slingPos - pos;   //���e����S���p�`���R�֌������x�N�g��
        float dist = Mathf.Sqrt(Mathf.Pow(dir.x, 2.0f) + Mathf.Pow(dir.y, 2.0f));   //���e�ƃS���p�`���R�Ƃ̋���
        Vector3 norm;   //���e����S���p�`���R�֌����������̐��K���x�N�g��
        norm.x = dir.x / dist;
        norm.y = dir.y / dist;
        norm.z = 0.0f;

        //���G�~�͈͓��ɃS���p�`���R������Ȃ�A
        if ((slingPos.x - slingSize.x / 2 < pos.x + searchRadius && pos.x - searchRadius < slingPos.x + slingSize.x / 2) &&
            (slingPos.y - slingSize.y / 2 < pos.y + searchRadius && pos.y - searchRadius < slingPos.y + slingSize.y / 2))
        {
            //�S���p�`���R�����e��艺���ɂ���Ȃ�A
            //float searchOffset = 0.5f;
            //if (pos.y > slingPos.y + searchOffset)
            {
                //�������x���S���p�`���R�̕����֕ύX
                veloc = norm * speed;   //���x�x�N�g��
            }
        }

        //��]����
        /*Vector2 v = transform.up.normalized;
        Vector2 s = (slingPos - pos).normalized;
        float vs = Vector3.Dot(v, s);
        if (vs < -1.0f || 1.0f < vs) vs = 1.0f;
        float theta = Mathf.Acos(vs);
        float outer = v.x * s.y - v.y * s.x;
        float rotateTheta = theta * 1.0f;

        //�������t�Ȃ�A��]�������t��
        //if (transform.localScale.x < 0)
        //rotateTheta *= -1;

        //�v���C���[�Ǝ��g�Ƃ̊p�x���������K��l���傫���Ȃ�΁A���̌����ɉ�]
        if (outer > 0)
        {
            transform.Rotate(0, 0, rotateTheta);
        }
        //���̌����ɉ�]
        else
        {
            transform.Rotate(0, 0, -rotateTheta);

        }*/

        //�S���p�`���R�ƐڐG������A
        if ((slingPos.x - slingSize.x / 2 < pos.x + radius && pos.x - radius < slingPos.x + slingSize.x / 2) &&
            (slingPos.y - slingSize.y / 2 < pos.y + radius && pos.y - radius < slingPos.y + slingSize.y / 2)) 
        {
            //�S���p�`���R�ւ̃_���[�W����
            gameManager.Damage();
            //���̃I�u�W�F�N�g��j��
            Burst();
        }

        //�ړ������ֈړ�����
        pos += veloc * Time.deltaTime;
        
        //��ʒ[���[���h���W�擾
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //��ʍ��[���B�Ȃ�A
        if(pos.x < leftBottom.x + stageEdgeOffset)
        {
            //�ʒu�␳
            pos.x = leftBottom.x + stageEdgeOffset;
            //�ړ��������]
            veloc.x *= -1.0f;
        }
        //��ʉE�[���B�Ȃ�A
        else if(rightTop.x - stageEdgeOffset < pos.x)
        {
            //�ʒu�␳
            pos.x = rightTop.x - stageEdgeOffset;
            //�ړ��������]
            veloc.x *= -1.0f;
        }

        //��ʊO�Ȃ�A
        /*if ((pos.x < leftBottom.x || rightTop.x < pos.x)
            || (pos.y < leftBottom.y || rightTop.y < pos.y))
        {
            //�I�u�W�F�N�g��j��
            Destroy(gameObject);
        }*/

        //�n�ʃI�u�W�F�N�g�ɓ���������A
        Vector2 groundSize = ground.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 groundPos = ground.transform.position;
        float groundTop = groundPos.y + groundSize.y / 2.0f;//* groundSclae.y 
        if (pos.y - size.y / 2.0f < groundTop) 
        {
            //���g�̔����A�j�󏈗�
            Burst();
        }

        //�ړ���ʒu���f
        transform.position = pos;
    }

    //�I�u�W�F�N�g�j��������
    protected void Burst()
    {
        //�Փˎ��G�t�F�N�g����
        GameObject explosion = Instantiate(explosionPref) as GameObject;
        explosion.transform.position = transform.position;
        Destroy(explosion, 2.0f);

        //���̃I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�p�`���R�e�ƐڐG������A
        if (collision.gameObject.tag == "ball")
        {
            //�X�R�A�|�C���g���Z
            gameManager.AddPoints(100);

            //���g�̔����A�j�󏈗�
            Burst();
        }
    }
}
