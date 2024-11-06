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
                Debug.LogError($"slotParent�� {i}��° �ڽĿ� ItemSlot ������Ʈ�� �����ϴ�.");
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
            Debug.LogError("selectedItem�� null");
            return;
        }

        if (selectedItem.craftables == null || selectedItem.craftables.Length == 0)
        {
            Debug.Log("craftables �迭�� ����ְų� null");
            return;
        }
        if (selectedItem == null)
        {
            Debug.Log("selectedItem�� null");
            return;
        }
        // 1. �ʿ��� ��� ���� �˻�
        bool allMaterialsAvailable = true; 
        foreach (var material in selectedItem.craftables)
        {
            bool foundMaterial = false;

            // �κ��丮���� ��� �˻�
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

            // ��ᰡ ���� ��� �÷��� ����
            if (!foundMaterial)
            {
                allMaterialsAvailable = false;
                break;
            }
        }

        // 2. ��ᰡ ������� ������ �޼��� ����
        if (!allMaterialsAvailable)
        {
            Debug.Log("��� ����");
            return;
        }

        // 3. ��ᰡ ����� ��� ũ������ ����
        foreach (var material in selectedItem.craftables) 
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item == material.ResourceItem)
                {
                    slots[i].quantity -= material.number;

                    // �κ��丮���� ��� ������ 0�� �Ǹ� �ش� ���� ����
                    if (slots[i].quantity <= 0)
                    {
                        slots[i].Clear();
                    }
                    break;
                }
            }
        }

        // 4. ��� ������ �κ��丮�� �߰�
        CharacterManager.Instance.Player.itemData = selectedItem;
        inventory.Additem();
        Debug.Log($"{selectedItem.displayName}��(��) ���� �Ϸ�");
    }
    public void SelectItem(int index)
    {
        if (craftItem[index].item == null) return;

        selectedItem = craftItem[index].item;
        selectedItemIndex = index;
        Debug.Log(selectedItem.displayName);
    }
}
