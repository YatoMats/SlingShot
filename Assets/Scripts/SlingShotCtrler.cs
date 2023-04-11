using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotCtrler : MonoBehaviour
{
    Vector2 swipeStartPos;  //�X���C�v�J�n�ʒu
    Vector2 swipeEndPos;    //�X���C�v�I���ʒu

    Vector3 slingEdge1; //�p�`���R�R�[�ʒu
    Vector3 slingEdge2; //�p�`���R�R�[�ʒu
    Vector3 slingMidPos;    //�p�`���R�R�����ʒu
    Vector3 startSlingMidPos;   //�p�`���R�R���������ʒu   
    LineRenderer lineRenderer;  //�p�`���R�R�`��p

    bool isShooting = false;    //�e�𔭎˒����ǂ���
    public bool IsShooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }

    [SerializeField] GameObject ballPref;    //�p�`���R�e�v���n�u
    GameObject ball;    //�p�`���R�e�C���X�^���X

    AudioSource aud;

    [SerializeField] float drawSlingRadius;  //�p�`���R�R��������~�̔��a
    
    // Start is called before the first frame update
    void Start()
    {
        //�����Đ��p�R���|�[�l���g���擾
        aud = GetComponent<AudioSource>();

        Vector2 pos = transform.position;
        //�p�`���R�R�[�ʒu�ݒ�
        slingEdge1 = new Vector3(pos.x - 0.4f, pos.y + 0.4f, -1.0f);
        slingEdge2 = new Vector3(pos.x + 0.4f, pos.y + 0.4f, -1.0f);
        slingMidPos = new Vector3(pos.x, pos.y - 0.25f, -1.0f);
        startSlingMidPos = slingMidPos;
        //��z���ʒu���W�����̃I�u�W�F�N�g��艜����������A�J�����ʒu����O�����ƕ\������Ȃ�
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // ���̕�
        renderer.startWidth = 0.05f;
        renderer.endWidth = 0.05f;
        // ���_�̐�
        renderer.positionCount = 3;
        //�R��`��
        renderer.SetPosition(0, slingEdge1);
        renderer.SetPosition(1, slingMidPos);
        renderer.SetPosition(2, slingEdge2);

        //TOOD:lineRenderer���܂Ƃ߂�
    }

    // Update is called once per frame
    void Update()
    {
        //��ʏ�Ɋ��ɒe������Ȃ�A�S���p�`���R�̑�����󂯕t���Ȃ�
        if (isShooting) return;

        //�X���C�v�����ɉ����āA�Ƃ΂��e�̐�����ς���
        //�^�b�v�n��
        if (Input.GetMouseButtonDown(0)){
            //�X���C�v�J�n�ʒu���擾
            swipeStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //�S���p�`���R�̈ʒu���W�A�T�C�Y���擾
            Vector3 slingPos = transform.position;
            Vector2 slingSize = GetComponent<SpriteRenderer>().bounds.size;
            //slingSize /= 2.0f;

            //�X���C�v�J�n�ʒu���S���p�`���R���痣��Ă�����A�������Ȃ�
            if ((slingPos.x - slingSize.x / 2 < swipeStartPos.x && swipeStartPos.x < slingPos.x + slingSize.x / 2) &&
                (slingPos.y - slingSize.y / 2 < swipeStartPos.y && swipeStartPos.y < slingPos.y + slingSize.y / 2))
            {
                //�^�b�v�ʒu�ɒe�𐶐�
                ball = Instantiate(ballPref, new Vector3(swipeStartPos.x, swipeStartPos.y, 0.0f), Quaternion.identity);
                ball.GetComponent<BallCtrler>().Velocity = new Vector3(0, 5.0f, 0); //0,0,0

                //�e�̓����蔻��𔭎˂܂Ő؂�
                ball.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        //�^�b�v�I���
        else if (Input.GetMouseButtonUp(0))
        {
            if (ball == null) return;

            //���˒��t���O�𗧂Ă�
            isShooting = true;

            //�X���C�v�I���ʒu���擾
            swipeEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //�����蔻���߂�
            ball.GetComponent<CircleCollider2D>().enabled = true;

            //�X���C�v�����A�����A���K���x�N�g���v�Z
            Vector2 s = new Vector2(transform.position.x, slingEdge1.y);
            Vector2 dir = s - swipeEndPos;
            float dist = Mathf.Sqrt((Mathf.Pow(dir.x, 2.0f) + Mathf.Pow(dir.y, 2.0f)));
            Vector2 dirNorm = new Vector2(dir.x / dist, dir.y / dist);
            
            //�e���x�x�N�g���ݒ�
            BallCtrler ctrler = ball.GetComponent<BallCtrler>();
            //�ݒ肵���Œᑬ�x�����Ȃ�A�͈͓��ɂȂ�悤��
            float speed = dist * 3.0f;
            float minSpeed = 7.0f;
            if(speed < minSpeed)
            {
                speed = minSpeed;
            }
            //�e���x���f
            ctrler.Velocity = dirNorm * speed;

            //Lerp�ŕR�����̈ʒu�ɖ߂�
            slingMidPos = startSlingMidPos;
        }
        //�^�b�v���A
        //�^�b�v�ʒu���擾���A�S���p�`���R�R�̒����_���^�b�v�ʒu��
        else if (Input.GetMouseButton(0))
        {
            if (ball == null) return;

            //�X�N���[����̃}�E�X�ʒu����A���[���h��̂��̈ʒu���擾
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0.0f;

            //�X�N���[���[�̃��[���h���W���擾
            float depth = 5.0f;
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
            //�^�b�v�ʒu���͈͊O�Ȃ�A�߂�
            //���[����
            if (mouseWorldPos.x < leftBottom.x)
            {
                mouseWorldPos.x = leftBottom.x;
            }
            //�E�[�z��
            if(rightTop.x < mouseWorldPos.x)
            {
                mouseWorldPos.x = rightTop.x;
            }
            //���[�z��
            if(mouseWorldPos.y < leftBottom.y)
            {
                mouseWorldPos.y = leftBottom.y;
            }
            //�^�b�v�ʒu����߂�����߂�
            float offsety = 3.0f;
            if (mouseWorldPos.y > transform.position.y + offsety)
            {
                mouseWorldPos.y = transform.position.y + offsety;
            }

            //�e�ʒu���^�b�v�ʒu��
            ball.transform.position = mouseWorldPos;

            //�R�ʒu���^�b�v�ʒu��
            slingMidPos = mouseWorldPos;
        }
        //�^�b�v���Ă��Ȃ��Ȃ�A�R�ʒu��߂�
        else
        {
            slingMidPos = startSlingMidPos;
        }

        Vector2 pos = transform.position;
        //�p�`���R�R�[�ʒu�ݒ�
        slingEdge1 = new Vector3(pos.x - 0.4f, pos.y + 0.4f, -1.0f);
        slingEdge2 = new Vector3(pos.x + 0.4f, pos.y + 0.4f, -1.0f);
        //��z���ʒu���W�����̃I�u�W�F�N�g��艜����������A�J�����ʒu����O�����ƕ\������Ȃ�
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // ���̕�
        renderer.startWidth = 0.05f;
        renderer.endWidth = 0.05f;
        // ���_�̐�
        renderer.positionCount = 3;
        //�R��`��
        renderer.SetPosition(0, slingEdge1);
        renderer.SetPosition(1, slingMidPos);
        renderer.SetPosition(2, slingEdge2);
    }
}
