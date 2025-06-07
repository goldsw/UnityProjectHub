using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public InventoryWeaponManager inventoryWeaponManager;

    public void AddItem(Item item)
    {
        if (inventoryWeaponManager.SearchEmptyWeaponSlots(item) == false)//총기류가 들어가는 슬롯을 체크해주는 매니저
        {
            SearchSlot(item);
        }

    }

    void SearchSlot(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)// 인벤토리 슬롯 형태의 배열을 탐색하여 비어있으면 소환
        {
            if(slot.transform.childCount >= 1)//슬롯에 자식으로 지정된 오브젝트가 있으면
            {
                continue;
            }
            if (SpawnItem(item, slot))
            {
                break;
            }
        }
    }

    bool SpawnItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);// 빈 UI를 빈 슬롯의 자식으로 인스턴스 하고
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();//그 UI의 inventoryItem 컴포넌트를 가져옴
        Debug.Log(item);
        inventoryItem.InitialiseItem(item);//아이템 정보를 그 UI에 붙여넣기 함
        return true;
    }


}
