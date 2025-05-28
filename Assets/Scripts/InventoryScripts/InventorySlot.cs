using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if(transform.childCount == 0)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        if(transform.childCount >= 1)
        {
            Transform swapParent = inventoryItem.parentAfterDrag;
            transform.GetChild(0).transform.SetParent(swapParent);
            inventoryItem.parentAfterDrag = transform;


        }
    }
}
