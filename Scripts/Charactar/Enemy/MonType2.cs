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

    //대기 상태
    protected override void Idle()
    {
        base.Idle();
    }

    //이동
    protected override void Move()
    {
        base.Move();
    }

    //공격
    protected override void Attack()
    {
        //공격 쿨타임이 아니면 공격하기
        if (elapsedTime <= 0)
        {
            anim.SetBool("IsMove", false);
            //공격 애니메이션에 이벤트를 넣어 히트박스가 켜지도록 함
            anim.SetTrigger("Attack");
            //공격 사운드 출력
            soundMgr.PlayAttackSound();

            //투사체 발사
            GameObject proj = Instantiate(projectilePref);
            proj.transform.position = shotPoint.position;
            proj.GetComponent<ProjectileCtrl>().power = _power;
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * 800f);

            elapsedTime = attackDelay;
        }

        //공격하는 동안은 가만히 있도록 해줌
        //공격 후 2초 뒤부터 플레이어를 바라보게 함
        if (elapsedTime < attackDelay - 2)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        }

        //공격 후 2초 뒤부터 플레이어를 쫓아가도록 함
        if (CalcPlayerDist() > attackRange && elapsedTime < attackDelay - 2)
        {
            monsterState = State.Run;
        }

        elapsedTime -= Time.deltaTime;
    }

    //사망
    protected override void Die()
    {
        base.Die();
    }

    protected override IEnumerator DieAction()
    {
        //사망 사운드 출력
        soundMgr.PlayDeathSound();
        yield return StartCoroutine(base.DieAction());
    }
}
