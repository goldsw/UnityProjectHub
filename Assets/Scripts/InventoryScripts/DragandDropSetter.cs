using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragandDropSetter : MonoBehaviour
{
    public Transform parentAfterDrag;

    InventoryItem inventoryItem;
    private void Start()
    {
        inventoryItem = GetComponent<InventoryItem>();
    }

    public void BeginDrag(Item item, Image image)
    {
        GameObject itemObject = inventoryItem.gameObject; //전역변수로 사용하지 말 것
        parentAfterDrag = transform.parent;
        image.raycastTarget = false;

        inventoryItem.NewSprite(item.image);
        itemObject.transform.SetParent(transform.root);
    }

    public void EndOfDrag(Item item, Image image)
    {
        GameObject itemObject = inventoryItem.gameObject;
        image.raycastTarget = true;

        itemObject.transform.SetParent(parentAfterDrag);

        if (!parentAfterDrag.GetComponent<InventorySlot>())
        {
            inventoryItem.NewSprite(item.equipImage);
        }

    }




}
