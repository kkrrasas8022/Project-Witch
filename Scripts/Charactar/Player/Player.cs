using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Walk,
    Run,
    LeftAttack,
    RightAttack,
    Damaged,
    Jump,
    Die
}


public class Player : Charactar
{
    //�÷��̾� ��ü�� �⺻���ۿ� ���õ� Class
    
    public Vector3 dir;                                 //�÷��̾��� �̵�����
    public bool canMove;
    [SerializeField]protected bool canRun;
    public GameObject dust;

    [Header("FSM State")]
    [SerializeField] State _state;                 //�÷��̾��� ���ѻ��¸ӽ� ����

    [Header("JumpStatus")]
    [SerializeField] private float _jumpPower;          //�÷��̾��� ������
    [SerializeField] private bool _isJump;              //�÷��̾ ���������� üũ

    [Header("CoolTime")]
    [SerializeField] protected float _leftTime;         //�޼��� ��ų ��Ÿ��
    [SerializeField] protected float _rightTime;        //�������� ��ų ��Ÿ��
    [SerializeField] protected float _leftcoolTime;     //�޼��� ��ų ��밡�� �ð�
    [SerializeField] protected float _rightcoolTime;    //�������� ��ų ��밡�� �ð�

    [Header("PlayerStatus")]
    [SerializeField] private int _coin;                 //������
    [SerializeField] private int _lv;                   //�÷��̾� ����
    [SerializeField] private float _curExp;             //�÷��̾��� ����ġ
    [SerializeField] private float _maxExp;             //�÷��̾��� ������ �ִ����ġ
    [SerializeField] private float _baseExp;            //�÷��̾��� ������ ����ġ�� ��������� ���̽� ����ġ

    [Header("UI")]
    public Image rightSkill;
    public Image leftSkill;
    public Slider slider_hp;
    public Slider slider_Exp;
    public Text txt_Lv;
    public Text txt_coin;
    public GameObject Game_UI;
    public GameObject LevelUP_UI;
    public List<Item> items;
    public List<RectTransform> CardPoints;
    public Text txt_hp;
    public Text txt_exp;

    [Header("Level_Status")]
    [SerializeField]private int _level_hp;
    [SerializeField]private int _level_power;

    [Header("Card_Status")]
    [SerializeField]private int _hpCard; 
    [SerializeField]private int _powerCard; 
    [SerializeField]private int _defanseCard;
    [SerializeField] private int _critCard;

    [Header("Shop_Status")]
    [SerializeField]private Data _shopData;

    public Player_Sound_Manager Sound_Manager;
    #region ������Ƽ
    public float MaxHp
    {
        get { return _maxHp; }
        set
        {
            _maxHp = value;
            Hp = _maxHp;
            slider_hp.maxValue = _maxHp;
            txt_hp.text = _curHp + "/" + _maxHp;
        }
    }
    public float Hp
    {
        get { return _curHp; }
        set
        {
            _curHp = value;
            slider_hp.value = _curHp;
            txt_hp.text = _curHp + "/" + _maxHp;

            if (_curHp <= 0)
            {
                GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Over_Game();
            }
        }
    }
    public int HpCard
    {
        get { return _hpCard; }
        set
        {
            _hpCard = value;
            MaxHp = _baseHp * (1 + 0.1f * (_lv - 1)) + 30 * HpCard +
                _shopData.health_upgrade_count * 10f;
        }
    }
    public int PowerCard
    {
        get { return _powerCard; }
        set
        {
            _powerCard = value;
            _power = _basePower * (1 + 0.3f * (_lv - 1)) + 3 * _powerCard +
                _shopData.attack_upgrade_count * 3f;
        }
    }
    public int DefanseCard
    {
        get { return _defanseCard; }
        set
        {
            _defanseCard = value;
            _defanse = _defanseCard * 0.01f + 
                _shopData.defence_upgrade_count * 0.02f;
        }
    }
    public int CritCard
    {
        get { return _critCard; }
        set
        {
            _critCard = value;
            _crit = _critCard * 0.05f + 
                _shopData.defence_upgrade_count * 0.02f;
        }
    }
    public float Crit
    {
        get { return _crit; }
        set { _crit = value; }
    }
    public int Coin
    {
        get { return _coin; }
        set
        {
            _coin = value;
            txt_coin.text = _coin.ToString();//������ ������ ����UI�� �������ش�
        }
    }
    public float Exp
    {
        get { return _curExp; }
        set
        {
            _curExp = value;
            if (_curExp >= _maxExp)//����ġ�� �ִ�� �������� �Ѵ�
            {
                LevelUP();
            }
            slider_Exp.value = _curExp;
            txt_exp.text = _curExp + "/" + _maxExp;
        }
    }

    public Data shopData
    {
        get { return _shopData; }
        set 
        { 
            _shopData = value;

            MaxHp = _baseHp * (1 + 0.1f *( _lv-1)) + 30 * HpCard + _shopData.health_upgrade_count * 10f;
            _power = _basePower * (1 + 0.3f * (_lv - 1)) + 3 * _powerCard + _shopData.attack_upgrade_count * 3f;
            _defanse = _defanseCard * 0.01f + _shopData.defence_upgrade_count * 0.02f;
            _crit = _critCard * 0.05f + _shopData.critical_upgrade_count * 0.01f;
        }
    }
    
   
    #endregion

    void Start()
    {
        //_shopData = GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Cur_Data;
        //���̺�� ��ġ�� �ε��Ͽ� ������
        _maxHp = _baseHp + _shopData.health_upgrade_count*10f;
        _curHp = _maxHp;
        _power = _basePower +_shopData.attack_upgrade_count*3f;
        _crit = 0 + _shopData.defence_upgrade_count * 0.02f;
        _defanse = 0 + _shopData.critical_upgrade_count * 0.01f;

        Coin = 0;//���̺�� ������ �ε�
        _lv = 1;
        txt_Lv.text = _lv.ToString();

        //
        slider_Exp.minValue = 0;
        slider_Exp.maxValue = _maxExp;
        slider_Exp.value= _curExp;
        txt_exp.text = _curExp + "/" + _maxExp;

        slider_hp.minValue = 0;
        slider_hp.maxValue = _maxHp;
        slider_hp.value = _curHp;
        txt_hp.text = _curHp + "/" + _maxHp;

        _leftTime = _leftcoolTime;
        _rightTime = _rightcoolTime;
    }//Start

    protected virtual void Update()//�÷��̾�� �÷��̾�� ĳ���Ϳ��� ����ϴ� �߻�Ŭ�����̴�
    {
        if (canMove == false)
            return;
        //��ų ��Ÿ���� ǥ�����ִ� UI
        rightSkill.fillAmount = 1 - (_rightTime / _rightcoolTime);
        leftSkill.fillAmount = 1 - (_leftTime / _leftcoolTime);
        //�� ��ų�� ��Ÿ���� ǥ��
        _leftTime += Time.deltaTime;
        if (_leftTime >= _leftcoolTime)
        {
            _leftTime = _leftcoolTime;
        }
        _rightTime += Time.deltaTime;
        if (_rightTime >= _rightcoolTime)
        {
            _rightTime = _rightcoolTime;
        }
        dust.transform.eulerAngles = Camera.main.transform.eulerAngles;//ķ�� ȸ���ϴ°� �÷��̾ �Ա� ȸ����
        #region FSM ���� ��ȯ
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (dir != Vector3.zero)
        {
            if (canRun == false)
                _state = State.Walk;
            else
                _state = State.Run;
        }
        else
        {
            if (_state == State.Walk)
            {
                _state = State.Idle;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _state = State.Jump;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _state = State.LeftAttack;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _state = State.RightAttack;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canRun == true)
            {
                canRun = false;
            }
            else
            {
                canRun = true;
            }
        }
        #endregion

        StateMachine(_state);
    }//Update


    void LevelUP()//�������� �ϸ� ȣ��Ǵ� �Լ�
    {
        float temp = _curExp - _maxExp;//�ʰ��� ����ġ�� ���
        _lv++;
        //����ɷ�ġ�� �÷��ش�
        //MaxHp += _level_hp;
        Hp= _maxHp;

        //_power += _level_power;

        _maxExp += _baseExp * 0.1f;
        slider_Exp.maxValue = _maxExp;
        txt_Lv.text = _lv.ToString();//UI�� ������ ǥ��
        Open_LevelUp_UI();//
        _curExp = temp;
    }//LevelUP

    void Open_LevelUp_UI()//�������� ������ UI�� ���ִ� �Լ�
    {
        Time.timeScale = 0f;
        Game_UI.SetActive(false);
        LevelUP_UI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        List<Item> checkList=new List<Item>();
        for(int i=0;i<CardPoints.Count;i++)
        {
            Item temp = Instantiate(items[Random.Range(0, items.Count)], LevelUP_UI.transform);
            temp.GetComponent<RectTransform>().position = CardPoints[i].position;
            if(i>0)
            {
                foreach(Item j in checkList)
                {
                    if(j.item_name==temp.item_name)
                    {
                        i--;
                        break;
                    }
                }
            }
            checkList.Add(temp);
        }
    }//Open_LevelUp_UI

    public void Close_LevelUp_UI()//������ UI�� ���� �Լ�
    {
        LevelUP_UI.SetActive(false);
        Game_UI.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }//Close_LevelUp_UI

    protected override void StateMachine(State state)//���ѻ��¸ӽ� 
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Walk:
                Move(this._speed);
                break;
            case State.Run:
                Move(this._speed*2f);
                break;
            case State.LeftAttack:
                LeftAttack();
                break;
            case State.RightAttack:
                RightAttack();
                break;
            case State.Damaged:
                break;
            case State.Jump:
                Jump();
                break;
        }
    }//stateMachine()

#region �÷��̾� ĳ���� �⺻����
    private void Move(float speed)//�÷��̾� ĳ������ �̵�
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        this.transform.Translate(dir * Time.deltaTime * speed, Space.Self);
        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;

        //Sound_Manager.Sound_Move();
    }

    protected virtual void LeftAttack()
    {
        _state = State.Idle;
    }

    protected virtual void RightAttack()
    {
       
        _state = State.Idle;
    }

    void Jump()
    {
        if (_isJump == false)
        { 
            GetComponent<Rigidbody>().AddForce(transform.up * _jumpPower); 
            _isJump = true;
            Sound_Manager.Sound_Jump();
        }
    }
#endregion
    private void OnCollisionEnter(Collision collision)//�÷��̾� ĳ���� ������ �ٴڿ� ������ �������¸� ����
    {
        if(collision.transform.root.tag=="Land")
        {
            _state = State.Idle;
            _isJump=false;
        }

    }//

    //private void OnCollisionStay(Collision collision)//�÷��̾� ĳ���� ������ �ٴڿ� ������ �������¸� ����
    //{
    //    if (collision.gameObject.tag == "Land")
    //    {
    //        _isJump = false;
    //    }
    //}

    public override void Damaged(float damage)
    {
        base.Damaged(damage);
        Hp = _curHp;
    }
}
