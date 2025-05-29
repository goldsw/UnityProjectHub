using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public InventorySlot[] gunSlots;
    public InventorySlot[] itemSlots;
    public GameObject inventoryItemPrefab;

    public void AddItem(Item item)
    {
        if(item.type == Item.ItemType.Gun)
        {
            if(item.actionType == Item.ActionType.Main_Weapon && gunSlots[0].transform.childCount <= 0)
            {
                SpawnItem(item, gunSlots[0]);
                return;
            }
            if (item.actionType == Item.ActionType.Secondary_Weapon && gunSlots[1].transform.childCount <= 0)
            {
                SpawnItem(item, gunSlots[1]);
                return;
            }
        }
        SearchSlot(item);

    }

    void SearchSlot(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if(slot.transform.childCount <= 0)
            {
                SpawnItem(item, slot);
}
        }
    }

    void SpawnItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);

    }


}
