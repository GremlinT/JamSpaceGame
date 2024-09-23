using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapIconScript : MonoBehaviour
{
    private Vector3 nearObjectPosition;
    [SerializeField]
    private Transform landingPosition;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClickDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    public void SetPositions(Vector3 _nearObjectPosition, Transform _landingPosition)
    {
        nearObjectPosition = _nearObjectPosition;
        landingPosition = _landingPosition;
    }

    public void OnPointerClickDelegate(PointerEventData data)
    {
        Spaceship spaceship = FindObjectOfType<Spaceship>();
        spaceship.SetMoveTargetPoint(nearObjectPosition);
        spaceship.SetLandingPosition(landingPosition);
    }

}
