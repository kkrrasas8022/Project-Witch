using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Witch : Player
{
    [Header("Attack Info")]
    public GameObject fireBall_Prefab;  //ȭ���� ����ü ������
    public GameObject iceArrow_Prefab;  //����ȭ�� ����ü ������
    public Transform left_Point;        //�޼� ���� ����ü �߻���ġ
    public Transform right_Point;       //������ ���� ����ü �߻���ġ
    public float _slow_Power;           //����ȭ��� ���� ������ �ϴ� ��
    public float _slow_Time;            //����ȭ��� ���� ������ �ϴ� �ð�

    protected override void LeftAttack()//�޼հ���(����ȭ��)
    {
        if (GameObject.Find("Game_Manager").GetComponent<Game_Manager>().isShopOpen == true)
            return;
        if (canRun == true)
            return;
        if (_leftTime >= _leftcoolTime)
        {
            Ray ray=new Ray(Camera.main.transform.position,Camera.main.transform.forward);
            RaycastHit hit=new RaycastHit();
            Vector3 arrow_Dir;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag=="Enemy")
                {
                    arrow_Dir= hit.transform.position-left_Point.position;
                }
                else
                {
                    arrow_Dir=Camera.main.transform.forward;
                }
            }
            else
            {
                arrow_Dir = Camera.main.transform.forward;
            }
            base.Sound_Manager.Sound_Ice_Attack();
            GameObject temp = Instantiate(iceArrow_Prefab, left_Point.position, left_Point.rotation);
            temp.GetComponent<IceArrow>().Init(this,arrow_Dir.normalized);
            _leftTime = 0;
        }
        
        base.LeftAttack();
    }

    protected override void RightAttack()//�����հ���(ȭ����)
    {
        if (GameObject.Find("Game_Manager").GetComponent<Game_Manager>().isShopOpen == true)
            return;
        if (canRun == true)
            return;
        if (_rightTime >= _rightcoolTime)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit = new RaycastHit();
            Vector3 arrow_Dir;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Enemy")
                {
                    arrow_Dir = hit.transform.position - right_Point.position;
                }
                else
                {
                    arrow_Dir = Camera.main.transform.forward;
                }
            }
            else
            {
                arrow_Dir = Camera.main.transform.forward;
            }
            GameObject temp = Instantiate(fireBall_Prefab, right_Point.position, right_Point.rotation);
            base.Sound_Manager.Sound_Fire_Attack();
            temp.GetComponent<FireBall>().Init(this, arrow_Dir.normalized);
            _rightTime = 0;
        }
        
        base.RightAttack();
    }
}
