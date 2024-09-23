using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlienWalkableSurface : MonoBehaviour
{
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        AlienMove alien = FindObjectOfType<AlienMove>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { alien.PointerClickMove((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
}
