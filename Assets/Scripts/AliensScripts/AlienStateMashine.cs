using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public enum AlienState //�������, � ������� ����� ���������� �������������
{
    idle,
    move,
    useItem
}

public class AlienStateMashine : MonoBehaviour
{
    private Alien alien;
    private Transform alienTR;
    [SerializeField]
    private AlienState currentState; //������� ������
    [SerializeField]
    private Vector3 destination; //�����, ���� ���� ���������
    [SerializeField]
    private UsableItem currentItem; //������� �������, � ������� ���� ��� �� ������
    
    [SerializeField]
    private float minDestination;

    void Start() 
    {
        alien = GetComponent<Alien>();
        alienTR = alien.transform;
    }

    private void StateMashine()
    {
        switch (currentState)
        {
            case AlienState.idle: //���� ������� ������ "������ �� ������"
                if (destination != Vector3.zero)
                {
                    currentState = AlienState.move;
                    break;
                }
                if (currentItem != null)
                {
                    if (currentItem.IsNear(alienTR))
                    {
                        if (currentItem.StartUse(alien))
                        {
                            currentState = AlienState.useItem;
                        }
                        break;
                    }
                    else
                    {
                        destination = currentItem.GetUsePoint();
                    }
                }
                break;
            case AlienState.move: //���� ������� ������ "���"
                if (destination == Vector3.zero)
                {
                    alien.StopMove();
                    currentState = AlienState.idle;
                    break;
                }
                if (Vector3.Distance(alienTR.position, destination) < minDestination)
                {
                    alien.StopMove();
                    destination = Vector3.zero;
                }
                else
                {
                    alien.Move(destination);
                }
                break;
            case AlienState.useItem: //���� ������� ������ "��������� �������"
                if (currentItem == null)
                {
                    currentState = AlienState.idle;
                    break;
                }
                else if (destination != Vector3.zero)
                {
                    currentState = AlienState.move;
                    break;
                }
                break;
            default:
                break;
        }
    }

    public void SetDestination(Vector3 _destination)
    {
        destination = _destination;
        if (currentItem) StopUseItem();
    }
    public void UseItem(UsableItem _item)
    {
        if (currentItem != _item)
        {
            StopUseItem();
        }
        if (!currentItem)
        {
            currentItem = _item;
            destination = currentItem.GetUsePoint();
        }
    }
    public void StopUseItem()
    {
        if (currentItem)
        {
            if(currentItem.StopUse(alien)) currentItem = null;
        }
    }

    void Update()
    {
        StateMashine();
    }
}
