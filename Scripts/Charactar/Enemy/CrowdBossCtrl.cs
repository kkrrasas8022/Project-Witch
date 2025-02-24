using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBossCtrl : MonoBehaviour
{
    int count = 3;
    Game_Manager gameMgr;

    void Start()
    {
        count = transform.childCount;
        gameMgr = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
    }

    void Update()
    {
        if(count == 0)
        {
            gameMgr.Clear_Boss();
        }
        count = transform.childCount;
    }
}
