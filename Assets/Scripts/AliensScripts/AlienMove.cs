using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AlienMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;
    private Transform TR;

    private Animator animator;

    private bool isMoving;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        TR = transform;
    }

    /*public void PointerClickMove(PointerEventData _pointer)
    {
        Debug.Log("click");
        MoveToDestination(_pointer.pointerCurrentRaycast.worldPosition);
    }*/

    public void MoveToDestination(Vector3 _destination)
    {
        destination = _destination;
        agent.SetDestination(destination);
        isMoving = true;
        animator.SetBool("isMove", true);
    }

    private bool ReachingDestinationPoint()
    {
        if (Vector3.Distance(TR.position, destination) < agent.stoppingDistance && isMoving) return true;
        else return false;
    }

    private void StopMoving()
    {
        isMoving = false;
        animator.SetBool("isMove", false);
    }

    void Update()
    {
        if (ReachingDestinationPoint()) StopMoving();
    }
}
