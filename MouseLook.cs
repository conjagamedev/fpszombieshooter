using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; 

    public Transform playerBody; 

    float xRotation = 0f; 

    public float Clamp1 = -90f;
    public float Clamp2 = 90f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; 
        float joyX = Input.GetAxis("Joy X") * mouseSensitivity * Time.deltaTime; 
        float joyY = Input.GetAxis("Joy Y") * mouseSensitivity * Time.deltaTime; 
 
        xRotation -= mouseY;
        xRotation -= joyY;
        xRotation = Mathf.Clamp(xRotation, Clamp1, Clamp2); 

        transform.localRotation = Quaternion.Euler(xRotation, 0,0); 
        playerBody.Rotate(Vector3.up * mouseX);
        playerBody.Rotate(Vector3.up * joyX); 
    }
}
