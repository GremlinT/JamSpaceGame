using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    Transform TR;
    [SerializeField]
    PlayerMovment player;
    [SerializeField]
    Transform doorModel;

    private bool isClosed;

    // Start is called before the first frame update
    void Start()
    {
        TR = transform;
        isClosed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((TR.position - player.transform.position).magnitude <= 2f)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        if (isClosed)
        {
            doorModel.Translate(doorModel.right * Time.deltaTime);
            Debug.Log((doorModel.position - (TR.up * 2.5f)).magnitude);
            if ((doorModel.position - (TR.right * 3 + TR.up * 2.5f)).magnitude <= 0.01) isClosed = false;
        }
    }

    private void CloseDoor()
    {
        if (!isClosed)
        {
            doorModel.Translate(-doorModel.right * Time.deltaTime);
            if ((doorModel.position - TR.right).magnitude <= 0.01) isClosed = true;
        }
    }
}
