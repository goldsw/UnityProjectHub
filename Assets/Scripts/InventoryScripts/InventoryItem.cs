using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image image;

    public Item testitem;


    [HideInInspector]
    public Item item;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start()
    {
        InitialiseItem(testitem);
    }

    public void InitialiseItem(Item newItem)
    {
        MainWeaponSlots ma = transform.parent.GetComponent<MainWeaponSlots>();
        item = newItem;
        if(ma != null)
        {
            image.sprite = newItem.GunImage;
        }
        else
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

}
