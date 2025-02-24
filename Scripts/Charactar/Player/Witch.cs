using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Witch : Player
{
    [Header("Attack Info")]
    public GameObject fireBall_Prefab;  //화염구 투사체 프리팹
    public GameObject iceArrow_Prefab;  //얼음화살 투사체 프리팹
    public Transform left_Point;        //왼손 공격 투사체 발사위치
    public Transform right_Point;       //오른손 공격 투사체 발사위치
    public float _slow_Power;           //얼음화살로 적을 느리게 하는 힘
    public float _slow_Time;            //얼음화살로 적을 느리게 하는 시간

    protected override void LeftAttack()//왼손공격(얼음화살)
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

    protected override void RightAttack()//오른손공격(화염구)
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
