using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMap : MonoBehaviour
{
    private List<MapObject> mapObjects = new List<MapObject>();
    private List<GameObject> mapObjectsIcons = new List<GameObject>();
    [SerializeField]
    GameObject targetPlaceIcon;
    Transform targetTR;
    private Vector3 targetUpDownPos;
    [SerializeField]
    private Transform mapZeroCoordPlace;
    [SerializeField]
    Transform spaceshipTR;
    [SerializeField]
    Spaceship spaceship;

    private bool isActive;
    public bool isMapTargetinActive;

    void Start()
    {
        targetTR = targetPlaceIcon.transform;
    }

    void Update()
    {
        if (isActive)
        {
            UpdateIconPositions();
            if (isMapTargetinActive)
            {
                TargetOnMapMove();
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void ActivateMap()
    {
        foreach (MapObject _obj in FindObjectsByType<MapObject>(FindObjectsSortMode.None))
        {
            mapObjects.Add(_obj);
            mapObjectsIcons.Add(_obj.MakeMapIcon(mapZeroCoordPlace));
            UpdateIconPositions();
            isActive = true;
        }
        
        
    }

    private void UpdateIconPositions()
    {
        for (int i = 0; i < mapObjectsIcons.Count; i++)
        {
            Vector3 newIconPosition = (mapObjects[i].ObjectPosition() - spaceshipTR.position) * 0.0001f;
            newIconPosition = mapZeroCoordPlace.position + newIconPosition;
            mapObjectsIcons[i].transform.position = newIconPosition;
        }
    }

    public void DeactivateMap()
    {
        foreach (GameObject _icon in mapObjectsIcons)
        {
            Destroy(_icon);
        }
        mapObjects.Clear();
        mapObjectsIcons.Clear();
        isActive = false;
    }

    public void TargetOnMapActivate()
    {
        targetPlaceIcon.SetActive(true);
        isMapTargetinActive = true;
        
    }

    private void TargetOnMapMove()
    {
        if (!spaceship.IsMoving())
        {
            targetUpDownPos = UpdateTargetUpDown();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "map")
                {
                    targetTR.position = hit.point + spaceshipTR.up * 0.1f + targetUpDownPos;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector3 movePosition = targetTR.position - mapZeroCoordPlace.position;
                        movePosition = movePosition * 10000f;
                        movePosition = spaceshipTR.position + movePosition;
                        spaceship.SetMoveTargetPoint(movePosition);
                    }
                }
                else
                {
                    targetTR.position = spaceshipTR.position;
                }
            }
        }
        else
        {
            targetTR.position = spaceshipTR.position;
        }
    }

    private Vector3 UpdateTargetUpDown()
    {
        return targetUpDownPos + spaceshipTR.up * 0.1f * Input.GetAxis("Mouse ScrollWheel");
    }
}
