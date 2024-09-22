using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BarmenScript : UseType
{
    PlayerMovment player;

    Transform TR;

    Text dialogText;

    IEnumerator activeCorutine; // firstDialogCor, secondDialogCor, thirdDialogCor;

    CameraScript cam;

    NavMeshAgent agent;

    [SerializeField]
    Transform workingPlace, secondCameraPlace;

    [SerializeField]
    Animator anim;

    [SerializeField]
    CameraTargetScript cts;

    bool isFree;

    private string[] firstDialog = new string[]
    {
        "Hey, is there someone there? Hey!",
        "Who's here?",
        "I'm a local bartender! I'm stuck in the back room! Help me get out!",
        "Hmm, what happened?",
        "The lights went out and the droid blocked the door. Get him away from the door.",
        "Okay, but how?",
        "It has a button, press it and it will go charging.",
        "I'll do everything now."
    };
    private string[] secondDialog = new string[]
    {
        "Ugh! Thank you!",
        "You're welcome. I'm here on business.",
        "Yeah, anything!",
        "I’m looking for a cat, its owners visited you the other day.",
        "Not many people come here, but I definitely haven’t seen any cats.",
        "I can still check with the cameras, but there is a problem.",
        "There’s still no light, we need to turn on the generator.",
        "Let's turn it on, what needs to be done and where is it?",
        "Oh, everything is simple, you just need to press the button.",
        "And the generator is in the back room, the second door from the hall, so you won’t get lost.",
        "I’ll turn it on in a minute."
    };
    private string[] thirdDialog = new string[]
    {
        "Thank you for your help! I looked at the cameras.",
        "There were really few guests over the last few days, and there were no cats with them at all",
        "They probably didn’t lose her here, maybe somewhere earlier.",
        "Thank you! I’ll go continue my search.",
        "Good luck to you! You will find her, I'm sure!",
        "Thank you!"
    };

    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player = FindObjectOfType<PlayerMovment>();
        cam = FindObjectOfType<CameraScript>();
        TR = transform;
        cts = GetComponent<CameraTargetScript>();
        agent = GetComponent<NavMeshAgent>();
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        activeCorutine = FirstDialog(firstDialog);
    }

    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }

        if (IsPlayerNear() && !Storyline.findBartentet)
        {
            Storyline.findBartentet = true;
            usable.Use(player);
        }
        if (Storyline.repearDroid && !isFree) 
        {
            agent.SetDestination(workingPlace.position);
            isFree = true;
        }
        if (isFree && !Storyline.needTurnOnGenerator)
        {
            if (Vector3.Distance(TR.position, workingPlace.position) < 0.1f)
            {
                agent.isStopped = true;
                activeCorutine = SecondDialog(secondDialog);
                usable.Use(player);
            }
        }
        if (IsPlayerNear() && Storyline.generatorIsOn && !Storyline.informationFromCafeRecived)
        {
            agent.isStopped = true;
            activeCorutine = ThirdDialog(thirdDialog);
            usable.Use(player);
        }
    }

    private bool IsPlayerNear()
    {
        float dist = Vector3.Distance(player.transform.position, TR.position);
        if (dist < 5f) return true;
        else return false;
    }

    public override void Use(CameraScript _cam)
    {
        base.Use(_cam);
        dialogText.enabled = true;
        stopUsingAccesable = false;
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player.StopMove(true);
        StartCoroutine(activeCorutine);
    }

    private IEnumerator FirstDialog(string[] firstDialog)
    {
        for (int i = 0; i < firstDialog.Length; i++)
        {
            dialogText.text = firstDialog[i];
            yield return new WaitForSeconds(3f);
            if (i == firstDialog.Length - 1)
            {
                stopUsingAccesable = true;
                Storyline.needRepearDroid = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                cts.cameraPoint = secondCameraPlace;
                dialogText.enabled = false;
                yield break;
            }
        }
    }

    private IEnumerator SecondDialog(string[] dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                stopUsingAccesable = true;
                Storyline.needTurnOnGenerator = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                dialogText.enabled = false;
                yield break;
            }
        }
    }

    private IEnumerator ThirdDialog(string[] dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                stopUsingAccesable = true;
                Storyline.informationFromCafeRecived = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                dialogText.enabled = false;
                yield break;
            }
        }
    }
}
