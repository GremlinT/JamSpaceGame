using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipUsable : UsableItem
{
    [SerializeField]
    private Transform upDoor, downDoor;

    private Quaternion upDoorOpenRotation, downDoorOpenRotation;
    private Quaternion upDoorCloseRotation, downDoorCloseRotation;

    private Transform TR;

    [SerializeField]
    private AlienCamera camera;

    [SerializeField]
    Transform pathPoint1, pathPoint2;
    [SerializeField]
    Transform cameraPoint1, cameraPoint2, exitCameraPoint;

    //состояния
    [SerializeField]
    private bool doorsIsOpen;
    [SerializeField]
    private bool userGoInside;
    [SerializeField]
    private bool userInside;
    [SerializeField]
    private bool readyToFly;

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
            }
            if (userGoInside && !userInside && !isStopUsing)
            {
                if (Vector3.Distance(user.GetPosition(), pathPoint1.position) <= 0.1f)
                {
                    user.ForceMove(pathPoint2.position);
                    camera.SetCameraToPosition(cameraPoint1);
                }
                if (Vector3.Distance(user.GetPosition(), pathPoint2.position) <= 0.1f)
                {
                    user.ForceMove(operationalPoint.position);
                    camera.SetCameraToPosition(cameraPoint2);
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
                }
                if (userInside && userGoInside && doorsIsOpen)
                {
                    user.ForceMove(pathPoint2.position);
                    user.ForceRotate(usePoint.position);
                    canStopUseManualy = false;
                    userGoInside = false;
                    camera.SetCameraToPosition(cameraPoint1);
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
                        camera.ClearCamera();
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
    }
}
