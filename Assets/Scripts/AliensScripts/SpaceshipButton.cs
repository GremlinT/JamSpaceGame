using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceshipButton : MonoBehaviour
{
    [SerializeField]
    private SpaceshipUsable spaceship;
    
    Transform TR;
    
    void Start()
    {
        TR = transform;

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { ButtonClick(); });
        trigger.triggers.Add(entryDown);
        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { ButtonOut(); });
        trigger.triggers.Add(entryUp);
    }

    private void ButtonClick()
    {
        TR.position = TR.position + TR.forward * 0.025f;
    }
    private void ButtonOut()
    {
        TR.position = TR.position + -TR.forward * 0.025f;
    }
}
