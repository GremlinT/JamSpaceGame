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
    
    // Start is called before the first frame update
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private protected void SetUsable()
    {
        usable = GetComponent<Usable>();
    }
}
