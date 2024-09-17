using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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

    public void StopFocusing()
    {
        focusedOnItem = false;
        currentUsable = null;
    }
}
