using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public string itemName;
    [SerializeField]
    InventorySystem inventorySystem;

    [SerializeField]
    private Usable assotiatedUsable;
    
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
        this.gameObject.SetActive(false);
    }

    public bool CheckAssotiatetUsable(Usable _usable)
    {
        if (_usable == assotiatedUsable)
        {
            return true;
        }
        else return false;
    }
}
