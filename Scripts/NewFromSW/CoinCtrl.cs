using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour
{
    Game_Manager manager;
    GameObject player;
    public int amount { get; set; }
    [SerializeField] float detectRange;

    void Start()
    {
        manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        player = manager.Player;
    }

    void Update()
    {
        //플레이어의 기준 좌표 보정(기준좌표가 땅바닥에 붙어있음)
        Vector3 correctPlayerPos = player.transform.position;
        correctPlayerPos.y += 1f;

        //플레이어와의 거리 계산
        float dist = Vector3.Distance(transform.position, correctPlayerPos);

        if(dist <= detectRange)
        {
            //플레이어를 향한 방향벡터
            Vector3 dir = correctPlayerPos - transform.position;
            dir.Normalize();
            transform.Translate(dir * Time.deltaTime * 100f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            manager.Count_Money(amount);
            Destroy(gameObject);
        }
    }
}
