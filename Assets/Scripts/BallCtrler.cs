using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCtrler : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;    //�I�u�W�F�N�g�̈ړ����x
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private Vector3 veloc = Vector3.up;  //�I�u�W�F�N�g�̈ړ������A���x
    public Vector3 Velocity
    {
        get { return veloc; }
        set { veloc = value; }
    }

    SlingShotCtrler slingCtrler;    //�S���p�`���R���
    GameManager gameManager;

    private void Awake()
    {
        slingCtrler = GameObject.Find("Y").GetComponent<SlingShotCtrler>();

        //��������x�x�N�g���ݒ�
        veloc = Vector3.up;
        veloc.y *= speed;

        //�x�N�g����]����
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
        //���̃I�u�W�F�N�g�̈ʒu���W���擾
        Vector3 pos = transform.position;

        //�ړ������ֈړ�����
        pos += veloc * Time.deltaTime;
        
        //��ʒ[���[���h���W�擾
        float depth = 5.0f;
        Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //��ʊO�Ȃ�A    //TODO:offset�l������
        if ( (pos.x < leftBottom.x || rightTop.x < pos.x) 
            || (pos.y < leftBottom.y || rightTop.y < pos.y) )
        {
            //���˒��t���O��܂�
            slingCtrler.IsShooting = false;
            //�R���{��������
            gameManager.ResetCombo();
            //�I�u�W�F�N�g��j��
            Destroy(gameObject);
        }

        //�ړ���ʒu���f
        transform.position = pos;
    }
}
