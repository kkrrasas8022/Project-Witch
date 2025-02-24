using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Charactar : MonoBehaviour//�߻�Ŭ���� ����
{
    [Header("Basic Status")]
    [SerializeField] protected float _curHp;    //ĳ���� ��ü�� ����ü��
    [SerializeField] protected float _maxHp;    //ĳ���� ��ü�� �ִ�ü��
    [SerializeField] protected float _power;    //ĳ���� ��ü�� ���ݷ�
    [SerializeField] protected float _defanse;  //ĳ���� ��ü�� ������
    [SerializeField] protected float _crit;     //ĳ���� ��ü�� ġ��Ÿ��
    [SerializeField] protected float _speed;    //ĳ���� ��ü�� �̵��ӵ�
    [SerializeField] protected float _baseHp;
    [SerializeField] protected float _basePower;

    protected abstract void StateMachine(State state);//FSM �����Լ�

    public float Power      //���ݷ¿� �����ϴ� ������Ƽ
    {
        get { return _power; }
        set { _power = value; }
    }
    public float Speed      //�ӵ��� �����ϴ� ������Ƽ
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public virtual void Damaged(float damage)   //ĳ���� ��ü�� �������� �Ծ����� �ߵ��ϴ� �Լ�
    {
        _curHp -= damage * (1 - this._defanse);//�������� ���������� ���ҽ�Ų �������� ü���� ���ҽ�Ų��
    }//Damaged

}//Charactar