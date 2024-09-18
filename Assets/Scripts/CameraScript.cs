using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform TR;
    [SerializeField]
    private CameraTargetScript cameraTarget;

    void Start()
    {
        TR = transform;
        SetCameraTarget(cameraTarget);
    }

    void Update()
    {
        CameraFollow();
    }

    public void SetCameraTarget(CameraTargetScript _cameraTarget)
    {
        cameraTarget = _cameraTarget;
        SetCameraPosition(cameraTarget.cameraPoint);
    }

    private void SetCameraPosition(Transform _campoint)
    {
        TR.position = _campoint.position;
        TR.rotation = _campoint.rotation;
    }

    private void CameraFollow()
    {
        TR.position = cameraTarget.cameraPoint.position;
        TR.rotation = cameraTarget.cameraPoint.rotation;
    }
}
