using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UsableItem : MonoBehaviour
{
    [SerializeField]
    string itemName;
    [SerializeField]
    private Transform usePoint;
    [SerializeField]
    private float minUseDistance;
    public bool canStopUseManualy;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        UserController controller = FindObjectOfType<UserController>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { controller.PointerClickSetUseItem(this); });
        trigger.triggers.Add(entry);
    }

    public Vector3 GetUsePoint()
    {
        return usePoint.position;
    }

    public bool IsNear(Transform userTR)
    {
        if (Vector3.Distance(userTR.position, usePoint.position) < minUseDistance) return true;
        else return false;
    }

    public void Use()
    {
        Debug.Log("Use " + itemName);
    }
    
    public void StopUse()
    {
        Debug.Log("Stop using " + itemName);
    }
    

    void Update()
    {
        
    }
}
