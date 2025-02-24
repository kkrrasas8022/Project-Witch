using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] Player _owner;//����(�÷��̾�)
    [SerializeField] Vector3 _dir;//�������
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
            }//ġ��Ÿ
            else
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power);
            }//�÷��̾��� ���ݷ��� �����ͼ� ���� ���� ü���� �ٰ���

            Destroy(this.transform.root.gameObject);
        }
    }

    public void Init(Player owner,Vector3 dir)//��ü�� ������ �� ������ �˷���
    {
        _owner = owner;
        _dir= dir;
    }
}
