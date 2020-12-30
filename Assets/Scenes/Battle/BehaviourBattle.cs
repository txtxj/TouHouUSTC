using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourBattle : MonoBehaviour
{
    //枚举鼠标移动类型
    enum MoveMouseXY
    {
        MouseXAndY, MouseX, MouseY
    }

    MoveMouseXY axes = MoveMouseXY.MouseXAndY;

    //鼠标敏感度
    float sensitivityX = 0.3f;
    float sensitivityY = 0.3f;

    //相机视野范围
    int MaxCameraView = 7;
    int MinCameraView = 3;

    //相机上下左右最大范围
    float CameraMaxX = 5, CameraMinX = -5;
    float CameraMaxY = 3, CameraMinY = -3;


    //鼠标右键移动相机
    void MoveCamera()
    {
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("Input.GetMouseButton(1)");
            if (axes == MoveMouseXY.MouseXAndY)
            {
                transform.position +=
                    new Vector3(-Input.GetAxis("Mouse X") * sensitivityX, -Input.GetAxis("Mouse Y") * sensitivityY, 0);
            }
            else if (axes == MoveMouseXY.MouseX)
            {
                transform.position += new Vector3(-Input.GetAxis("Mouse X") * sensitivityX, 0, 0);
            }
            else
            {
                transform.position += new Vector3(0, -Input.GetAxis("Mouse Y") * sensitivityY, 0);
            }
            if (transform.position.x > CameraMaxX)
                transform.position = new Vector3(CameraMaxX, transform.position.y, transform.position.z);
            if (transform.position.x < CameraMinX)
                transform.position = new Vector3(CameraMinX, transform.position.y, transform.position.z);
            if (transform.position.y > CameraMaxY)
                transform.position = new Vector3(transform.position.x, CameraMaxY, transform.position.z);
            if (transform.position.y < CameraMinY)
                transform.position = new Vector3(transform.position.x, CameraMinY, transform.position.z);
        }

    }

    void ChangeCameraView()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.orthographicSize <= MaxCameraView)
                Camera.main.orthographicSize += 0.5F;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //if(Camera.main.fieldOfView>MaxCameraView)
            //	Camera.main.fieldOfView-=2;
            if (Camera.main.orthographicSize >= MinCameraView)
                Camera.main.orthographicSize -= 0.5F;
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        MoveCamera();
        ChangeCameraView();
    }

}
