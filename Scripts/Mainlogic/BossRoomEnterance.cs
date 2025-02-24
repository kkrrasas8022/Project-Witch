using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnterance : MonoBehaviour
{
    public GameObject wall;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            wall.SetActive(true);
        }
    }
}
