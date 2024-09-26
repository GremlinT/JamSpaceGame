using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UsableItem : MonoBehaviour
{
    [SerializeField]
    string itemName;
    [SerializeField]
    protected Transform usePoint, operationalPoint;
    [SerializeField]
    private float minUseDistance;

    protected bool canStopUseManualy, hasStopUsingProcedure;
    [SerializeField]
    protected bool isUsed, isStopUsing;
    
    protected Alien user;

    protected void AddClickEventTrigger()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        UserController controller = FindObjectOfType<UserController>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { controller.PointerClickSetUseItem(this); });
        trigger.triggers.Add(entry);
    }

    void Start()
    {
        AddClickEventTrigger();
        canStopUseManualy = true;
        hasStopUsingProcedure = false;
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

    public bool StartUse(Alien _user)
    {
        if (!isUsed && user == null)
        {
            isUsed = true;
            user = _user;
            return true;
        }
        else
        {
            Debug.Log(itemName + " is busy");
            return false;
        }
    }
    
    public bool StopUse(Alien _user)
    {
        if (isUsed)
        {
            if (user != _user)
            {
                Debug.Log(itemName + " is used by another user");
                return true;
            }
            else
            {
                if (canStopUseManualy)
                {
                    if (!hasStopUsingProcedure)
                    {
                        isStopUsing = false;
                        user = null;
                        isUsed = false;
                        return true;
                    }
                    isStopUsing = true;
                    Debug.Log("Begin stoping procedure for " + itemName);
                    return false;
                }
                else
                {
                    Debug.Log("Cant stop use " + itemName + " manualy");
                    return false;
                }
            }
        }
        else
        {
            Debug.Log(itemName + " is not used");
            return true;
        }
    }

    bool min = true;
    protected virtual void InternalUse()
    {
        if (isUsed)
        {
            if (transform.localScale.x > 0.5f && min) transform.localScale = transform.localScale + -Vector3.one * Time.deltaTime;
            if (transform.localScale.x <= 0.5f) { min = false; }
            if (transform.localScale.x < 1f && !min) transform.localScale = transform.localScale + Vector3.one * Time.deltaTime;
            if (transform.localScale.x >= 1f) { min = true; }
        }
        else
        {
            if (transform.localScale.x < 1f) transform.localScale = transform.localScale + Vector3.one * Time.deltaTime;
            if (transform.localScale.x >= 1f) { min = true; }
        }
    } 
    
    void Update()
    {
        InternalUse();
    }
}
