using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrocenDoor : MonoBehaviour
{
    [SerializeField]
    AutoDoor autoDoor;

    [SerializeField]
    Transform brokeDoorPosition;

    bool doorIsBroken;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (!doorIsBroken)
        {
            autoDoor.SetOpenDoorPosition(brokeDoorPosition);
            doorIsBroken = true;
        }
    }

    public void DoorRepeared()
    {
        autoDoor.SetBaseOpenDoorPosition();
    }
}
