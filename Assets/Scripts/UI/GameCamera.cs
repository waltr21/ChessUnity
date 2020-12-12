using System;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform playerCam;
    public float mouseSense;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool canLook = false;
    private Vector3 defaultPos;
    private Quaternion defaultRotation;


    public int reverse = 1;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        defaultPos = playerCam.position;
        defaultRotation = playerCam.rotation;
        //defaultPos = playerCam.transform;
    }

    void Update()
    {
        CamLook();
    }

    void CamLook()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canLook = true;
            xRotation = playerCam.localEulerAngles.x;
            yRotation = playerCam.localEulerAngles.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            canLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

            xRotation -= mouseY;
            yRotation -= mouseX;
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation * -1, 0f);
        }
        else
        {
            playerCam.position = Vector3.Lerp(playerCam.position, defaultPos, 5.0f * Time.deltaTime);
            playerCam.rotation = Quaternion.Lerp(playerCam.rotation, defaultRotation, 5.0f * Time.deltaTime);
        }
    }
}
