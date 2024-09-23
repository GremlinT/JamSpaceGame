using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent; //агент навигации - нужен
    [SerializeField]
    CameraScript cam; //камера, отдельное поведение

    private CameraTargetScript cts; //место куда убедт фокусироватьсякамера когда на игрока, отдельно
    private bool camTargetAtPlayer; //флаг что камера смотрит на игрока, отдельно

    [SerializeField]
    Animator anim; //анимато, нужен

    Transform TR; //трансформ игрока, ?

    public bool focusedOnItem;//информация что смотрит на объект, отдельно

    private Usable currentUsable; //текущий предмет, который использую, пытаюсь использовать, отдельно
    [SerializeField]
    private Pickable currentPickable; //аналогично но подбираемый, отдельно

    [SerializeField]
    Transform spaceshipTR; //трансформ корабля для финального диалога - отдельно

    Text dialogText; //оконо для текста диалогов - отделно

    bool storyEnd; //флаг что история окончилась - отдельно

    private string[] endDialog = new string[] //текст финального диалога - отдельно
    {
        "Привет! Это снова мы, твои клиенты!",
        "Да, слушаю.",
        "Тут такое дело...мы нашли кошку...",
        "О, замечательно! А где она была?",
        "Ну, мы оказывается ездели без нее...",
        "Она дома была.",
        "Но спасибо вам за помошь!",
        "....ммм...",
    };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TR = transform;
        cts = GetComponent<CameraTargetScript>();
        DontDestroyOnLoad(gameObject);
        focusedOnItem = false;

        dialogText = GameObject.Find("DialogText").GetComponent<Text>();

        storyEnd = false;

    }

    private void MoveAnimation()
    {
        if (agent.velocity != Vector3.zero)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    void Update()
    {
        MoveAnimation();
        if (!focusedOnItem)
        {
            if (currentUsable && (TR.position - currentUsable.GetUsePosition()).magnitude <= 0.1f)
            {
                focusedOnItem = true;
                camTargetAtPlayer = false;
                //if (agent.hasPath) agent.path = null;
                currentUsable.Use(this);
            }
        }
        if (currentPickable && (TR.position - currentPickable.transform.position).magnitude <= 2f)
        {
            currentPickable.TakeItem();
            currentPickable = null;
        }
        if (!focusedOnItem && !camTargetAtPlayer)
        {
            cam.SetCameraTarget(cts);
            camTargetAtPlayer = true;
        }
        if (Storyline.hotelChekedNoCat && Storyline.allCatChecked && Storyline.informationFromCafeRecived)
        {
            Debug.Log(Vector3.Distance(spaceshipTR.position, TR.position));
            if (Vector3.Distance(spaceshipTR.position, TR.position) < 6f && !storyEnd)
            {
                storyEnd = true;
                dialogText.enabled = true;
                StopMove(true);
                focusedOnItem = true;
                StartCoroutine(EndDialog(endDialog));
            }
        }
        if (Storyline.storyEnd)
        {
            Application.Quit();
        }
    }

    private IEnumerator EndDialog(string[] dialog) //корутина финалього диалога - отдельно
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogText.text = dialog[i];
            yield return new WaitForSeconds(3f);
            if (i == dialog.Length - 1)
            {
                Storyline.storyEnd = true;
                StopMove(false);
                dialogText.enabled = false;
                yield break;
            }
        }
    }

    public void MoveToPointer(BaseEventData _pointer) //метод, заставляющий двишаться к месту клика, нужно 
    {
        if (!focusedOnItem)
        {
            currentUsable = null;
            PointerEventData pointer = (PointerEventData)_pointer;
            Vector3 pointerPosition = pointer.pointerCurrentRaycast.worldPosition;
            agent.SetDestination(pointerPosition);
        }
    }

    public void InteractWithObject(Usable _usable) //метод взаимодействия с объекто или движания к нему, отдельно в части взаимодействя
    {
        if (!focusedOnItem)
        {
            if (_usable.HasUsePosition())
            {
                currentUsable = _usable;
                agent.SetDestination(_usable.GetUsePosition());
            }
            else Debug.Log("no focus point");
        }
        else
        {
            if (_usable.CheckAssotiatetUsable(currentUsable))
            {
                _usable.Use(this);
            }
        }
    }

    public void MoveToObject(Pickable _pickable) //метод аналогичен предыдущему но для подбираемых, нужность аналогично тому что выше
    {
        if (!focusedOnItem)
        {
            currentPickable = _pickable;
            agent.SetDestination(_pickable.transform.position);
        }
        else
        {
            if (_pickable.CheckAssotiatetUsable(currentUsable))
            {
                _pickable.TakeItem();
            }
        }
    }

    public void StopFocusing() //снятие фокусировки с объекта, отдельно
    {
        focusedOnItem = false;
        currentUsable = null;
        cam.SetCameraTarget(cts);
        camTargetAtPlayer = true;
    }

    public void StopUseNavMesh() //прекращение использования навмеша, ?
    {
        agent.enabled = false;
    }
    public void StartUseNavMesh() //начало использования навмеша, ?
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
    }

    public void StopMove(bool isStop) //постановка текущего движения на паузу, нужен
    {
        agent.isStopped = isStop;
    }
}
