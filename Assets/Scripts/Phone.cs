using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : UseType
{
    [SerializeField]
    GameObject phoneProjection;
    [SerializeField]
    Transform playerHead;

    private bool incomingCall, waitForCall;

    [SerializeField]
    private float timeBeforeCall, timeBetweenDings;

    private string[] dialogTexts = new string[]
    {
        "Привет! Вы детектив, да? Можете нам помочь?",
        "Да, что у вас случилось?",
        "У нас пропала кошка! Самая любимая!",
        "Как это произошло?",
        "Мы были в путешествии по галактике,  она где то потерялась.",
        "Я найду ее! Пришлите мне ее фото и ваш маршрут.",
        "Спасибо! Все выслал на почту!"
    };

    public override void Use(CameraScript _cam)
    {
        incomingCall = false;
        phoneProjection.SetActive(true);
        phoneProjection.transform.LookAt(playerHead);
        usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
        StartCoroutine(PhoneDialog(dialogTexts));
    }
    
    void Start()
    {
        SetUsable();
        stopUsingAccesable = false;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        waitForCall = true;
    }

    void Update()
    {
        if (!incomingCall && waitForCall)
        {
            if (timeBeforeCall > 0)
            {
                timeBeforeCall -= Time.deltaTime;
            }
            else
            {
                incomingCall = true;
                waitForCall = false;
            }
        }
        if (incomingCall)
        {
            if (timeBetweenDings > 0)
            {
                timeBetweenDings -= Time.deltaTime;
            }
            else
            {
                Debug.Log("DING!");
                timeBetweenDings = 2f;
            }
        }
    }

    private IEnumerator PhoneDialog(string[] dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            Debug.Log(dialog[i]);
            if (i == dialog.Length - 1)
            {
                phoneProjection.SetActive(false);
                Storyline.hasFirstMission = true;
                stopUsingAccesable = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                yield break;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
