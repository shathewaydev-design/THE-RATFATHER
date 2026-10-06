using UnityEngine;
using System;

public class BillBoard : MonoBehaviour
{
    
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
            return;

        // Vector3 direction = mainCamera.transform.position - transform.position;

        // // Only rotate around Y axis
        // direction.y = 0f;

        // if (direction.sqrMagnitude > 0.001f)
        // {
        //     transform.rotation = Quaternion.LookRotation(direction);
        // }
        transform.forward = mainCamera.transform.forward;
    }

    
}