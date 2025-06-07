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


    //[HideInInspector]
    public Item item;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start()//오브젝트가 생성될때 Update보다 먼저 호출됨
    {
        //테스트 아이템 적용(테스트 아이템이 없는 경우 오류가 나 아이템매니저를 만들어서 모든 아이템에 테스트 아이템을 적용해줄 예정 아이템 저장 후 불러오기랑 비슷하게 만들거임)
        //축약: 지금은 오류나는데 필요하면 이 코드 지우고 하셈
        InitialiseItem(testitem);
    }

    public void InitialiseItem(Item newItem)
    {
        Debug.Log(newItem);
        MainWeaponSlots ma =  transform.parent.GetComponent<MainWeaponSlots>();
        item = newItem;
        if (ma != null)
        {
            image.sprite = newItem.GunImage;
        }
        else
            image.sprite = newItem.image;//여기 오류나는 이유가 Start에서 한번 더 이 함수를 호출해서 그런거임
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
