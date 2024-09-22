using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseType : MonoBehaviour
{
    private protected Usable usable;

    public bool stopUsingAccesable;
    
    public virtual void Use(CameraScript _cam)
    {
        if (GetComponent<CameraTargetScript>())
        {
            _cam.SetCameraTarget(GetComponent<CameraTargetScript>());
        }
    }
    
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
    }

    void Update()
    {
        
    }

    private protected void SetUsable()
    {
        usable = GetComponent<Usable>();
    }
}
