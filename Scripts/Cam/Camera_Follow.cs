using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [Header("Cam Position")]
    [SerializeField]private Transform _camPos;          //ī�޶��� ��ġ�� �޾ƿ��� ����
    // Start is called before the first frame update
    void Start()
    {
        _camPos = GameObject.Find("Player").transform.Find("Dust").transform.Find("CamPoint");//ī�޶��� ��ġ�� ��������
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _camPos.position;//ī�޶��� ��ġ�� �������� ��ȯ������
    }
}
