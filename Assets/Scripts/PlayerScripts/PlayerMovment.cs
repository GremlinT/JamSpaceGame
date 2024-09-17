using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    CameraScript cam;

    Transform TR;

    public bool focusedOnItem;

    private Usable currentUsable;
    [SerializeField]
    private Pickable currentPickable;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TR = transform;
    }

    void Update()
    {
        if (currentUsable && (TR.position - currentUsable.GetUsePosition()).magnitude <= 0.1f)
        {
            focusedOnItem = true;
            currentUsable.Use(this);
        }
        if (currentPickable && (TR.position - currentPickable.transform.position).magnitude <= 2f)
        {
            currentPickable.TakeItem();
            currentPickable = null;
        }
        if (!focusedOnItem)
        {
            cam.SetCameraTarget(GetComponent<CameraTargetScript>());
        }
    }

    public void MoveToPointer(BaseEventData _pointer) 
    {
        if (!focusedOnItem)
        {
            currentUsable = null;
            PointerEventData pointer = (PointerEventData)_pointer;
            Vector3 pointerPosition = pointer.pointerCurrentRaycast.worldPosition;
            agent.SetDestination(pointerPosition);
        }
    }

    public void InteractWithObject(Usable _usable)
    {
        if (!focusedOnItem)
        {
            currentUsable = _usable;
            agent.SetDestination(_usable.GetUsePosition());
        }
    }

    public void MoveToObject(Pickable _pickable)
    {
        if (!focusedOnItem)
        {
            currentPickable = _pickable;
            agent.SetDestination(_pickable.transform.position);
        }
        else
        {
            if (_pickable.CheckAssotiatetUsable(currentUsable))
            {
                _pickable.TakeItem();
            }
        }
    }

    public void StopFocusing()
    {
        focusedOnItem = false;
        currentUsable = null;
    }
}
