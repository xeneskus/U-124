using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCamnCont : MonoBehaviour
{
    public float _xSens, _ySens;

    public Transform annen;
    //public Transform LookPoint;

    float xRotation, yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


 
    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _xSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _ySens;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //kamera dondurgec
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        annen.rotation = Quaternion.Euler(0, yRotation, 0);
        //LookPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }
}
