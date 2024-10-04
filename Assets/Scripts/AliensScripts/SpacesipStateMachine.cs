using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SpaceshipStates
{
    offline,
    userBoarding,
    readyTofly,
    onland,
    takeoff,
    rotating,
    move,
    landing
}

public class SpacesipStateMachine : MonoBehaviour
{
    private Transform pointToUse;

    protected void AddClickEventTrigger()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        UserController controller = FindObjectOfType<UserController>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { controller.PointerClickSpaceship(pointToUse.position); });
        trigger.triggers.Add(entry);
    }

    void Start()
    {
        AddClickEventTrigger();
    }

    void Update()
    {
        
    }
}
