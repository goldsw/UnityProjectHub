using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainWeaponSlots : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if(inventoryItem.item.type == Item.ItemType.Gun)
        {
            if (transform.childCount == 0)
            {
                inventoryItem.dragSetter.parentAfterDrag = transform;
            }
            if (transform.childCount >= 1)
            {
                Transform swapParent = inventoryItem.dragSetter.parentAfterDrag;
                transform.GetChild(0).transform.SetParent(swapParent);
                inventoryItem.dragSetter.parentAfterDrag = transform;


            }

        }



    }
}