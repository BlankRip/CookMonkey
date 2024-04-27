using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum LookAtMode
    {
        LookAt, LookAtInverted, CameraForward, CameraForwardInverted
    }

    [SerializeField] private LookAtMode lookAtMode;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        switch (lookAtMode)
        {
            case LookAtMode.LookAt:
                transform.LookAt(cameraTransform);
                break;
            case LookAtMode.LookAtInverted:
                Vector3 dirFromCam = transform.position - cameraTransform.position;
                transform.LookAt(transform.position + dirFromCam);
                break;
            case LookAtMode.CameraForward:
                transform.forward = cameraTransform.forward;
                break;
            case LookAtMode.CameraForwardInverted:
                transform.forward = -cameraTransform.forward;
                break;
            default:
                break;
        }
    }
}
