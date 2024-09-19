using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    CameraScript cam;

    private CameraTargetScript cts;
    private bool camTargetAtPlayer;

    Transform TR;

    public bool focusedOnItem;

    private Usable currentUsable;
    [SerializeField]
    private Pickable currentPickable;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TR = transform;
        cts = GetComponent<CameraTargetScript>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!focusedOnItem)
        {
            if (currentUsable && (TR.position - currentUsable.GetUsePosition()).magnitude <= 0.1f)
            {
                Debug.Log("1");
                focusedOnItem = true;
                camTargetAtPlayer = false;
                currentUsable.Use(this);
                if (agent.enabled) agent.isStopped = true;
            }
        }
        if (currentPickable && (TR.position - currentPickable.transform.position).magnitude <= 2f)
        {
            currentPickable.TakeItem();
            currentPickable = null;
        }
        if (!focusedOnItem && !camTargetAtPlayer)
        {
            cam.SetCameraTarget(cts);
            camTargetAtPlayer = true;
        }
    }

    public void MoveToPointer(BaseEventData _pointer) 
    {
        Debug.Log(agent.isStopped);
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
            if (_usable.HasUsePosition())
            {
                currentUsable = _usable;
                agent.SetDestination(_usable.GetUsePosition());
            }
            else Debug.Log("no focus point");
        }
        else
        {
            if (_usable.CheckAssotiatetUsable(currentUsable))
            {
                _usable.Use(this);
            }
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
        Debug.Log("2");
        focusedOnItem = false;
        currentUsable = null;
        agent.isStopped = false;
    }

    public void StopUseNavMesh()
    {
        agent.isStopped = true;
        agent.enabled = false;
    }
    public void StartUseNavMesh()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }
    }
}
