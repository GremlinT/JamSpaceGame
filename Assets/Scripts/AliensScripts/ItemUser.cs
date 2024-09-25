using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUser : MonoBehaviour
{
    private UsableItem currentUseItem;
    private Transform TR;

    public bool isUse { get; private set; }

    void Start()
    {
        TR = transform;
    }

    public void SetUseItem(UsableItem usableItem)
    {
        currentUseItem = usableItem;
    }

    /*public bool CanStopUseItem()
    {
        if (currentUseItem.canStopUseManualy) return true;
        else return false;
    }*/

    private void UseItem(UsableItem item)
    {
        isUse = true;
        //item.Use();
    }

    /*public void StopUseItem()
    {
        currentUseItem.StopUse();
        isUse = false;
        currentUseItem = null;
    }*/

    void Update()
    {
        if (currentUseItem != null && !isUse)
        {
            if (currentUseItem.IsNear(TR)) 
            {
                UseItem(currentUseItem);
            }
        }
    }
}
