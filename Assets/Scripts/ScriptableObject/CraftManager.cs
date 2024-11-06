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
    public ItemSlot[ ] slots;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    public void Start()
    {

    }

    public ItemData GetItemData(int value)
    {
        if (itemDatas.Count <= value)
        {
            return null;
        }
        return itemDatas[value];
    }

    public void CraftingAx()
    {
        slots = inventory.slots;
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    if (slots[i].item != null)
        //    {
        //        if (selectedItem.type == ItemType.Craftable)
        //        {
        //            for (int i = 0; i < selectedItem.consumables.Length; i++)
        //            {
        //                switch (selectedItem.consumables[i].type)
        //                {
        //                    case ConsumableType.health:
        //                        conditions.health += selectedItem.consumables[i].value;
        //                        break;
        //                    case ConsumableType.hunger:
        //                        conditions.hunger += selectedItem.consumables[i].value;
        //                        break;
        //                    case ConsumableType.thirst:
        //                        conditions.thirst += selectedItem.consumables[i].value;
        //                        break;
        //                    case ConsumableType.speedup:
        //                        conditions.SpeedUp(selectedItem.consumables[i].value);
        //                        break;
        //                }
        //            }
        //            inventory.RemoveSelectedItem();
        //        }
        //    }

        //    else
        //    {
        //        slots[i].Clear();
        //    }
        //}
        //if (inventory != null)
        //{
        //    selectedItem = slots[index].item;
        //}

    }
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

    }




}
