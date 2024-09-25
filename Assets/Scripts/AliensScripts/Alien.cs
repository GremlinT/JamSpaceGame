using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    [SerializeField]
    private string alienName;
    private Transform TR;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        TR = transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
        animator.SetBool("isMove", true);
    }
    
    public void StopMove()
    {
        agent.ResetPath();
        animator.SetBool("isMove", false);
    }
    
    public Vector3 GetPosition()
    {
        return TR.position;
    }

    void Update()
    {
        
    }
}
