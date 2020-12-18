using System;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform playerCam;
    public float mouseSense;
    public GameObject TriPod;
    private float xCamRotation = 0f;
    private float yCamRotation = 0f;
    private float yTriRotation = 0f;
    private bool canLook = false;
    private bool canRotate = false;
    private Vector3 defaultPos;
    private Quaternion defaultRotation;
    public float ZoomIncr = 3f;


    public int reverse = 1;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        defaultPos = playerCam.position;
        defaultRotation = playerCam.localRotation;
        //defaultPos = playerCam.transform;
    }

    void Update()
    {
        CamLook();
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom(ZoomIncr);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Zoom(-ZoomIncr);
        }
    }

    private void Zoom(float incr)
    {
        defaultPos = Vector3.MoveTowards(transform.position, TriPod.transform.position, incr);
    }

    void CamLook()
    {
        if (Input.GetMouseButtonDown(2))
        {
            canLook = true;
            xCamRotation = playerCam.rotation.eulerAngles.x;
            yCamRotation = playerCam.rotation.eulerAngles.y * -1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(2))
        {
            canLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            canRotate = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

            xCamRotation -= mouseY;
            yCamRotation -= mouseX;
            playerCam.rotation = Quaternion.Euler(xCamRotation, yCamRotation * -1, 0f);
        }
        else if (canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

            //xTriRotation -= mouseY;
            yTriRotation -= mouseX;
            TriPod.transform.rotation = Quaternion.Euler(TriPod.transform.rotation.x, yTriRotation * -1, 0f);
            defaultPos = playerCam.position;
        }
        else
        {
            //defaultPos.x = playerCam.position.x;
            playerCam.position = Vector3.Lerp(playerCam.position, defaultPos, 5.0f * Time.deltaTime);
            //playerCam.rotation = Quaternion.Lerp(playerCam.rotation, defaultRotation, 5.0f * Time.deltaTime);
            Quaternion temp = Quaternion.LookRotation(TriPod.transform.position - transform.position);
            playerCam.rotation = Quaternion.Lerp(transform.rotation, temp, 5.0f * Time.deltaTime);
        }
    }
}
