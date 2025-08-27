using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainWeaponSlots : MonoBehaviour, IDropHandler
{
    public Item.ItemType item_type;
    public void OnDrop(PointerEventData eventData)
    {

        InventoryItem grabItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        Transform swapParent = grabItem.dragSetter.parentAfterDrag;


        if (grabItem.item.type == item_type || grabItem.GetComponent<InventorySlot>())
        {
            if (transform.childCount == 0)
            {
                grabItem.dragSetter.parentAfterDrag = transform;
            }
            if (transform.childCount >= 1)
            {
                Transform equidedGun = transform.GetChild(0);
                InventoryItem equidedinfo = equidedGun.GetComponent<InventoryItem>();

                if (grabItem.item.type == equidedinfo.item.type)
                {
                    //장착되있던 아이템이 빠져나감
                    equidedinfo.dragSetter.parentAfterDrag = swapParent;
                    equidedGun.transform.SetParent(swapParent);
                    equidedinfo.dragSetter.CheckSlots(equidedinfo.item);

                    //마우스가 갖고있던 
                    grabItem.dragSetter.parentAfterDrag = transform;
                    Debug.Log(equidedGun.name);
                    Debug.Log(equidedinfo.image.sprite);
                }



            }

        }



    }
}