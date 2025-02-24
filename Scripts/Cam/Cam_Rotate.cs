using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Rotate : MonoBehaviour
{
    [SerializeField] float _rotspeed;
    public bool canMoveCam;
    float _rot_x;//누적된 회전값
    float _rot_y;//누적된 회전값

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

    void Camera_Rot()//카메라 회전
    {
        if (canMoveCam == false)
            return;
         Vector2 mousepos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//마우스 위치를 받아옴

         _rot_x += mousepos.x * Time.deltaTime * _rotspeed;//회전값을 저장
         _rot_y += mousepos.y * Time.deltaTime * _rotspeed;//회전값을 저장

         _rot_y = Mathf.Clamp(_rot_y, -90f, 90f);//x축 회전값의 최대값을 정해둠

        this.transform.eulerAngles = new Vector3(-_rot_y, _rot_x, 0);//회전한 좌표로 이동시킴
        
    }
}
