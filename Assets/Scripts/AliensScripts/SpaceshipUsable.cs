using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipUsable : UsableItem
{
    [SerializeField]
    Transform upDoor, downDoor;

    private bool upDoorOpen, downDoorOpen;

    void Start()
    {
        AddClickEventTrigger();
        canStopUseManualy = true;
    }

    protected override void InternalUse()
    {
        if (isUsed)
        {
            if (!upDoorOpen)
            {
                upDoor.Rotate(upDoor.right, 40f);
                upDoorOpen = true;
            }
            
        }
        
    }

    void Update()
    {
        InternalUse();
    }
}
