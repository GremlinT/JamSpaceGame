using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Droid : UseType
{
    [SerializeField]
    Transform basePosition;
    [SerializeField]
    BrocenDoor brockenDoor;

    PlayerMovment player;

    NavMeshAgent agent;

    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovment>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { player.InteractWithObject(usable); });
        trigger.triggers.Add(entry);
    }

    public override void Use(CameraScript _cam)
    {
        if (Storyline.needRepearDroid && !Storyline.repearDroid)
        {
            base.Use(_cam);
        }
        
    }

    public void ReturnTobase()
    {
        agent.SetDestination(basePosition.position);
        brockenDoor.DoorRepeared();
        Storyline.repearDroid = true;
        usable.StopUsing();
    }
    void Update()
    {
        
    }
}
