using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public int itemID;
    [SerializeField]
    private string itemName;
    [SerializeField]
    InventorySystem inventorySystem;

    [SerializeField]
    private Usable assotiatedUsable;

    [SerializeField]
    private bool wearing;
    [SerializeField]
    Transform wearingPlace;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeItem()
    {
        inventorySystem.AddItemToInventory(this);
        if (!wearing)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Wear();
        }
    }

    public bool CheckAssotiatetUsable(Usable _usable)
    {
        if (_usable == assotiatedUsable)
        {
            return true;
        }
        else return false;
    }

    private void Wear()
    {
        Transform itemTR = transform;
        itemTR.SetParent(wearingPlace);
        itemTR.position = wearingPlace.position;
        itemTR.rotation = wearingPlace.rotation;
    }

    public string ItemInformation()
    {
        return itemName;
    }

}
