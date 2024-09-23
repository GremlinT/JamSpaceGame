using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : UseType
{
    [SerializeField]
    GameObject monitor, keyboard, monitorButtons;

    private bool isACtive, needCheckInformation;

    private bool hasEmail, openEmail;

    Material monitorMaterial, newMonitorMaterial;

    Texture hasMailTexture, openMailTexture;

    [SerializeField]
    Pickable catPhoto;

    public override void Use(CameraScript _cam)
    {
        
        if (!isACtive)
        {
            isACtive = true;
            monitor.SetActive(true);
            keyboard.SetActive(true);
            /*if (Storyline.hasFirstMission)
            {
                monitorMaterial.SetTexture("_MainTex", hasMailTexture);
            }
            if (Storyline.openMail)
            {
                monitorMaterial.SetTexture("_MainTex", openMailTexture);
            }*/
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
        monitorMaterial = monitor.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MonitorMaterial") as Material;
        hasMailTexture = Resources.Load<Texture>("Textures/monitorWhisMail") as Texture;
        openMailTexture = Resources.Load<Texture>("Textures/monitorWhisOpenMail") as Texture;
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
            if (!hasEmail)
            {
                monitorMaterial.SetTexture("_MainTex", hasMailTexture);
                hasEmail = true;
            }
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
            monitorMaterial.SetTexture("_MainTex", openMailTexture);
            Storyline.openMail = true;
        }
        else if (!Storyline.copyPhoto)
        {
            FindObjectOfType<InventorySystem>().AddItemToInventory(catPhoto);
            Storyline.copyPhoto = true;
        }
    }
}
