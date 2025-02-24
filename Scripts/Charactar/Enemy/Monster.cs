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
        //UI가 항상 일정 크기로 보이게 해주기
        Vector3 monPos = transform.position;
        monPos.y = 0;
        Vector3 camPos = Camera.main.transform.position;
        camPos.y = 0;
        float distance = Vector3.Distance(monPos, camPos);
        distance += 3f;
        if (monCanvas != null)
        {
            monCanvas.transform.localScale = Vector3.one * (distance / 2000f); // 10은 임의의 값으로 조절 가능
        }
    }

    public override void Damaged(float value)
    {
        //피격시 UI가 비활성화상태이면
        if (monCanvas.gameObject.activeSelf == false)
        {
            //UI활성화
            ShowUI();
        }

        _curHp -= value;
        RefreshUI();

        //사망판정
        if(_curHp <= 0 )
        {
            monsterState = State.Die;
        }
    }

    //몬스터 UI 새로고침함수
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
        //게임레벨과 일치시키기
        level = GameObject.Find("Game_Manager").GetComponent<Game_Manager>().gameLevel;

        //최대 체력은 레벨당 20%씩 증가
        _maxHp = _baseHp + (_baseHp * 0.2f) * level;

        //공격력은 레벨당 10%씩 증가
        _power = _basePower + (_basePower * 0.1f) * level;

        //몬스터의 체력은 최대체력으로 시작
        _curHp = _maxHp;

        //몬스터 상태는 대기상태
        monsterState = State.Idle;

        //공격 타임 체크는 0(바로 공격 가능)
        elapsedTime = 0;

        RefreshUI();
    }

    protected void Leveling()
    {
        //몬스터 레벨이 게임레벨보다 낮으면 레벨업
        if (GameObject.Find("Game_Manager").GetComponent<Game_Manager>().gameLevel > level)
        {
            level++;
            LevelUp();
        }
    }

    protected void LevelUp()
    {
        //최대 체력 20%증가
        _maxHp += _baseHp * 0.2f;

        //최대 체력 증가치 만큼 현재체력도 증가
        float increasedHp = _baseHp * 0.2f;
        //현재 체력이 최대체력을 넘어가면 최대체력으로 제한
        if(increasedHp + _curHp > _maxHp)
        {
            _curHp = _maxHp;
        }

        //공격력 10%증가
        _power += _basePower * 0.1f;

        RefreshUI();
    }

    //플레이어와의 거리 계산 함수
    protected float CalcPlayerDist()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist;
    }

    protected virtual void Idle()
    {
        //플레이어가 탐지 거리내에 들어오면 이동상태로 변경
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

        //플레이어가 공격 거리 내에 들어오면 공격 상태로 변경
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
        //사망 애니메이션 재생
        if(anim != null)
        {
            anim.SetTrigger("Die");
        }
        //다른 몬스터들에게 시체가 방해되지 않도록
        GetComponent<Rigidbody>().isKinematic = true;
        //물리적 움직임과 콜리더를 비활성화
        GetComponent<Collider>().enabled = false;
        //UI 비활성화
        HideUI();

        //경험치 제공
        player.GetComponent<Player>().Exp += dropExp;
        //코인 드랍
        if (coinPref != null)
        {
            GameObject coin = Instantiate(coinPref);
            coin.transform.position = transform.position;
            coin.GetComponent<CoinCtrl>().amount = reward;
        }

        //2초 대기 후 오브젝트 제거
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

        if(gameMgr.Stage == Game_Stage.Stage_Boss)
        {
            gameMgr.Clear_Game();
        }
    }

    protected override void StateMachine(State state)//유한상태머신 
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
