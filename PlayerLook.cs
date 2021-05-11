using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerWallRun wallRun;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;
    [SerializeField] float sensX = 50.0f;
    [SerializeField] float sensY = 50.0f;
    float sensMultiplier = 0.01f;

    float mouseX, mouseY;
    float xRotation, yRotation;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateLook();
    }

    void UpdateLook()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * sensMultiplier;
        xRotation -= mouseY * sensY * sensMultiplier;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.currentCamTilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }




}
