using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField]
    private GameObject mapObjectPref;
    [SerializeField]
    private Transform landingPosition;

    [SerializeField]
    Transform TR;
        
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject MakeMapIcon(Transform _map)
    {
        GameObject _icon = Instantiate(mapObjectPref, _map);
        _icon.GetComponent<MapIconScript>().SetLandingPosition(landingPosition.position);
        return _icon;
    }

    public Vector3 ObjectPosition()
    {
        return TR.position;
    }
}
