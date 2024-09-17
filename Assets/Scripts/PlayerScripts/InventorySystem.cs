using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<Pickable> itemsInInventory = new List<Pickable>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            OpenInventory();
        }
    }

    public void AddItemToInventory(Pickable _item)
    {
        itemsInInventory.Add(_item);
    }

    private void OpenInventory()
    {
        Debug.Log("В инвентаре: ");
        foreach (Pickable _item in itemsInInventory)
        {
            Debug.Log(_item.itemName);
        }
        
    }
}
