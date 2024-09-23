using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform TR;
    [SerializeField]
    private CameraTargetScript cameraTarget;

    bool isFollow;

    void Start()
    {
        TR = transform;
        SetCameraTarget(cameraTarget);
        DontDestroyOnLoad(gameObject);
        isFollow = true;
    }

    void Update()
    {
        if (isFollow) CameraFollow();
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

    public void SetFollowOnOff(bool _isFollow)
    {
        isFollow = _isFollow;
    }
}
