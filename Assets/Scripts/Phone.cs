using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : UseType
{
    [SerializeField]
    GameObject phoneProjection;
    [SerializeField]
    Transform playerHead;

    private bool incomingCall, waitForCall, callDiasplay;

    [SerializeField]
    private float timeBeforeCall, timeBetweenDings;

    Material[] phoneCallMaterial, phoneNoCallMaterial;

    Coroutine phoneCallCoroutine;

    [SerializeField]
    AudioSource phoneAudioSource;

    Text phoneText;

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
        if (incomingCall)
        {
            StopCoroutine(phoneCallCoroutine);
            gameObject.GetComponent<Renderer>().materials = phoneNoCallMaterial;
            incomingCall = false;
            stopUsingAccesable = false;
            phoneProjection.SetActive(true);
            phoneProjection.transform.LookAt(playerHead);
            usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
            usable.SetStopUsingAccesable(stopUsingAccesable);
            StartCoroutine(PhoneDialog(dialogTexts));
        }
    }
    
    void Start()
    {
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        waitForCall = true;
        phoneNoCallMaterial = gameObject.GetComponent<Renderer>().materials;
        phoneCallMaterial = new Material[] { phoneNoCallMaterial[0], Resources.Load<Material>("Materials/PhoneCallmaterial") as Material};
        callDiasplay = false;
        phoneText = GameObject.Find("DialogText").GetComponent<Text>();
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
            if (!callDiasplay)
            {
                gameObject.GetComponent<Renderer>().materials = phoneCallMaterial;
                callDiasplay = true;
                phoneCallCoroutine = StartCoroutine(PhoneMaterialChange());
            }
            if (timeBetweenDings > 0)
            {
                timeBetweenDings -= Time.deltaTime;
            }
            else
            {
                phoneAudioSource.Play();
                timeBetweenDings = 2f;
            }
        }
    }

    bool yellowState = true;
    private IEnumerator PhoneMaterialChange()
    {
        Color color = phoneCallMaterial[1].color;
        while (true)
        {
            if (yellowState)
            {
                for (float green = 1; green >= 0.5; green -= 0.01f)
                {
                    Debug.Log("gree");
                    color.g = green;
                    phoneCallMaterial[1].color = color;
                    yield return new WaitForSeconds(0.05f);
                }
                yellowState = false;
            }
            if (!yellowState)
            {
                for (float green = 0.5f; green <= 1; green += 0.01f)
                {
                    Debug.Log("yel");
                    color.g = green;
                    phoneCallMaterial[1].color = color;
                    yield return new WaitForSeconds(0.05f);
                }
                yellowState = true;
            }
        }
    }

    private IEnumerator PhoneDialog(string[] dialog)
    {
        phoneText.enabled = true;
        for (int i = 0; i < dialog.Length; i++)
        {
            phoneText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                phoneProjection.SetActive(false);
                Storyline.hasFirstMission = true;
                stopUsingAccesable = true;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                usable.SetAssotiatedUsingAccesable(stopUsingAccesable);
                phoneText.enabled = false;
                yield break;
            }
        }
    }
}
