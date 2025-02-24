using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;

public enum Game_Stage { Stage_Ready, Stage_01, Stage_01_Middle_Boss, Stage_02, Stage_02_Middle_Boss, Stage_Boss, Clear, Over }

public class Game_Manager : MonoBehaviour
{
    public Transform[] Spawn_Point;     // 몬스터 스폰포인트
    public GameObject[] Enemy_Prefab;   // 몬스터 객체
    public GameObject Next_Stage_Point; // 다음 스테이지 포인트
    public GameObject Fade_Scene;       // 화면 어둡게 하기
    public GameObject Player;           // 임시 플레이어
    public Text[] Upgrade_Count;        // 강화 카운트
    public Text Goods;                  // 재화
    public Text Debug_Stage;
    GameObject Enemies_Pool;            // 몬스터 
    GameObject Shap_Npc;                // 임시 던전 상인
    [SerializeField]Data m_save;
    [SerializeField]Data m_load;
    public Data Cur_Data;
    public bool isShopOpen;


    public Game_Stage Stage;
    Vector3 Origin_Point;           // 플레이어 시작 포인트
    Vector3 Middle_Boss_Point;      // 중간보스 시작 포인트   
    Vector3 Boss_Point;             // 보스 클리어 포인트

    public bool is_clear;   // 스테이지를 클리어 했는가
    bool is_maked;          // 몬스터를 생성할 것인가
    int stage_count;

    [Header("Boss")]
    public Transform Boss_Spawn_Point;
    public GameObject[] Boss_Monster;

    // ===================================================== 통합 코드
    #region Plus_Code
    public float gameTime = 0;
    public float levelTime = 10f;
    public float flowTime = 0;
    public int gameLevel = 0;

    void CountTime()
    {
        gameTime += Time.deltaTime;
    }

    void Levelizing()
    {
        if (flowTime <= 0)
        {
            gameLevel++;
            flowTime = levelTime;
        }

        flowTime -= Time.deltaTime;
    }
    #endregion
    // ===================================================== 여기까지

    private void Awake()
    {
        Init_Data();


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Fade_Scene.gameObject.SetActive(true);

        Invoke("Game_Start", 1f);
        Load_Data();
        CountTime();
        Levelizing();

        
    }

    void Start()
    {
        Player.GetComponent<Player>().txt_coin.text = Cur_Data.m_money.ToString();
    }

    void Update()
    {
        //Debug_Stage.text = Stage.ToString();

        Game_Process();
    }

    #region Game_Start_Data
    void Init_Data()
    {
        Player = GameObject.Find("Player");
        Enemies_Pool = GameObject.Find("Enemies");
        Shap_Npc = GameObject.Find("Shap_NPC");
        Origin_Point = Player.transform.position;
        Middle_Boss_Point = new Vector3(25f, 5.5f, 129.5f);
        Boss_Point = new Vector3(35f, 5.5f, 191f);
        Stage = Game_Stage.Stage_Ready;
        is_maked = false;
        m_save = new Data();
        Cur_Data = new Data();
        for (int i = 0; i < 4; i++)
        {
            Upgrade_Count[i].text = "0 / 10";
        }
    }

    void Game_Start()
    {
        Stage = Game_Stage.Stage_01;
        is_maked = true;
        stage_count = 1;
    }

    public void Fade_In()
    {
        Fade_Scene.gameObject.SetActive(true);
    }

    void PlayerReset()//플레이어 위치와 회전값을초기화하고 움직임을 통제하는 함수
    {
        Reset_Player_Point();
        Player.transform.rotation= Quaternion.identity;
        Player.GetComponent<Player>().canMove = false;
        Player.GetComponent<Rigidbody>().useGravity = false;
        Camera.main.transform.eulerAngles = Vector3.zero;
        Camera.main.GetComponent<Cam_Rotate>().canMoveCam = false;
    }
    void PlayerCanMove()//플레이어의 움직임을 통제하던걸 풀어주는 함수
    {
        Player.GetComponent<Player>().canMove = true;
        Player.GetComponent<Rigidbody>().useGravity = true;
        Camera.main.GetComponent<Cam_Rotate>().canMoveCam = true;
    }

    public void Fade_Out()
    {
        Fade_Scene.gameObject.SetActive(false);
    }
    #endregion

    #region Enemy_Spawn_and_Reset
    IEnumerator Spawn_Enemy()
    {
        is_maked = false;

        yield return new WaitForSeconds(3f);

        int i = Random.Range(0, Spawn_Point.Length);
        int j = Random.Range(0, Enemy_Prefab.Length);

        GameObject temp = Instantiate(Enemy_Prefab[j], Spawn_Point[i].position, Spawn_Point[i].rotation);
        temp.transform.parent = Enemies_Pool.transform;
        is_maked = true;
    }

    void Reset_Monster()
    {
        foreach (Transform child in Enemies_Pool.transform)
        {
            Destroy(child.gameObject);
        }

        Next_Stage_Point.transform.position = Middle_Boss_Point;
        Shap_Npc.gameObject.SetActive(false);
    }

    public void Reset_Player_Point()
    {
        Player.transform.position = Origin_Point;
    }
    #endregion

    IEnumerator ResetCoru()
    {
        PlayerReset();
        yield return new WaitForSeconds(0.5f);
        PlayerCanMove();
    }

    #region Game_Stage_Switch
    void Game_Process()
    {
        switch (Stage)
        {
            case Game_Stage.Stage_01:
                if (stage_count == 1)
                {
                    stage_count++;
                    Reset_Monster();
                    StartCoroutine(ResetCoru());
                    Invoke("Fade_Out", 1f);
                }

                if (is_maked == true)
                {
                    StartCoroutine(Spawn_Enemy());
                }
                break;
            case Game_Stage.Stage_01_Middle_Boss:
                is_maked = false;

                if (stage_count == 2)
                {
                    stage_count++;
                    Reset_Monster();

                    Next_Stage_Point.transform.position = Boss_Point;
                    Next_Stage_Point.gameObject.SetActive(false);
                    Instantiate(Boss_Monster[0], Boss_Spawn_Point.position, Boss_Spawn_Point.rotation);
                }
                break;
            case Game_Stage.Stage_02:
                if (stage_count == 3)
                {
                    Next_Stage_Point.GetComponent<CapsuleCollider>().enabled = true;
                    is_maked = true;

                    stage_count++;
                    Reset_Monster();
                    StartCoroutine(ResetCoru());
                    Invoke("Fade_Out", 1f);
                }
          
                if (is_maked == true)
                {
                    StartCoroutine(Spawn_Enemy());
                }
                break;
            case Game_Stage.Stage_02_Middle_Boss:
                is_maked = false;
          
                if (stage_count == 4)
                {
                    stage_count++;
                    Reset_Monster();

                    Next_Stage_Point.transform.position = Boss_Point;
                    Next_Stage_Point.gameObject.SetActive(false);
                    Instantiate(Boss_Monster[1], Boss_Spawn_Point.position, Boss_Spawn_Point.rotation);
                }
                break;
            case Game_Stage.Stage_Boss:
                if (stage_count == 5)
                {
                    Next_Stage_Point.GetComponent<CapsuleCollider>().enabled = true;
                    is_maked = false;

                    stage_count++;
                    Reset_Monster();
                    StartCoroutine(ResetCoru());
                    Instantiate(Boss_Monster[2], Boss_Spawn_Point.position, Boss_Spawn_Point.rotation);
                    Invoke("Fade_Out", 1f);
                }
                break;
            case Game_Stage.Clear:
                Clear_Game();
                break;
            case Game_Stage.Over:
                Over_Game();
                break;
        }
    }


    public void Clear_Boss()
    {
        is_clear = true;

        Next_Stage_Point.gameObject.SetActive(true);
        Next_Stage_Point.GetComponent<CapsuleCollider>().enabled = true;
        Shap_Npc.gameObject.SetActive(true);

        Save_Cur_Data();
        Save_Data();
    }

    public void Clear_Game()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("04_ClearScene");
    }

    public void Over_Game()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("05_OverScene");
    }


    #endregion

    #region Pause
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Button_Yes()
    {
        Time.timeScale = 1f;
    }

    public void Button_No()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    #endregion

    #region Save_and_Load
    public void Save_Data()
    {
        FileStream Fs = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.Create);
        BinaryFormatter Bf = new BinaryFormatter();
        Bf.Serialize(Fs, m_save);

        Fs.Close();
    }

    public void Load_Data()
    {
        FileStream Fs = null;

        try
        {
            Fs = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.Open);
        }
        catch (System.Exception) { }

        if (Fs != null)
        {
            BinaryFormatter Bf = new BinaryFormatter();
            m_load = (Data)Bf.Deserialize(Fs);

            Fs.Close();

            

            Load_Cur_Data();
        }
    }
    //플레이어 넘겨주는 함수
    //...


    public void Save_Cur_Data()
    {
        m_save=Cur_Data;

        //m_save.m_money = Cur_Data.m_money;
        //
        //m_save.health_upgrade_count = Cur_Data.health_upgrade_count;
        //m_save.attack_upgrade_count = Cur_Data.attack_upgrade_count;
        //m_save.defence_upgrade_count = Cur_Data.defence_upgrade_count;
        //m_save.critical_upgrade_count = Cur_Data.critical_upgrade_count;
    }

    public void Load_Cur_Data()
    {
        ChangeCurData(m_load);
    }

    public void ChangeCurData(Data data)
    {
        Cur_Data = data;
        Player.GetComponent<Player>().shopData = Cur_Data;
    }

    public void Show_Data()
    {
        Goods.text = Cur_Data.m_money.ToString();

        Upgrade_Count[0].text = Cur_Data.health_upgrade_count + " / 10";
        Upgrade_Count[1].text = Cur_Data.attack_upgrade_count + " / 10";
        Upgrade_Count[2].text = Cur_Data.defence_upgrade_count + " / 10";
        Upgrade_Count[3].text = Cur_Data.critical_upgrade_count + " / 10";
    }

    public void Count_Money(int _val)
    {
        Cur_Data.m_money += _val;
        Show_Money();
    }
    public void Show_Money()
    {
        Player.GetComponent<Player>().txt_coin.text = Cur_Data.m_money.ToString();
    }
    public void Buy_Ability(int _val)
    {
        Cur_Data.m_money -= _val;
        if (Cur_Data.m_money < 0)
            Cur_Data.m_money = 0;
    }
    #endregion

    public void Upgrade()
    {
        GameObject Click_Button = EventSystem.current.currentSelectedGameObject;
        Data tempData = Cur_Data;
        bool canPay = false;
        switch (Click_Button.name)
        {
            case "B_Health":
                if (tempData.health_upgrade_count < 10&&tempData.m_money>0)
                {
                    tempData.health_upgrade_count++;
                    canPay = true;
                }
                else
                {
                    print("You Can't Upgrade");
                    canPay = false;
                }
                break;
            case "B_Attack":
                if (tempData.attack_upgrade_count < 10 && tempData.m_money > 0)
                {
                    tempData.attack_upgrade_count++;
                    canPay = true;
                }
                else
                {
                    print("You Can't Upgrade");
                    canPay = false;
                }
                break;
            case "B_Defence":
                if (tempData.defence_upgrade_count < 10 && tempData.m_money >0)
                {
                    tempData.defence_upgrade_count++;
                    canPay = true;
                }
                else
                {
                    print("You Can't Upgrade");
                    canPay = false;
                }
                break;
            case "B_Critical":
                if (tempData.critical_upgrade_count < 10 && tempData.m_money > 0)
                {
                    tempData.critical_upgrade_count++;
                    canPay = true;
                }
                else
                {
                    print("You Can't Upgrade");
                    canPay = false;
                }
                break;
            default:
                break;
        }
        if (canPay)
        { 
            Buy_Ability(10);
        }
        Show_Data();
        Show_Money();
        ChangeCurData(tempData);
        Save_Cur_Data();
    }
}

// Day 01
// 랜덤 몬스터 랜덤 스폰 Clear
// 스테이지 클리어 시 Player 처음 위치로 이동 Clear
// 스테이지 클리어 시 맵 몬스터 초기화 Clear
// 일시정지 Clear
// 던전 상점 Clear

// Day 02
// 재화 저장 Clear