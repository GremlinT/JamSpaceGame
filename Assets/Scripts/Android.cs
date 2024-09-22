using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Android : UseType
{

    PlayerMovment player;

    IEnumerator activeCorutine;

    Text dialogText;

    private string[] firstDialog = new string[]
    {
        "Good afternoon. Tell me, did you find...",
        "Hello! Visit the cat show! The best cat show in this sector of the galaxy!",
        "But I'm looking...",
        "Hello! Visit the cat show! The best cat show in this sector of the galaxy!",
        "Wait, I need...",
        "Hello! Visit the cat show! The best cat show in this sector of the galaxy!",
        "That's a nasty piece of hardware.",
        "Please don’t call me names, I work here!"
    };
    
    private string[] mainDialog = new string[]
    {
        "Hello! Visit the cat show! The best cat show in this sector of the galaxy!"
    };
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        player = FindObjectOfType<PlayerMovment>();
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        activeCorutine = FirstDialog(firstDialog);
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
                Storyline.talkingWhisAndroid = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                dialogText.enabled = false;
                activeCorutine = BaseDialog(mainDialog);
                yield break;
            }
        }
    }

    private IEnumerator BaseDialog(string[] firstDialog)
    {
        for (int i = 0; i < firstDialog.Length; i++)
        {
            dialogText.text = firstDialog[i];
            yield return new WaitForSeconds(3f);
            if (i == firstDialog.Length - 1)
            {
                stopUsingAccesable = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                player.StopMove(false);
                usable.StopUsing();
                dialogText.enabled = false;
                yield break;
            }
        }
    }

    void Update()
    {
        
    }
}
