using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MonType
{
    Melee,
    Range,
    Boss,
}

public enum MonState
{
    Idle,
    Move,
    Attack,
    Die,
}

public class Monster : Charactar
{
    [SerializeField] protected float detectRange;

    [Header("attack values")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDelay;
    protected float elapsedTime;

    [SerializeField] protected State monsterState;
    protected bool isDie;

    [SerializeField] protected int level;
    bool isLevelTime;

    [SerializeField] protected float dropExp;
    [SerializeField] protected int reward;
    public GameObject coinPref;

    protected Animator anim;
    protected GameObject player;
    Game_Manager gameMgr;

    protected Canvas monCanvas;
    public Slider hpSlider;

    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        gameMgr = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        anim = GetComponentInChildren<Animator>();
        monCanvas = GetComponentInChildren<Canvas>();
        monCanvas.gameObject.SetActive(false);

        InitStatus();
    }

    protected virtual void Update()
    {
        Leveling();

        StateMachine(monsterState);
        
        SizingCanvas();
    }

    void SizingCanvas()
    {
        //UI�� �׻� ���� ũ��� ���̰� ���ֱ�
        Vector3 monPos = transform.position;
        monPos.y = 0;
        Vector3 camPos = Camera.main.transform.position;
        camPos.y = 0;
        float distance = Vector3.Distance(monPos, camPos);
        distance += 3f;
        if (monCanvas != null)
        {
            monCanvas.transform.localScale = Vector3.one * (distance / 2000f); // 10�� ������ ������ ���� ����
        }
    }

    public override void Damaged(float value)
    {
        //�ǰݽ� UI�� ��Ȱ��ȭ�����̸�
        if (monCanvas.gameObject.activeSelf == false)
        {
            //UIȰ��ȭ
            ShowUI();
        }

        _curHp -= value;
        RefreshUI();

        //�������
        if(_curHp <= 0 )
        {
            monsterState = State.Die;
        }
    }

    //���� UI ���ΰ�ħ�Լ�
    public void RefreshUI()
    {
        hpSlider.value = _curHp / _maxHp;
    }

    void ShowUI()
    {
        monCanvas.gameObject.SetActive(true);
    }

    void HideUI()
    {
        monCanvas.gameObject.SetActive(false);
    }

    public virtual void InitStatus()
    {
        //���ӷ����� ��ġ��Ű��
        level = GameObject.Find("Game_Manager").GetComponent<Game_Manager>().gameLevel;

        //�ִ� ü���� ������ 20%�� ����
        _maxHp = _baseHp + (_baseHp * 0.2f) * level;

        //���ݷ��� ������ 10%�� ����
        _power = _basePower + (_basePower * 0.1f) * level;

        //������ ü���� �ִ�ü������ ����
        _curHp = _maxHp;

        //���� ���´� ������
        monsterState = State.Idle;

        //���� Ÿ�� üũ�� 0(�ٷ� ���� ����)
        elapsedTime = 0;

        RefreshUI();
    }

    protected void Leveling()
    {
        //���� ������ ���ӷ������� ������ ������
        if (GameObject.Find("Game_Manager").GetComponent<Game_Manager>().gameLevel > level)
        {
            level++;
            LevelUp();
        }
    }

    protected void LevelUp()
    {
        //�ִ� ü�� 20%����
        _maxHp += _baseHp * 0.2f;

        //�ִ� ü�� ����ġ ��ŭ ����ü�µ� ����
        float increasedHp = _baseHp * 0.2f;
        //���� ü���� �ִ�ü���� �Ѿ�� �ִ�ü������ ����
        if(increasedHp + _curHp > _maxHp)
        {
            _curHp = _maxHp;
        }

        //���ݷ� 10%����
        _power += _basePower * 0.1f;

        RefreshUI();
    }

    //�÷��̾���� �Ÿ� ��� �Լ�
    protected float CalcPlayerDist()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist;
    }

    protected virtual void Idle()
    {
        //�÷��̾ Ž�� �Ÿ����� ������ �̵����·� ����
        if (CalcPlayerDist() < detectRange)
        {
            monsterState = State.Run;
        }
    }

    protected virtual void Move()
    {
        anim.SetBool("IsMove", true);
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        transform.forward = Vector3.Lerp(transform.forward, dir, 20 * Time.deltaTime);
        transform.position += (dir * Time.deltaTime * _speed);

        //�÷��̾ ���� �Ÿ� ���� ������ ���� ���·� ����
        if (CalcPlayerDist() < attackRange)
        {
            monsterState = State.RightAttack;
        }

        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
        }
    }

    protected virtual void Attack()
    {
    }

    protected virtual void Die()
    {
        if (!isDie)
        {
            StartCoroutine(DieAction());
        }
    }

    protected virtual IEnumerator DieAction()
    {
        isDie = true;
        //��� �ִϸ��̼� ���
        if(anim != null)
        {
            anim.SetTrigger("Die");
        }
        //�ٸ� ���͵鿡�� ��ü�� ���ص��� �ʵ���
        GetComponent<Rigidbody>().isKinematic = true;
        //������ �����Ӱ� �ݸ����� ��Ȱ��ȭ
        GetComponent<Collider>().enabled = false;
        //UI ��Ȱ��ȭ
        HideUI();

        //����ġ ����
        player.GetComponent<Player>().Exp += dropExp;
        //���� ���
        if (coinPref != null)
        {
            GameObject coin = Instantiate(coinPref);
            coin.transform.position = transform.position;
            coin.GetComponent<CoinCtrl>().amount = reward;
        }

        //2�� ��� �� ������Ʈ ����
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

        if(gameMgr.Stage == Game_Stage.Stage_Boss)
        {
            gameMgr.Clear_Game();
        }
    }

    protected override void StateMachine(State state)//���ѻ��¸ӽ� 
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Run:
                Move();
                break;
            case State.RightAttack:
                Attack();
                break;
            case State.Die:
                Die();
                break;
        }
    }//stateMachine()
}
