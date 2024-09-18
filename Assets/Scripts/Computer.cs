using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : UseType
{
    [SerializeField]
    GameObject monitor, keyboard, monitorButtons;

    private bool isACtive, needCheckInformation;

    public override void Use(CameraScript _cam)
    {
        if (!isACtive)
        {
            isACtive = true;
            monitor.SetActive(true);
            keyboard.SetActive(true);
        }
        else
        {
            isACtive = false;
            monitor.SetActive(false);
            keyboard.SetActive(false);
        }
        
    }

    void Start()
    {
        isACtive = false;
        needCheckInformation = true;
    }

    void Update()
    {
        if (isACtive && needCheckInformation) computerInformation();
    }

    private void computerInformation()
    {
        if (Storyline.hasFirstMission)
        {
            monitorButtons.SetActive(true);
        }
        else if (Storyline.copyPhoto)
        {
            monitorButtons.SetActive(false);
            needCheckInformation = false;
        }
        else 
        {

        }
    }

    public void ComputerWork()
    {
        if (!Storyline.openMail)
        {
            Debug.Log("Open mail");
            Storyline.openMail = true;
        }
        else if (!Storyline.copyPhoto)
        {
            Debug.Log("Copying photo");
            Storyline.copyPhoto = true;
        }
    }
}
