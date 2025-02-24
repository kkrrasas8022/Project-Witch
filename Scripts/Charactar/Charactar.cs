using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Charactar : MonoBehaviour//추상클래스 구현
{
    [Header("Basic Status")]
    [SerializeField] protected float _curHp;    //캐릭터 객체의 현재체력
    [SerializeField] protected float _maxHp;    //캐릭터 객체의 최대체력
    [SerializeField] protected float _power;    //캐릭터 객체의 공격력
    [SerializeField] protected float _defanse;  //캐릭터 객체의 내구력
    [SerializeField] protected float _crit;     //캐릭터 객체의 치명타율
    [SerializeField] protected float _speed;    //캐릭터 객체의 이동속도
    [SerializeField] protected float _baseHp;
    [SerializeField] protected float _basePower;

    protected abstract void StateMachine(State state);//FSM 가상함수

    public float Power      //공격력에 접근하는 프로퍼티
    {
        get { return _power; }
        set { _power = value; }
    }
    public float Speed      //속도에 접근하는 프로퍼티
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public virtual void Damaged(float damage)   //캐릭터 객체가 데미지를 입었을때 발동하는 함수
    {
        _curHp -= damage * (1 - this._defanse);//데미지를 내구력으로 감소시킨 데미지로 체력을 감소시킨다
    }//Damaged

}//Charactar