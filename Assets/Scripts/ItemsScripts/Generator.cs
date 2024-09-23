using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Generator : UseType
{
    PlayerMovment player;

    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player = FindObjectOfType<PlayerMovment>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { player.InteractWithObject(usable); });
        trigger.triggers.Add(entry);
    }

    public override void Use(CameraScript _cam)
    {
        if (Storyline.needTurnOnGenerator && !Storyline.generatorIsOn)
        {
            base.Use(_cam);
        }

    }

    public void TurnGeneratorOn(Renderer buttRnd)
    {
        Storyline.generatorIsOn = true;
        buttRnd.material = Resources.Load("Materials/ButtonEnabledMat") as Material;
    }

    void Update()
    {
        
    }
}
