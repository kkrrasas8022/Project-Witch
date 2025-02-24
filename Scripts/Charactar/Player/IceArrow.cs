using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : MonoBehaviour
{
    [SerializeField] Player _owner;//주인
    [SerializeField] Vector3 _dir;//진행방향
    void Start()
    {
        Destroy(this.transform.root.gameObject,10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _dir * 15f * Time.deltaTime;//발사하여 이동
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            int i = Random.Range(0, 100);
            if (i <= _owner.Crit * 100)
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power * 2);
            }//치명타
            else
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power);
            }//플레이어의 공격력을 가져와서 맞은 적의 체력을 줄게함
            StartCoroutine(SlowEnemy(other,
                ((Witch)_owner)._slow_Power,
                ((Witch)_owner)._slow_Time));
            //적 객체를 느려지게 느려지게 함
            Destroy(this.transform.root.gameObject,2.1f);
        }
    }

    IEnumerator SlowEnemy(Collider other, float slow_power,float slow_time)//적을 느려지게 만드는 코루틴
    {
        float temp = other.gameObject.GetComponent<Charactar>().Speed;//적의 원래 속도를 받아저장함
        other.gameObject.GetComponent<Charactar>().Speed = temp * (1 - ((Witch)_owner)._slow_Power);//적의 속도를 원래속도에 느려진 속도로 계산하여 할당함
        yield return new WaitForSeconds(slow_time);//느려지는 시간동안 적이 느려짐
        if(other!=null)
        {
            other.gameObject.GetComponent<Charactar>().Speed = temp;//느려지는 시간이 끝나면 원래 속도로 돌아옴
        }
    }

    public void Init(Player owner,Vector3 dir)//객제가 생성될 때 객체의 주인을 알려줌
    {
        _owner = owner;
        _dir= dir;
    }
}
