using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserController : MonoBehaviour
{
    private AlienStateMashine stateMashine;

    void Start()
    {
        stateMashine = GetComponent<AlienStateMashine>();
    }

    public void PointerClickMove(PointerEventData _pointer)
    {
        stateMashine.SetDestination(_pointer.pointerCurrentRaycast.worldPosition);
    }

    public void PointerClickSetUseItem(UsableItem item)
    {
        stateMashine.UseItem(item);
    }

    public void StopMovement()
    {
        stateMashine.SetDestination(Vector3.zero);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.X)) StopMovement();
    }
}
