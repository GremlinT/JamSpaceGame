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
    [SerializeField]
    float doorSpeed;

    private bool isClosed;

    private Vector3 openPosition, closePosition;

    // Start is called before the first frame update
    void Start()
    {
        TR = transform;
        isClosed = true;
        openPosition = TR.position + TR.right * 3 + TR.up * 2;
        closePosition = TR.position + TR.up * 2;
        player = FindObjectOfType<PlayerMovment>();
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
            doorModel.position = Vector3.Lerp(doorModel.position, openPosition, Time.deltaTime * doorSpeed);
            if ((doorModel.position - openPosition).magnitude <= 0.05) isClosed = false;
        }
    }

    private void CloseDoor()
    {
        if (!isClosed)
        {
            doorModel.position = Vector3.Lerp(doorModel.position, closePosition, Time.deltaTime * doorSpeed);
            if ((doorModel.position - closePosition).magnitude <= 0.05) isClosed = true;
        }
    }
}
