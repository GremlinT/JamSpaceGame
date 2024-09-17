using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    [SerializeField]
    Transform pointForUse;
    [SerializeField]
    CameraScript cam;

    private bool isUsing;

    private PlayerMovment player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsing)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                isUsing = false;
                player.StopFocusing();
            }
        }
    }

    public void Use(PlayerMovment _player)
    {
        isUsing = true;
        player = _player;
        CameraFocusing();
    }

    public Vector3 GetUsePosition()
    {
        return pointForUse.position;
    }

    private void CameraFocusing()
    {
        if (GetComponent<CameraTargetScript>())
        {
            cam.SetCameraTarget(GetComponent<CameraTargetScript>());
        }
    }
}
