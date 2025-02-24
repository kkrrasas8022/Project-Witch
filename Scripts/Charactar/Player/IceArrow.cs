using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : MonoBehaviour
{
    [SerializeField] Player _owner;//����
    [SerializeField] Vector3 _dir;//�������
    void Start()
    {
        Destroy(this.transform.root.gameObject,10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _dir * 15f * Time.deltaTime;//�߻��Ͽ� �̵�
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            int i = Random.Range(0, 100);
            if (i <= _owner.Crit * 100)
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power * 2);
            }//ġ��Ÿ
            else
            {
                other.gameObject.GetComponent<Charactar>().Damaged(_owner.Power);
            }//�÷��̾��� ���ݷ��� �����ͼ� ���� ���� ü���� �ٰ���
            StartCoroutine(SlowEnemy(other,
                ((Witch)_owner)._slow_Power,
                ((Witch)_owner)._slow_Time));
            //�� ��ü�� �������� �������� ��
            Destroy(this.transform.root.gameObject,2.1f);
        }
    }

    IEnumerator SlowEnemy(Collider other, float slow_power,float slow_time)//���� �������� ����� �ڷ�ƾ
    {
        float temp = other.gameObject.GetComponent<Charactar>().Speed;//���� ���� �ӵ��� �޾�������
        other.gameObject.GetComponent<Charactar>().Speed = temp * (1 - ((Witch)_owner)._slow_Power);//���� �ӵ��� �����ӵ��� ������ �ӵ��� ����Ͽ� �Ҵ���
        yield return new WaitForSeconds(slow_time);//�������� �ð����� ���� ������
        if(other!=null)
        {
            other.gameObject.GetComponent<Charactar>().Speed = temp;//�������� �ð��� ������ ���� �ӵ��� ���ƿ�
        }
    }

    public void Init(Player owner,Vector3 dir)//������ ������ �� ��ü�� ������ �˷���
    {
        _owner = owner;
        _dir= dir;
    }
}
