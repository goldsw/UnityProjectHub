using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWeaponManager : MonoBehaviour
{
    public GameObject inventoryItemPrefab;
    public MainWeaponSlots[] WeaponSlots;
    // Start is called before the first frame update
    void Start()
    {
    }

    public bool SearchEmptyWeaponSlots(Item item)
    {
        if (item.type == Item.ItemType.Gun)
        {
            if (item.actionType == Item.ActionType.Main_Weapon && WeaponSlots[0].transform.childCount <= 0)
            {
                SpawnItem(item, WeaponSlots[0]);
                return true;
            }
            if (item.actionType == Item.ActionType.Secondary_Weapon && WeaponSlots[1].transform.childCount <= 0)
            {
                SpawnItem(item, WeaponSlots[1]);
                return true;
            }
        }
        return false;
    }

    void SpawnItem(Item item, MainWeaponSlots slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);

    }



}
