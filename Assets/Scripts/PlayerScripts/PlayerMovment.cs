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

    public bool focusedOnItem;

    private Usable currentUsable;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && currentUsable)
        {
            transform.LookAt(currentUsable.transform.position);
        }
    }

    public void MoveToPointer(BaseEventData _pointer) 
    {
        currentUsable = null;
        PointerEventData pointer = (PointerEventData)_pointer;
        Vector3 pointerPosition = pointer.pointerCurrentRaycast.worldPosition;
        agent.SetDestination(pointerPosition);
    }

    public void InteractWithObject(Usable _usable)
    {
        currentUsable = _usable;
        agent.SetDestination(_usable.GetUsePosition());
    }
}
