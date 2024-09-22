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
    NavMeshAgent agent;
    [SerializeField]
    CameraScript cam;

    private CameraTargetScript cts;
    private bool camTargetAtPlayer;

    [SerializeField]
    Animator anim;

    Transform TR;

    public bool focusedOnItem;

    private Usable currentUsable;
    [SerializeField]
    private Pickable currentPickable;

    [SerializeField]
    Transform spaceshipTR;

    Text dialogText;

    bool storyEnd;

    private string[] endDialog = new string[]
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

    private IEnumerator EndDialog(string[] dialog)
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

    public void MoveToPointer(BaseEventData _pointer) 
    {
        if (!focusedOnItem)
        {
            currentUsable = null;
            PointerEventData pointer = (PointerEventData)_pointer;
            Vector3 pointerPosition = pointer.pointerCurrentRaycast.worldPosition;
            agent.SetDestination(pointerPosition);
        }
    }

    public void InteractWithObject(Usable _usable)
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

    public void MoveToObject(Pickable _pickable)
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

    public void StopFocusing()
    {
        focusedOnItem = false;
        currentUsable = null;
        cam.SetCameraTarget(cts);
        camTargetAtPlayer = true;
    }

    public void StopUseNavMesh()
    {
        agent.enabled = false;
    }
    public void StartUseNavMesh()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
    }

    public void StopMove(bool isStop)
    {
        agent.isStopped = isStop;
    }
}
