using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrainState
{
    work,
    rest
}
public class AlienBrain : MonoBehaviour
{
    private AlienStateMashine stateMashine;

    [SerializeField]
    Transform placeForRest;

    [SerializeField]
    UsableItem workItem;

    [SerializeField]
    float workingTime, restingTime;

    private float timer;
    private BrainState currentBrainState;

    void Start()
    {
        stateMashine = GetComponent<AlienStateMashine>();
        timer = restingTime;
        currentBrainState = BrainState.rest;
    }

    private void BrainMashine()
    {
        switch (currentBrainState)
        {
            case BrainState.work:
                if (timer <= 0)
                {
                    timer = restingTime;
                    stateMashine.StopUseItem();
                    stateMashine.SetDestination(placeForRest.position);
                    currentBrainState = BrainState.rest;
                }
                break;
            case BrainState.rest:
                if (timer <= 0)
                {
                    timer = workingTime;
                    stateMashine.UseItem(workItem);
                    currentBrainState = BrainState.work;
                }
                break;
            default:
                break;
        }
    }
    void Update()
    {
        timer -= Time.deltaTime;
        BrainMashine();
    }
}
