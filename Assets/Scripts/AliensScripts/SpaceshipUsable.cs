using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class SpaceshipUsable : UsableItem
{
    [SerializeField]
    private Transform upDoor, downDoor;

    private Quaternion upDoorOpenRotation, downDoorOpenRotation;
    private Quaternion upDoorCloseRotation, downDoorCloseRotation;

    private Transform TR;

    [SerializeField]
    private AlienCamera mainCamera;

    [SerializeField]
    Transform pathPoint1, pathPoint2;
    [SerializeField]
    Transform cameraPoint1, cameraPoint2, exitCameraPoint;

    //оборудование
    [SerializeField]
    GameObject mainMonitor, leftMonitor, rightMonitor;
    [SerializeField]
    GameObject statusMonitorMain, statusMonitorMoveFirst, statusMonitorMoveSecond, statusMonitorRotate;
    [SerializeField]
    Transform statusMonitorMoveTRFirst, statusMonitorMoveTRSecond, statusMonitorRotateTR;

    //состояния
    [SerializeField]
    private bool doorsIsOpen;
    [SerializeField]
    private bool userGoInside;
    [SerializeField]
    private bool userInside;
    [SerializeField]
    private bool readyToFly;
    [SerializeField]
    private bool flying;
    [SerializeField]
    private bool landing;

    //таймеры
    [SerializeField]
    float timeBeforeTakeOff;

    void Start()
    {
        AddClickEventTrigger();
        canStopUseManualy = true;
        hasStopUsingProcedure = false;

        TR = transform;
        
        upDoorCloseRotation = upDoor.localRotation;
        downDoorCloseRotation = downDoor.localRotation;

        upDoor.Rotate(upDoor.InverseTransformDirection(upDoor.forward), -89);
        upDoorOpenRotation = upDoor.localRotation;
        upDoor.localRotation = upDoorCloseRotation;

        downDoor.Rotate(downDoor.InverseTransformDirection(downDoor.forward), 58);
        downDoorOpenRotation = downDoor.localRotation;
        downDoor.localRotation = downDoorCloseRotation;
    }
    
    private void DoorControl(Transform door, Quaternion targetRotation, float angel, out bool doorState, bool open)
    {
        doorState = false;
        if (door.localRotation != targetRotation)
        {
            door.Rotate(door.InverseTransformDirection(door.forward), angel * Time.deltaTime);
            if (open) doorState = false;
            else doorState = true;
            if (Quaternion.Angle(targetRotation, door.localRotation) < 5)
            {
                door.localRotation = targetRotation;
                if (open) doorState = true;
                else doorState = false;
            }
        }
    }

    public void OnOffButtonClick()
    {
        if (readyToFly)
        {
            if (!flying)
            {
                flying = true;
                canStopUseManualy = false;
                mainMonitor.SetActive(true);
                leftMonitor.SetActive(true);
                rightMonitor.SetActive(true);
                statusMonitorMain.SetActive(true);
                SetTargetPoint(TR.position + TR.up * 10f);
            }
            else
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(TR.position, -TR.up, out hitInfo, 450f))
                {
                    moveTargetPoint = hitInfo.point;
                    landing = true;
                }
                else
                {
                    Debug.Log("No place for landing");
                    landing = false;
                }
            }
            
        }
    }
    private void Landing()
    {
        if (!isMoving)
        {
            flying = false;
            landing = false;
            canStopUseManualy = true;
            mainMonitor.SetActive(false);
            leftMonitor.SetActive(false);
            rightMonitor.SetActive(false);
            statusMonitorMain.SetActive(false);
        }
    }

    [SerializeField]
    bool isRotating, isMoving, isSpecialRotation;
    [SerializeField]
    float maxSpeed;
    Quaternion specialRotation;
    Vector3 moveTargetPoint;
    private void SetTargetPoint(Vector3 _targetPoint)
    {
        moveTargetPoint = _targetPoint;
    }
    private void MoveToPoint()
    {
        RotateToPointDirection();
        if (!isRotating)
        {
            if (Vector3.Distance(TR.position, moveTargetPoint) > 100f)
            {
                TR.position += (moveTargetPoint - TR.position).normalized * Time.deltaTime * maxSpeed;
                if (!isMoving)
                {
                    statusMonitorMoveFirst.SetActive(true);
                    statusMonitorMoveSecond.SetActive(true);
                    isMoving = true;
                }
            }
            else if (Vector3.Distance(TR.position, moveTargetPoint) > 0.1f)
            {
                TR.position = Vector3.Lerp(TR.position, moveTargetPoint, Time.deltaTime);
                if (!isMoving)
                {
                    statusMonitorMoveFirst.SetActive(true);
                    statusMonitorMoveSecond.SetActive(true);
                    isMoving = true;
                }
            }
            else
            {
                statusMonitorMoveFirst.SetActive(false);
                statusMonitorMoveSecond.SetActive(false);
                isMoving = false;
            }
        }
        if (isSpecialRotation && !isMoving && !isRotating)
        {
            RotateToSpecialRotation();
        }
    }

    private void RotateToSpecialRotation()
    {
        if (Quaternion.Angle(TR.rotation, specialRotation) > 1f)
        {
            TR.rotation = Quaternion.Lerp(TR.rotation, specialRotation, Time.deltaTime);
        }
        else
        {
            isSpecialRotation = false;
        }
    }

    private void RotateToPointDirection()
    {
        if (Vector3.Distance(TR.position, moveTargetPoint) > 20f)
        {
            Vector3 targetVector = (moveTargetPoint - TR.position).normalized;
            if (Vector3.Angle(TR.forward, targetVector) < 2f)
            {
                TR.rotation = Quaternion.LookRotation(targetVector);
                statusMonitorRotate.SetActive(false);
                isRotating = false;
            }
            else
            {
                if (!isRotating)
                {
                    isRotating = true;
                    statusMonitorRotate.SetActive(true);
                }
                TR.rotation = Quaternion.Lerp(TR.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime);
            }
        }
    }

    protected override void InternalUse()
    {
        if (isUsed)
        {
            if (!doorsIsOpen && !readyToFly && !isStopUsing)
            {
                DoorControl(upDoor, upDoorOpenRotation, -89, out doorsIsOpen, true);
                DoorControl(downDoor, downDoorOpenRotation, 58, out doorsIsOpen, true);
            }
            if (doorsIsOpen && !userGoInside && !isStopUsing)
            {
                user.ForceMove(pathPoint1.position);
                user.ForceRotate(operationalPoint.position);
                canStopUseManualy = false;
                hasStopUsingProcedure = true;
                userGoInside = true;
                mainCamera.SetcameraParent(TR);
            }
            if (userGoInside && !userInside && !isStopUsing)
            {
                if (Vector3.Distance(user.GetPosition(), pathPoint1.position) <= 0.1f)
                {
                    user.ForceMove(pathPoint2.position);
                    mainCamera.SetCameraToPosition(cameraPoint1);
                }
                if (Vector3.Distance(user.GetPosition(), pathPoint2.position) <= 0.1f)
                {
                    user.ForceMove(operationalPoint.position);
                    mainCamera.SetCameraToPosition(cameraPoint2);
                }
                if (Vector3.Distance(user.GetPosition(), operationalPoint.position) <= 0.1f)
                {
                    user.transform.SetParent(gameObject.transform);
                    user.ForceRotate(operationalPoint.position + operationalPoint.forward);
                    userInside = true;
                }
            }
            if (userInside && doorsIsOpen && !isStopUsing)
            {
                DoorControl(upDoor, upDoorCloseRotation, 89, out doorsIsOpen, false);
                DoorControl(downDoor, downDoorCloseRotation, -58, out doorsIsOpen, false);
                if (!doorsIsOpen)
                {
                    readyToFly = true;
                    canStopUseManualy = true;
                }
            }
            if (isStopUsing)
            {
                if (userInside && !doorsIsOpen)
                {
                    readyToFly = false;
                    DoorControl(upDoor, upDoorOpenRotation, -89, out doorsIsOpen, true);
                    DoorControl(downDoor, downDoorOpenRotation, 58, out doorsIsOpen, true);
                    mainCamera.SetCameraToPosition(exitCameraPoint);
                }
                if (userInside && userGoInside && doorsIsOpen)
                {
                    user.ForceMove(pathPoint2.position);
                    user.ForceRotate(usePoint.position);
                    canStopUseManualy = false;
                    userGoInside = false;
                }
                if (!userGoInside && userInside)
                {
                    if (Vector3.Distance(user.GetPosition(), pathPoint2.position) <= 0.1f)
                    {
                        user.ForceMove(pathPoint1.position);
                        
                    }
                    if (Vector3.Distance(user.GetPosition(), pathPoint1.position) <= 0.1f)
                    {
                        user.ForceMove(usePoint.position);
                        mainCamera.ClearCamera();
                    }
                    if (Vector3.Distance(user.GetPosition(), usePoint.position) <= 0.1f)
                    {
                        user.transform.SetParent(null);
                        userInside = false;
                        hasStopUsingProcedure = false;
                        canStopUseManualy = true;
                        user.StopMove();
                        user.GetComponent<AlienStateMashine>().StopUseItem(); //StopUse(user);
                    }
                }
            }

        }
        else
        {
            DoorControl(upDoor, upDoorCloseRotation, 89, out doorsIsOpen, false);
            DoorControl(downDoor, downDoorCloseRotation, -58, out doorsIsOpen, false);
        }
        
    }

    void Update()
    {
        InternalUse();
        if (flying)
        {
            MoveToPoint();
        }
            
        if (landing) Landing();
       
    }
}
