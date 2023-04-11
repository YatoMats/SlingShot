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
        //�ŏ�����v���C���[���Ɍ������Đi��

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

        //�������x���S���p�`���R�̕����֕ύX
        veloc = norm * speed;   //���x�x�N�g��

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�p�`���R�e�ƐڐG������A
        if (collision.gameObject.tag == "ball")
        {
            //�X�R�A�|�C���g���Z
            gameManager.AddPoints(200);

            //���g�̔����A�j�󏈗�
            Burst();
        }
    }
}
