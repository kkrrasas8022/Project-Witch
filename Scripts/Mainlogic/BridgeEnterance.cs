using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEnterance : MonoBehaviour
{
    Game_Manager Manager;
    public GameObject bridgeWall;

    private void Awake()
    {
        Init_Data();
    }

    void Init_Data()
    {
        Manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
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
                case Game_Stage.Stage_02:
                    this.GetComponent<Collider>().enabled = false;
                    Manager.Stage = Game_Stage.Stage_02_Middle_Boss;
                    break;
                case Game_Stage.Stage_Boss:
                    this.GetComponent<Collider>().enabled = false;
                    Manager.Stage = Game_Stage.Clear;
                    break;
            }

            bridgeWall.SetActive(true);
        }
    }
}
