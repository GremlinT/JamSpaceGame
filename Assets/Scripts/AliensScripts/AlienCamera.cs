using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlienCamera : MonoBehaviour
{
    private Transform TR;

    Vector3 newPosition;
    Quaternion newRotation;
    bool isMove, isRotate;
    bool cameraIsBusy;
    [SerializeField]
    Alien alien;
    [SerializeField]
    Transform cameraPosition;

    void Start()
    {
        TR = transform;
    }

    private void AlienFollow()
    {
        TR.LookAt(alien.GetPosition());
        TR.position = Vector3.Lerp(TR.position, cameraPosition.position, Time.deltaTime);
    }

    public void SetCameraToPosition(Transform targetPosition)
    {
        newPosition = targetPosition.position;
        newRotation = targetPosition.rotation;
        isMove = true;
        isRotate = true;
        cameraIsBusy = true;
    }

    public void ClearCamera()
    {
        cameraIsBusy = false;
        TR.SetParent(null);
    }

    public void SetcameraParent(Transform newParent)
    {
        TR.SetParent(newParent);
    }
        
    
    void Update()
    {
        if (isMove)
        {
            TR.position = Vector3.Lerp(TR.position, newPosition, Time.deltaTime);
            if (Vector3.Distance(TR.position, newPosition) < 0.1)
            {
                isMove = false;
            }
        }
        if (isRotate)
        {
            TR.rotation = Quaternion.Lerp(TR.rotation, newRotation, Time.deltaTime);
            if (Quaternion.Angle(newRotation, TR.rotation) < 2f)
            {
                TR.rotation = newRotation;
                isRotate = false;
            }
        }
        if (!cameraIsBusy) AlienFollow();
    }
}
