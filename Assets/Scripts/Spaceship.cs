using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : UseType
{
    [SerializeField]
    InventorySystem invetory;
    [SerializeField]
    Transform playerTR;

    public override void Use(CameraScript _cam)
    {
        if (invetory.CheckInInventory(1))
        {
            base.Use(_cam);
            playerTR.SetParent(transform);
            playerTR.position = transform.position;
        }
        else
        {
            Debug.Log("Ключей нет");
            usable.StopUsing();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
