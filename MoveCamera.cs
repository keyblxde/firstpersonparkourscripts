using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPostion = null;
    
    void Update()
    {
        transform.position = cameraPostion.position;
    }
}
