using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipUsable : UsableItem
{
    [SerializeField]
    private Transform upDoor, downDoor;

    private Quaternion upDoorOpenRotation, downDoorOpenRotation;
    private Quaternion upDoorCloseRotation, downDoorCloseRotation;

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

        upDoorCloseRotation = upDoor.localRotation;
        downDoorCloseRotation = downDoor.localRotation;

        upDoor.Rotate(upDoor.InverseTransformDirection(upDoor.forward), -89);
        upDoorOpenRotation = upDoor.localRotation;
        upDoor.localRotation = upDoorCloseRotation;

        downDoor.Rotate(downDoor.InverseTransformDirection(downDoor.forward), 58);
        downDoorOpenRotation = downDoor.localRotation;
        downDoor.localRotation = downDoorCloseRotation;
    }

    protected override void InternalUse()
    {
        if (isUsed)
        {
            if (!doorsIsOpen)
            {
                if (upDoor.localRotation != upDoorOpenRotation)
                {
                    upDoor.Rotate(upDoor.InverseTransformDirection(upDoor.forward), -89 * Time.deltaTime);
                    doorsIsOpen = false;
                    if (Quaternion.Angle(upDoorOpenRotation, upDoor.localRotation) < 5)
                    {
                        upDoor.localRotation = upDoorOpenRotation;
                        doorsIsOpen = true;
                    }
                }
                if (downDoor.localRotation != downDoorOpenRotation)
                {
                    downDoor.Rotate(downDoor.InverseTransformDirection(downDoor.forward), 58 * Time.deltaTime);
                    doorsIsOpen = false;
                    if (Quaternion.Angle(downDoorOpenRotation, downDoor.localRotation) < 5)
                    {
                        downDoor.localRotation = downDoorOpenRotation;
                        doorsIsOpen = true;
                    }
                }
            }
            if (doorsIsOpen && !userGoInside)
            {
                user.Move(operationalPoint.position);
                userGoInside = true;
            }
            if (userGoInside && !userInside)
            {
                if (Vector3.Distance(user.GetPosition(), operationalPoint.position) < 0.5f)
                {
                    user.StopMove();
                    userInside = true;
                }
            }
            
        }
        else
        {
            if (upDoor.localRotation != upDoorCloseRotation)
            {
                upDoor.Rotate(upDoor.InverseTransformDirection(upDoor.forward), 90 * Time.deltaTime);
                if (Quaternion.Angle(upDoorCloseRotation, upDoor.localRotation) < 5)
                {
                    upDoor.localRotation = upDoorCloseRotation;
                }
                
            }
            if (downDoor.localRotation != downDoorCloseRotation)
            {
                downDoor.Rotate(downDoor.InverseTransformDirection(downDoor.forward), -58 * Time.deltaTime);
                if (Quaternion.Angle(downDoorCloseRotation, downDoor.localRotation) < 5)
                {
                    downDoor.localRotation = downDoorCloseRotation;
                }
            }
        }
        
    }

    void Update()
    {
        InternalUse();
    }
}
