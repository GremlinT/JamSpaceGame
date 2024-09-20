using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WalkableSurface : MonoBehaviour
{
    PlayerMovment player;


    void Start()
    {
        player = FindObjectOfType<PlayerMovment>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { player.MoveToPointer((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }



    void Update()
    {

    }
}
