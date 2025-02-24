using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Rotate : MonoBehaviour
{
    [SerializeField] float _rotspeed;
    public bool canMoveCam;
    float _rot_x;//������ ȸ����
    float _rot_y;//������ ȸ����

    // Start is called before the first frame update
    void Start()
    {
        _rotspeed = 360f;
    }

    

    // Update is called once per frame
    void Update()
    {
        Camera_Rot();
    }

    void Camera_Rot()//ī�޶� ȸ��
    {
        if (canMoveCam == false)
            return;
         Vector2 mousepos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//���콺 ��ġ�� �޾ƿ�

         _rot_x += mousepos.x * Time.deltaTime * _rotspeed;//ȸ������ ����
         _rot_y += mousepos.y * Time.deltaTime * _rotspeed;//ȸ������ ����

         _rot_y = Mathf.Clamp(_rot_y, -90f, 90f);//x�� ȸ������ �ִ밪�� ���ص�

        this.transform.eulerAngles = new Vector3(-_rot_y, _rot_x, 0);//ȸ���� ��ǥ�� �̵���Ŵ
        
    }
}
