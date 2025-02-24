using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Next_Stage : MonoBehaviour
{
    Game_Manager Manager;
    public GameObject bossRoomWall;
    public GameObject bridgeWall;

    private void Awake()
    {
        Init_Data();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void Init_Data()
    {
        Manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
    }

    void HideWalls()
    {
        bossRoomWall.SetActive(false);
        bridgeWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (Manager.Stage)
            {
                case Game_Stage.Stage_01:
                    this.GetComponent<Collider>().enabled = false;
                    Manager.Stage = Game_Stage.Stage_01_Middle_Boss;
                    break;
                case Game_Stage.Stage_01_Middle_Boss:
                    if (Manager.is_clear == true)
                    {
                        this.GetComponent<Collider>().enabled = false;
                        Manager.Fade_In();
                        Manager.Stage = Game_Stage.Stage_02;
                        Manager.is_clear = false;
                        HideWalls();
                    }
                    break;
                case Game_Stage.Stage_02:
                    this.GetComponent<Collider>().enabled = false;
                    Manager.Stage = Game_Stage.Stage_02_Middle_Boss;
                    break;
                case Game_Stage.Stage_02_Middle_Boss:
                    if (Manager.is_clear == true)
                    {
                        this.GetComponent<Collider>().enabled = false;
                        Manager.Fade_In();
                        Manager.Stage = Game_Stage.Stage_Boss;
                        Manager.is_clear = false;
                        HideWalls();
                    }
                    break;
                case Game_Stage.Stage_Boss:
                    this.GetComponent<Collider>().enabled = false;
                    //Manager.Stage = Game_Stage.Clear;
                    break;
            }
        }
    }
}
