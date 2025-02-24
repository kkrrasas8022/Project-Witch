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
    float actionDelay = 2.5f; //���� �ൿ ���� ���ð�
    float actionTime; //���� �ൿ �ð� �帧
    [SerializeField]
    float meleeDelay = 4f; //�������� ���ð�
    float meleeTime; //�������� �ð� �帧
    [SerializeField]
    float throwDelay = 7f; //���Ÿ����� ���ð�
    float throwTime; //���Ÿ����� �ð� �帧
    [SerializeField]
    float stampDelay = 11f; //����İ��� ���ð�
    float stampTime; //����� �ð� �帧

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

        //���� ���� ���ð�
        if (actionTime > 0)
        {
            actionTime -= Time.deltaTime;
        }
        //������ ���ð�
        if (throwTime > 0)
        {
            throwTime -= Time.deltaTime;
        }
        //����� ���ð�
        if (stampTime > 0)
        {
            stampTime -= Time.deltaTime;
        }
    }

    //��� ����
    protected override void Idle()
    {
        if(isThrowing)
        {
            //���� ���� �÷��̾� �ٶ󺸱�
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        }
        //���� �� 2�� �ں��� �÷��̾ �ٶ󺸰� ��
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

    //�̵�
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

    //����
    protected override void Attack()
    {
        anim.SetBool("IsMove", false);

        //���� ��Ÿ���� �ƴϸ� �����ϱ�
        if (actionTime <= 0)
        {
            //��������
            //���� �ִϸ��̼ǿ� �̺�Ʈ�� �־� ��Ʈ�ڽ��� �������� ��
            switch(attType)
            {
                //���� ����
                case AttackType.Melee:
                    anim.SetTrigger("Punch");

                    break;
                //���Ÿ� ����
                case AttackType.Range:
                    //�ִϸ��̼� �̺�Ʈ�� ������ �� �߻�
                    anim.SetTrigger("Throw");
                    isThrowing = true;

                    throwTime = throwDelay;
                    break;
                //����� ����
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

    //���
    protected override void Die()
    {
        base.Die();
    }

    protected override IEnumerator DieAction()
    {
        soundMgr.PlayDeathSound();
        yield return StartCoroutine(base.DieAction());
    }

    //�ִϸ��̼� �̺�Ʈ�� Ȱ��ȭ�� ��Ʈ�ڽ��� �浹üũ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<Charactar>().Damaged(_power);
            other.gameObject.GetComponentInParent<Charactar>().Damaged(_power);
        }
    }
}
