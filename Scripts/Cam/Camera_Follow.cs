using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [Header("Cam Position")]
    [SerializeField]private Transform _camPos;          //카메라의 위치를 받아오는 변수
    // Start is called before the first frame update
    void Start()
    {
        _camPos = GameObject.Find("Player").transform.Find("Dust").transform.Find("CamPoint");//카메라의 위치를 가져오고
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _camPos.position;//카메라의 위치를 매프레임 변환시켜줌
    }
}
