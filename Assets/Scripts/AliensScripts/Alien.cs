using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    [SerializeField]
    private string alienName;
    [SerializeField]
    private float moveSpeed;
    private Transform TR;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isForced, isForceMove, isForceRotate;
    private Vector3 forceMoveTarget;
    private Vector3 forceRotationTarget;

    void Start()
    {
        TR = transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 destination)
    {
        if (!isForced)
        {
            agent.SetDestination(destination);
            animator.SetBool("isMove", true);
        }
    }

    public void ForceMove(Vector3 destination)
    {
        isForced = true;
        agent.enabled = false;
        
        forceMoveTarget = destination;
        isForceMove = true;
        
        animator.SetBool("isMove", true);
    }

    public void ForceRotate(Vector3 rotationTarget)
    {
        isForced = true;
        agent.enabled = false;
        
        forceRotationTarget = rotationTarget;
        isForceRotate = true;
        
        animator.SetBool("isMove", true);
    }
    
    private void InternalForceMove()
    {
        if (isForceMove)
        {
            Vector3 targetDestination = (forceMoveTarget - TR.position).normalized;
            TR.position = TR.position + targetDestination * Time.deltaTime * moveSpeed;
            if (Vector3.Distance(TR.position, forceMoveTarget) < 0.05f)
            {
                isForceMove = false;
                if (!isForceRotate) animator.SetBool("isMove", false);
                TR.position = forceMoveTarget;
            }
        }
    }
    private void InternalForceRotation()
    {
        if (isForceRotate)
        {
            Quaternion targetRotation = Quaternion.LookRotation((forceRotationTarget - TR.position).normalized, TR.up);
            TR.rotation = Quaternion.Lerp(TR.rotation, targetRotation, 5 * Time.deltaTime);
            if (Quaternion.Angle(targetRotation, TR.rotation) < 2f)
            {
                TR.rotation = targetRotation;
                if (!isForceMove) animator.SetBool("isMove", false);
                isForceRotate = false;
            }
        }
    }

    public void StopMove()
    {
        isForced = false;
        isForceMove = false;
        isForceRotate = false;
        agent.enabled = true;
        agent.ResetPath();
        animator.SetBool("isMove", false);
    }
    
    public Vector3 GetPosition()
    {
        return TR.position;
    }

    void Update()
    {
        InternalForceMove();
        InternalForceRotation();
    }
}
