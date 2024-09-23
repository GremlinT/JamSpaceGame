using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserController : MonoBehaviour
{
    private AlienMove move;
    private ItemUser itemUser;

    private bool isMoveAccesable;

    void Start()
    {
        move = GetComponent<AlienMove>();
        itemUser = GetComponent<ItemUser>();
    }

    public void PointerClickMove(PointerEventData _pointer)
    {
        StartMove(_pointer.pointerCurrentRaycast.worldPosition);
    }

    public void PointerClickSetUseItem(UsableItem item)
    {
        StartMove(item.GetUsePoint());
        if (isMoveAccesable) itemUser.SetUseItem(item);
    }

    private void StartMove(Vector3 movePoint)
    {
        if (!itemUser.isUse)
        {
            move.MoveToDestination(movePoint);
            isMoveAccesable = true;
        }
        else if (itemUser.CanStopUseItem())
        {
            itemUser.StopUseItem();
            move.MoveToDestination(movePoint);
            isMoveAccesable = true;
        }
        else
        {
            isMoveAccesable = false;
            Debug.Log("Cant move");
        }
    }

    void Update()
    {
        
    }
}
