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
        //�÷��̾��� ���� ��ǥ ����(������ǥ�� ���ٴڿ� �پ�����)
        Vector3 correctPlayerPos = player.transform.position;
        correctPlayerPos.y += 1f;

        //�÷��̾���� �Ÿ� ���
        float dist = Vector3.Distance(transform.position, correctPlayerPos);

        if(dist <= detectRange)
        {
            //�÷��̾ ���� ���⺤��
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
