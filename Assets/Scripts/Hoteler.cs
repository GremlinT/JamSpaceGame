using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hoteler : UseType
{
    PlayerMovment player;
    Transform playerTR;

    Transform TR;

    Text dialogText;

    IEnumerator activeCorutine; // firstDialogCor, secondDialogCor, thirdDialogCor;

    CameraScript cam;

    NavMeshAgent agent;

    [SerializeField]
    Transform pointOfPatrol1, pointOfPatrol2, pointOfPatrol3, cabinetPoint, arestPoint;
    [SerializeField]
    Transform currentPatrolPoint;

    [SerializeField]
    Animator anim;

    [SerializeField]
    CameraTargetScript cts;

    bool isAlarmed, inCabinet, cathPlayer;


    private string[] firstDialog = new string[]
    {
        "What did you want?",
        "Um..I'm at work, I'm looking for a missing cat...",
        "I don’t know about any cats! There’s nothing like that here!",
        "Are you sure? My clients stayed with you and lost her.",
        "There are no cats and there never were, there is nothing for you to look for here!",
        "Can I at least take a look around?",
        "Don't go anywhere!",
        "...yes, a suspicious guy...",
    };
    private string[] secondDialog = new string[]
    {
        "What are you doing here! This is a closed area!",
        "I'm already leaving.",
        "Get out! I told you - there’s nothing here!",
        "Here, if you need it so much, there was no recording, there was no cat!",
        "Now get out of here!",
        "That's it, I'm leaving, I'm leaving.",
        "...he’s strange, he specifically wrote that there are no cats...",
        "But it looks like she really isn't here."
    };
    private string[] thirdDialog = new string[]
    {
        "Шеф, все пропало, ко мне пришел подозрительный тип.",
        "...невнятно...",
        "Спрашивает про кошек. Надо заканчивать.",
        "...невнято...",
        "Понял. Сегодня всех похищенных кошек привезу вам и уеду.",
        "...невнятно...",
        "Ого, да тут целый приступный синдикат!",
        "Срочно вызываю полицию!"
    };
    private string[] fourDialog = new string[]
    {
        "Вы арестованы за похищения кошек!",
        "А-а-а, я не виноват!",
        "А вам спасибо за помощь!",
        "Всегда рад.",
        "Но ту кошку что я ищу он не похищал...",
    };

    public override void Use(CameraScript _cam)
    {
        base.Use(_cam);
        agent.isStopped = true;
        dialogText.enabled = true;
        stopUsingAccesable = false;
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player.StopMove(true);
        StartCoroutine(activeCorutine);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player = FindObjectOfType<PlayerMovment>();
        cam = FindObjectOfType<CameraScript>();
        TR = transform;
        playerTR = player.transform;
        cts = GetComponent<CameraTargetScript>();
        agent = GetComponent<NavMeshAgent>();
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        activeCorutine = FirstDialog(firstDialog);
        currentPatrolPoint = pointOfPatrol1;

        cathPlayer = false;

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { player.InteractWithObject(usable); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
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
        
        if (Vector3.Distance(playerTR.position, cabinetPoint.position) < 2f && !isAlarmed)
        {
            Alarm();
        }
        if (isAlarmed && Vector3.Distance(TR.position, pointOfPatrol3.position) < 0.2f)
        {
            agent.isStopped = true;
            if (Vector3.Distance(TR.position, playerTR.position) < 3f && !cathPlayer)
            {
                cathPlayer = true;
                activeCorutine = SecondDialog(secondDialog);
                usable.Use(player);
            };
        }
        /*if (Vector3.Distance(TR.position, currentPatrolPoint.position) < 0.3f)
        {
            Patrol();
        }
        if (Vector3.Distance(playerTR.position, pointOfPatrol3.position) < 5f && Vector3.Distance(TR.position, playerTR.position) < 3f)
        {
            Alarm();
        }*/
    }
    
    private void Alarm()
    {
        isAlarmed = true;
        StartCoroutine(CheckCabinet());
    }

    private void Patrol()
    {
        if (currentPatrolPoint == pointOfPatrol1)
        {
            agent.SetDestination(pointOfPatrol2.position);
            currentPatrolPoint = pointOfPatrol2;
        }
        if (currentPatrolPoint == pointOfPatrol2)
        {
            agent.SetDestination(pointOfPatrol3.position);
            currentPatrolPoint = pointOfPatrol3;
        }
            
        if (currentPatrolPoint == pointOfPatrol3)
        {
            agent.SetDestination(pointOfPatrol1.position);
            currentPatrolPoint = pointOfPatrol1;
        }
            
    }

    private IEnumerator CheckCabinet()
    {
        dialogText.enabled = true;
        dialogText.text = "Hmm, it seems he went towards the office... we need to watch him...";
        yield return new WaitForSeconds(3f);
        dialogText.enabled = false;
        yield return new WaitForSeconds(5f);
        dialogText.enabled = true;
        dialogText.text = "Okay, I'll go check it out.";
        yield return new WaitForSeconds(3f);
        dialogText.enabled = false;
        if (agent.destination != pointOfPatrol3.position) agent.SetDestination(pointOfPatrol3.position);
        /*if (Vector3.Distance(TR.position, pointOfPatrol3.position) < 0.2f)
        {
            agent.isStopped = true;
            if (Vector3.Distance(TR.position, playerTR.position) < 3f)
            {
                activeCorutine = SecondDialog(secondDialog);
                usable.Use(player);
            }
        }*/
    }

    private IEnumerator FirstDialog(string[] dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                stopUsingAccesable = true;
                Storyline.hotreleierTolking = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                agent.isStopped = false;
                usable.StopUsing();
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
                Storyline.hotelierCathInOffice = true;
                Storyline.hotelChekedNoCat = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                //agent.isStopped = false;
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
                Storyline.hotelierDontCatch = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                dialogText.enabled = false;
                yield break;
            }
        }
    }
    private IEnumerator FourDialog(string[] dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                stopUsingAccesable = true;
                Storyline.hotelierArest = true;
                Storyline.hotelChekedNoCat = true;
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
