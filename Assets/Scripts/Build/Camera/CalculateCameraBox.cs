using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CalculateCameraBox : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        if (!TryGetComponent(out _camera))
        {
            Debug.Log("CameraCalculateBox: no camera detected, please attach this script to a camera object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
