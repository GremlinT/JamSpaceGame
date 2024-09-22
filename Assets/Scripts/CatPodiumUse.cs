using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatPodiumUse : UseType
{
    PlayerMovment player;
    Text dialogText;
    bool isCheked;
    string dialogGood = "Not similar.";
    string dialogBad = "I didn't take a photo! But they seemed to be talking about something else.";

    IEnumerator activeCorutine;

    InventorySystem inventorySystem;

    [SerializeField]
    Transform pointForItem;

    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player = FindObjectOfType<PlayerMovment>();
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        inventorySystem = player.GetComponent<InventorySystem>();
        activeCorutine = Dialog(dialogGood);
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { player.InteractWithObject(usable); });
        trigger.triggers.Add(entry);
    }

    public override void Use(CameraScript _cam)
    {
        base.Use(_cam);
        dialogText.enabled = true;
        
        stopUsingAccesable = false;
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player.StopMove(true);
        if (!isCheked)
        {
            if (inventorySystem.CheckInInventory(3))
            {
                activeCorutine = Dialog(dialogGood);
                StartCoroutine(activeCorutine);
            }
            else
            {
                activeCorutine = DialogNoFoto(dialogBad);
                StartCoroutine(activeCorutine);
            }
        }
    }
    
    private IEnumerator Dialog(string dialog)
    {
        //inventorySystem.GetItem(3, pointForItem);
        dialogText.text = dialog;
        yield return new WaitForSeconds(3f);
        stopUsingAccesable = true;
        for (int i = 0; i < 3; i++)
        {
            if (!Storyline.catCheked[i])
            {
                Storyline.catCheked[i] = true;
                break;
            }
        }
        dialogText.enabled = false;
        isCheked = true;
        int catCheked = 0;
        foreach(bool flag in Storyline.catCheked)
        {
            if (flag) catCheked++;
        }
        if (catCheked == 3) Storyline.allCatChecked = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        player.StopMove(false);
        usable.StopUsing();
        //inventorySystem.HideItem(3);
    }
    private IEnumerator DialogNoFoto(string dialog)
    {
        dialogText.text = dialog;
        yield return new WaitForSeconds(3f);
        stopUsingAccesable = true;
        for (int i = 0; i < 3; i++)
        {
            if (!Storyline.catCheked[i])
            {
                Storyline.catCheked[i] = true;
                break;
            }
        }
        dialogText.enabled = false;
        isCheked = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        player.StopMove(false);
        usable.StopUsing();
    }
    void Update()
    {
        
    }
}
