using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonType2 : Monster
{
    public GameObject projectilePref;
    public Transform shotPoint;
    MonsterSoundMgr soundMgr;

    protected override void Start()
    {
        base.Start();
        soundMgr = GetComponentInChildren<MonsterSoundMgr>();
    }

    protected override void Update()
    {
        base .Update();
    }

    //��� ����
    protected override void Idle()
    {
        base.Idle();
    }

    //�̵�
    protected override void Move()
    {
        base.Move();
    }

    //����
    protected override void Attack()
    {
        //���� ��Ÿ���� �ƴϸ� �����ϱ�
        if (elapsedTime <= 0)
        {
            anim.SetBool("IsMove", false);
            //���� �ִϸ��̼ǿ� �̺�Ʈ�� �־� ��Ʈ�ڽ��� �������� ��
            anim.SetTrigger("Attack");
            //���� ���� ���
            soundMgr.PlayAttackSound();

            //����ü �߻�
            GameObject proj = Instantiate(projectilePref);
            proj.transform.position = shotPoint.position;
            proj.GetComponent<ProjectileCtrl>().power = _power;
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);

            elapsedTime = attackDelay;
        }

        //�����ϴ� ������ ������ �ֵ��� ����
        //���� �� 2�� �ں��� �÷��̾ �ٶ󺸰� ��
        if (elapsedTime < attackDelay - 2)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        }

        //���� �� 2�� �ں��� �÷��̾ �Ѿư����� ��
        if (CalcPlayerDist() > attackRange && elapsedTime < attackDelay - 2)
        {
            monsterState = State.Run;
        }

        elapsedTime -= Time.deltaTime;
    }

    //���
    protected override void Die()
    {
        base.Die();
    }

    protected override IEnumerator DieAction()
    {
        //��� ���� ���
        soundMgr.PlayDeathSound();
        yield return StartCoroutine(base.DieAction());
    }
}
