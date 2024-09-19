using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapIconScript : MonoBehaviour
{
    private Vector3 landingPosition;
    
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    public void SetLandingPosition(Vector3 _landingPosition)
    {
        landingPosition = _landingPosition;
    }

    public void OnPointerClickDelegate(PointerEventData data)
    {
        FindObjectOfType<Spaceship>().SetMoveTargetPoint(landingPosition);
    }

}
