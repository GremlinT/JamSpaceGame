using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    [SerializeField]
    Transform pointForUse;
    [SerializeField]
    CameraScript cam;

    [SerializeField]
    UseType useType;

    [SerializeField]
    Usable assotiatedUsable;

    private bool isUsing;
    [SerializeField]
    private bool stopUsingAccesable;

    private PlayerMovment player;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetStopUsingAccesable(bool setting)
    {
        stopUsingAccesable = setting;
    }
    public void SetAssotiatedUsingAccesable(bool setting)
    {
        if (assotiatedUsable)
        {
            assotiatedUsable.SetStopUsingAccesable(setting);
        }
    }

    public bool CheckAssotiatetUsable(Usable _usable)
    {
        if (_usable == assotiatedUsable)
        {
            return true;
        }
        else return false;
    }
    // Update is called once per frame
    void Update()
    {
        StopUsingByKey();
    }

    public void Use(PlayerMovment _player)
    {
        isUsing = true;
        player = _player;
        useType.Use(cam);
    }

    public Vector3 GetUsePosition()
    {
        return pointForUse.position;
    }

    public bool HasUsePosition()
    {
        if (pointForUse) return true;
        else return false;
    }

    private void StopUsingByKey()
    {
        if (Input.GetKey(KeyCode.Escape) && stopUsingAccesable) StopUsing();
    }

    public void StopUsing()
    {
        if (isUsing)
        {
            isUsing = false;
            player.StartUseNavMesh();
            player.StopFocusing();
            player.transform.SetParent(null);
        }
    }
}
