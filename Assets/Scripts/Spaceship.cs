using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spaceship : UseType
{
    [SerializeField]
    InventorySystem invetory;
    [SerializeField]
    Transform playerTR;
    [SerializeField]
    PlayerMovment playerM;
    [SerializeField]
    Transform playerPlace;
    [SerializeField]
    private bool isActive, isLanding, isMoving, isTakeOff, isMoveToLanding;
    private Vector3 moveTargetPoint;

    [SerializeField]
    SpaceshipMap map;

    Transform TR;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    private Transform takeOffPoint;

    public override void Use(CameraScript _cam)
    {
        if (invetory.CheckInInventory(1))
        {
            base.Use(_cam);
            playerM.StopUseNavMesh();
            playerTR.SetParent(playerPlace);
            playerTR.position = playerPlace.position;
        }
        else
        {
            Debug.Log("Ключей нет");
            usable.StopUsing();
        }
    }

    void Start()
    {
        TR = transform;
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
    }

    void Update()
    {
        if (isActive)
        {
            MoveToPoint();
            if (isLanding)
            {
                Landing();
            }
            if (isTakeOff)
            {
                TakeOff();
            }
        }
    }

    public void TurnSpaceShipOnOff()
    {
        switch (isActive)
        {
            case true:
                RaycastHit hitInfo;
                if (Physics.Raycast(TR.position, -TR.up, out hitInfo, 20f))
                {
                    moveTargetPoint = hitInfo.point;
                    isLanding = true;
                }
                break;
            case false:
                isActive = true;
                stopUsingAccesable = false;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                moveTargetPoint = TR.position + TR.up * 0.5f;
                map.ActivateMap();
                break;
        }
    }

    public void AutoTakeOff()
    {
        isTakeOff = true;
    }
    public void AutoLanding()
    {
        isMoveToLanding = true;
    }

    private void MoveToPoint()
    {
        if (Vector3.Distance(TR.position, moveTargetPoint) > 100f)
        {
            TR.position += (moveTargetPoint - TR.position).normalized * Time.deltaTime * maxSpeed;
            if (!isMoving) isMoving = true;
        }
        else if (Vector3.Distance(TR.position, moveTargetPoint) > 0.1f)
        {
            TR.position = Vector3.Lerp(TR.position, moveTargetPoint, Time.deltaTime);
            if (!isMoving) isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void MoveToPointOnMap(BaseEventData _pointer)
    {
        Debug.Log(_pointer.selectedObject.name);
        //Vector3 targetPosition = _pointer.selectedObject.transform.position;
        //moveTargetPoint = targetPosition;
    }

    private void Landing()
    {
        if (!isMoving)
        {
            isActive = false;
            stopUsingAccesable = true;
            usable.SetStopUsingAccesable(stopUsingAccesable);
            isLanding = false;
            map.DeactivateMap();
        }
    }

    private void MovingToLanding()
    {
        if (!isMoving)
        {

        }
    }

    public void SetMoveTargetPoint(Vector3 _point)
    {
        if (!isMoving) moveTargetPoint = _point;
    }

    private void TakeOff()
    {
        if (!isMoving)
        {
            if (Vector3.Distance(TR.position, takeOffPoint.position) < 0.1)
            {
                takeOffPoint = null;
                isTakeOff = false;
            }
            else 
            {
                if (moveTargetPoint != takeOffPoint.position) moveTargetPoint = takeOffPoint.position;
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
