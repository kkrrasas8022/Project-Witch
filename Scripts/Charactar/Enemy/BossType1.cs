using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossType1 : Monster
{
    enum AttackType
    {
        Melee,
        Range,
        Special,
    }

    public GameObject attHitBox;
    public Transform rockSpanwPoint;
    public GameObject rockPref;
    public GameObject shockWavePref;
    public Transform shockWavePoint;
    MonsterSoundMgr soundMgr;

    [SerializeField]
    float actionDelay = 2.5f; //공격 행동 공통 대기시간
    float actionTime; //공격 행동 시간 흐름
    [SerializeField]
    float meleeDelay = 4f; //근접공격 대기시간
    float meleeTime; //근접공격 시간 흐름
    [SerializeField]
    float throwDelay = 7f; //원거리공격 대기시간
    float throwTime; //원거리공격 시간 흐름
    [SerializeField]
    float stampDelay = 11f; //충격파공격 대기시간
    float stampTime; //충격파 시간 흐름

    bool isThrowing;

    AttackType attType;

    protected override void Start()
    {
        base.Start();
        soundMgr = GetComponentInChildren<MonsterSoundMgr>();
        actionTime = actionDelay;
        meleeTime = meleeDelay;
        throwTime = throwDelay;
        stampTime = stampDelay;
    }

    protected override void Update()
    {
        base.Update();

        //공격 공통 대기시간
        if (actionTime > 0)
        {
            actionTime -= Time.deltaTime;
        }
        //던지기 대기시간
        if (throwTime > 0)
        {
            throwTime -= Time.deltaTime;
        }
        //충격파 대기시간
        if (stampTime > 0)
        {
            stampTime -= Time.deltaTime;
        }
    }

    //대기 상태
    protected override void Idle()
    {
        if(isThrowing)
        {
            //던질 때는 플레이어 바라보기
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        }
        //공격 후 2초 뒤부터 플레이어를 바라보게 함
        if (actionTime < actionDelay - 2)
        {
            isThrowing = false;
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        }

        if (actionTime <= 0)
        {
            monsterState = State.Run;
        }
    }

    //이동
    protected override void Move()
    {
        anim.SetBool("IsMove", true);
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        transform.position += (dir * Time.deltaTime * _speed);

        if (CalcPlayerDist() < attackRange)
        {
            monsterState = State.RightAttack;
            attType = AttackType.Melee;
        }

        if(throwTime <= 0 && actionTime <= 0)
        {
            monsterState = State.RightAttack;
            attType = AttackType.Range;
        }

        if(stampTime <= 0 && actionTime <= 0)
        {
            monsterState = State.RightAttack;
            attType = AttackType.Special;
        }
    }

    //공격
    protected override void Attack()
    {
        anim.SetBool("IsMove", false);

        //공격 쿨타임이 아니면 공격하기
        if (actionTime <= 0)
        {
            //근접공격
            //공격 애니메이션에 이벤트를 넣어 히트박스가 켜지도록 함
            switch(attType)
            {
                //근접 공격
                case AttackType.Melee:
                    anim.SetTrigger("Punch");

                    break;
                //원거리 공격
                case AttackType.Range:
                    //애니메이션 이벤트로 돌생성 및 발사
                    anim.SetTrigger("Throw");
                    isThrowing = true;

                    throwTime = throwDelay;
                    break;
                //충격파 공격
                case AttackType.Special:
                    anim.SetTrigger("Stamp");

                    stampTime = stampDelay;
                    break;
            }
            actionTime = actionDelay;
        }

        monsterState = State.Idle;
    }

    public GameObject CreateRock()
    {
        GameObject rock = Instantiate(rockPref, rockSpanwPoint.position, Quaternion.identity);
        rock.transform.SetParent(rockSpanwPoint);
        return rock;
    }

    public Vector3 GetPlayerPos()
    {
        Vector3 correctedPlayerPos = player.transform.position;
        correctedPlayerPos.y += 1;
        return correctedPlayerPos;
    }

    public void Stamp()
    {
        for(int i = 0; i < 4; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 90 * i, 0);
            GameObject sw = Instantiate(shockWavePref, shockWavePoint.position, rot);
            sw.GetComponent<ProjectileCtrl>().power = _power;
        }
    }

    //사망
    protected override void Die()
    {
        base.Die();
    }

    protected override IEnumerator DieAction()
    {
        soundMgr.PlayDeathSound();
        yield return StartCoroutine(base.DieAction());
    }

    //애니메이션 이벤트로 활성화된 히트박스의 충돌체크
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<Charactar>().Damaged(_power);
            other.gameObject.GetComponentInParent<Charactar>().Damaged(_power);
        }
    }
}
