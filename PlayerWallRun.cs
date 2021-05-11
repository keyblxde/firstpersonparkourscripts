using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]private Transform orientation = null;

    [Header("Wall Detection")]
    [SerializeField] private float minWallDistance = 1.0f;
    [SerializeField] private float minJumpHeight= 1.5f;
    private bool leftWall = false;
    private bool rightWall = false;
    RaycastHit lHit;
    RaycastHit rHit;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;
    Vector3 wallRunDirection;
    bool isWallRunning;

    [Header("Wall Run Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;
    public float currentCamTilt { get; private set; }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (leftWall)
            {
                StartWallRun();
                isWallRunning = true;
                WallRunCamera();
                Debug.Log("wall running on the left");
            }
            else if (rightWall)
            {
                StartWallRun();
                isWallRunning = true;
                WallRunCamera();
                Debug.Log("wall running on the right");
            }
            else
            {
                EndWallRun();
                isWallRunning = false;
                WallRunCamera();
            }
        }
        else
        {
            EndWallRun();
            isWallRunning = false;
            WallRunCamera();
        }
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }

    void CheckWall()
    {
        leftWall = Physics.Raycast(transform.position, -orientation.right, out lHit, minWallDistance);
        rightWall = Physics.Raycast(transform.position, orientation.right, out rHit, minWallDistance);
    }
    
    void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (leftWall)
            {
                wallRunDirection = transform.up + lHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (rightWall)
            {
                wallRunDirection = transform.up + rHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            
        }

    }

    void WallRunCamera()
    {
        if (isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

            if (leftWall)
            {
                currentCamTilt = Mathf.Lerp(currentCamTilt, -camTilt, camTiltTime * Time.deltaTime);
            }
            else if (rightWall)
            {
                currentCamTilt = Mathf.Lerp(currentCamTilt, camTilt, camTiltTime * Time.deltaTime);
            }
        }
        else if (!isWallRunning)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
            currentCamTilt = Mathf.Lerp(currentCamTilt, 0, camTiltTime * Time.deltaTime);
        }
    }

    void EndWallRun()
    {
        rb.useGravity = true;
    }


}
