using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] Player _owner;//주인(플레이어)
    [SerializeField] Vector3 _dir;//진행방향
    void Start()
    { 
        Destroy(this.transform.root.gameObject, 10f);
    }//Start

    // Update is called once per frame
    void Update()
    {
        transform.position += _dir * 10f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            print(this.name + " " +other.name + " hit");
            int i = Random.Range(0, 100);
            if (i <= _owner.Crit * 100)
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power*2);
            }//치명타
            else
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power);
            }//플레이어의 공격력을 가져와서 맞은 적의 체력을 줄게함

            Destroy(this.transform.root.gameObject);
        }
    }

    public void Init(Player owner,Vector3 dir)//객체가 생성될 떄 주인을 알려줌
    {
        _owner = owner;
        _dir= dir;
    }
}
