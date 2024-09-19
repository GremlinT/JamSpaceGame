using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Spaceship : UseType
{
    [SerializeField]
    InventorySystem invetory;
    [SerializeField]
    Transform playerTR;
    [SerializeField]
    PlayerMovment playerM;
    [SerializeField]
    Transform playerPlace;
    [SerializeField]
    private bool isActive, isLanding, isMoving, isTakeOff, isMoveToLanding, isRotating, isSpecialRotation;
    private Vector3 moveTargetPoint;
    private Quaternion specialRotation;

    [SerializeField]
    SpaceshipMap map;

    Transform TR;

    [SerializeField]
    float maxSpeed;

    [SerializeField]
    private Transform takeOffPoint, landingPoint;

    [SerializeField]
    int taregetSceneNomber;


    public override void Use(CameraScript _cam)
    {
        if (invetory.CheckInInventory(1))
        {
            base.Use(_cam);
            playerM.StopUseNavMesh();
            playerTR.SetParent(playerPlace);
            playerTR.position = playerPlace.position;
        }
        else
        {
            Debug.Log("������ ���");
            //usable.StopUsing();
        }
    }

    void Start()
    {
        TR = transform;
        SetUsable();
        stopUsingAccesable = true;
        usable.SetStopUsingAccesable(stopUsingAccesable);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (isActive)
        {
            MoveToPoint();
            if (isLanding)
            {
                Landing();
            }
            if (isTakeOff)
            {
                TakeOff();
            }
            if (isMoveToLanding)
            {
                MovingToLanding();
            }
        }
    }

    public void TurnSpaceShipOnOff()
    {
        switch (isActive)
        {
            case true:
                RaycastHit hitInfo;
                if (Physics.Raycast(TR.position, -TR.up, out hitInfo, 20f))
                {
                    moveTargetPoint = hitInfo.point;
                    takeOffPoint = hitInfo.collider.gameObject.GetComponent<landingBay>().GetTakeOffPosition();
                    isLanding = true;
                }
                break;
            case false:
                isActive = true;
                stopUsingAccesable = false;
                usable.SetStopUsingAccesable(stopUsingAccesable);
                moveTargetPoint = TR.position + TR.up * 0.5f;
                map.ActivateMap();
                break;
        }
    }

    public void AutoTakeOff()
    {
        if (takeOffPoint != null && isActive)
        {
            isTakeOff = true;
        }
    }
    public void AutoLanding()
    {
        if (landingPoint != null && isActive)
        {
            isMoveToLanding = true;
        }
    }

    private void MoveToPoint()
    {
        RotateToPointDirection();
        if (!isRotating)
        {
            if (Vector3.Distance(TR.position, moveTargetPoint) > 100f)
            {
                TR.position += (moveTargetPoint - TR.position).normalized * Time.deltaTime * maxSpeed;
                if (!isMoving) isMoving = true;
            }
            else if (Vector3.Distance(TR.position, moveTargetPoint) > 0.1f)
            {
                TR.position = Vector3.Lerp(TR.position, moveTargetPoint, Time.deltaTime);
                if (!isMoving) isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
        if (isSpecialRotation && !isMoving && !isRotating)
        {
            RotateToSpecialRotation();
        }
    }

    private void RotateToSpecialRotation()
    {
        if (Quaternion.Angle(TR.rotation, specialRotation) > 1f)
        {
            TR.rotation = Quaternion.Lerp(TR.rotation, specialRotation, Time.deltaTime);
        }
        else
        {
            isSpecialRotation = false;
        }
    }

    private void RotateToPointDirection()
    {
        if (Vector3.Distance(TR.position, moveTargetPoint) > 5f)
        {
            if (Vector3.Angle(TR.forward, moveTargetPoint - TR.position) > 1f)
            {
                if (!isRotating) isRotating = true;
                Vector3 targetVector = (moveTargetPoint - TR.position).normalized;
                TR.rotation = Quaternion.Lerp(TR.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime);
            }
            else
            {
                isRotating = false;
            }
        }
    }
    
    private void Landing()
    {
        if (!isMoving)
        {
            isActive = false;
            stopUsingAccesable = true;
            usable.SetStopUsingAccesable(stopUsingAccesable);
            isLanding = false;
            map.DeactivateMap();
        }
    }

    private void MovingToLanding()
    {
        if (!isMoving)
        {
            if (Vector3.Distance(TR.position, landingPoint.position) < 0.1 && !isRotating)
            {
                landingPoint = null;
                isMoveToLanding = false;
            }
            else
            {
                if (moveTargetPoint != landingPoint.position) moveTargetPoint = landingPoint.position;
                if (specialRotation != landingPoint.rotation) specialRotation = landingPoint.rotation;
                if (!isSpecialRotation) isSpecialRotation = true;
            }
        }
    }

    public void SetMoveTargetPoint(Vector3 _point)
    {
        if (!isMoving) moveTargetPoint = _point;
    }
    public void SetLandingPosition(Transform _position)
    {
        landingPoint = _position;
    }

    private void TakeOff()
    {
        if (!isMoving)
        {
            if (Vector3.Distance(TR.position, takeOffPoint.position) < 0.1)
            {
                takeOffPoint = null;
                isTakeOff = false;
            }
            else 
            {
                if (moveTargetPoint != takeOffPoint.position) moveTargetPoint = takeOffPoint.position;
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Jump()
    {
        SceneManager.LoadScene(taregetSceneNomber);
    }
}
