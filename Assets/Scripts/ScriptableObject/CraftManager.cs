using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    [SerializeField] List<ItemData> itemDatas;

    public UIInventory inventory; 
    public ItemSlot[] craftItem = new ItemSlot[7];  
    public ItemSlot[] slots = new ItemSlot[7];  
    public Transform slotParent;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    public void Start()
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (craftItem[i] != null)
            {
                craftItem[i].item = itemDatas[i];
            }
            else
            {
                Debug.LogError($"slotParent의 {i}번째 자식에 ItemSlot 컴포넌트가 없습니다.");
            }
        }
    }

    public ItemData GetItemData(int value)
    {
        if (itemDatas.Count <= value)
        {
            return null;
        }
        return itemDatas[value];
    }

    public void Crafting()
    {
        slots = inventory.slots;
        if (selectedItem == null)
        {
            Debug.LogError("selectedItem이 null");
            return;
        }

        if (selectedItem.craftables == null || selectedItem.craftables.Length == 0)
        {
            Debug.Log("craftables 배열이 비어있거나 null");
            return;
        }
        if (selectedItem == null)
        {
            Debug.Log("selectedItem이 null");
            return;
        }
        // 1. 필요한 재료 조건 검사
        bool allMaterialsAvailable = true; 
        foreach (var material in selectedItem.craftables)
        {
            bool foundMaterial = false;

            // 인벤토리에서 재료 검색
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item == material.ResourceItem)
                {
                    if (slots[i].quantity >= material.number)
                    {
                        foundMaterial = true;
                        break;
                    }
                }
            }

            // 재료가 없는 경우 플래그 설정
            if (!foundMaterial)
            {
                allMaterialsAvailable = false;
                break;
            }
        }

        // 2. 재료가 충분하지 않으면 메서드 종료
        if (!allMaterialsAvailable)
        {
            Debug.Log("재료 부족");
            return;
        }

        // 3. 재료가 충분할 경우 크래프팅 수행
        foreach (var material in selectedItem.craftables) 
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item == material.ResourceItem)
                {
                    slots[i].quantity -= material.number;

                    // 인벤토리에서 재료 수량이 0이 되면 해당 슬롯 비우기
                    if (slots[i].quantity <= 0)
                    {
                        slots[i].Clear();
                    }
                    break;
                }
            }
        }

        // 4. 결과 아이템 인벤토리에 추가
        CharacterManager.Instance.Player.itemData = selectedItem;
        inventory.Additem();
        Debug.Log($"{selectedItem.displayName}이(가) 제작 완료");
    }
    public void SelectItem(int index)
    {
        if (craftItem[index].item == null) return;

        selectedItem = craftItem[index].item;
        selectedItemIndex = index;
        Debug.Log(selectedItem.displayName);
    }
}
