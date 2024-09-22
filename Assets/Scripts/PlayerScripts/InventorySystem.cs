using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private List<Pickable> itemsInInventory = new List<Pickable>();
    [SerializeField]
    Image[] iconsPlaces;
    [SerializeField]
    Text ItemInfo;

    void Start()
    {

    }

    void Update()
    {

    }

    public void AddItemToInventory(Pickable _item)
    {
        itemsInInventory.Add(_item);
        iconsPlaces[itemsInInventory.IndexOf(_item)].sprite = _item.GetIcon();
        iconsPlaces[itemsInInventory.IndexOf(_item)].enabled = true;
        /*foreach (Image imagePlace in iconsPlaces)
        {
            if (!imagePlace.sprite)
            {
                imagePlace.sprite = _item.GetIcon();
                imagePlace.enabled = true;
                break;
            }
        }*/
    }

    private void OpenInventory()
    {
        Debug.Log("В инвентаре: ");
        foreach (Pickable _item in itemsInInventory)
        {
            Debug.Log(_item.ItemInformation());
        }
    }

    public void GeiItemInfo(int _index)
    {
        ItemInfo.text = itemsInInventory[_index].ItemInformation();
        ItemInfo.enabled = true;
    }

    public void TurnItemInfoOff()
    {
        ItemInfo.enabled = false;
    }

    public bool CheckInInventory(int _ID)
    {
        foreach (var item in itemsInInventory)
        {
            if (item.itemID == _ID)
            {
                return true;
            }
        }
        return false;
    }

    public void GetItem(int _ID, Transform pointForItem)
    {
        foreach (var item in itemsInInventory)
        {
            if (item.itemID == _ID)
            {
                item.gameObject.SetActive(true);
                item.gameObject.transform.position = pointForItem.position;
                item.gameObject.transform.rotation = pointForItem.rotation;
                break;
            }
        }
    }

    public void HideItem(int _ID)
    {
        foreach (var item in itemsInInventory)
        {
            if (item.itemID == _ID)
            {
                item.gameObject.SetActive(false);
                break;
            }
        }
    }
}
