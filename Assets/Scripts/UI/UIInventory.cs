using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject Crafting;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPostion;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;
    public GameObject fillBottleButton;
    public GameObject grilledMeatButton;
    public GameObject InCraftingButton;

    private Player player;
    [SerializeField] private PlayerConditions conditions;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    void Start()
    {
        player = CharacterManager.Instance.Player;
        conditions = CharacterManager.Instance.Player.condition;
        dropPostion = CharacterManager.Instance.Player.dropPosition;
        CharacterManager.Instance.Player.controller.addItem += Additem;
        player.controller.inventory += Toggle;
        player.controller.InteractionInventroy += SetActivetrueInteractionButton;
        player.controller.ExitInteractionInventory += SetActivefalseInteractionButton;
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelectedItemWindow();
    }

    void Update()
    {

    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
        fillBottleButton.SetActive(false);
        grilledMeatButton.SetActive(false);
    }

    //�κ��丮 ������ ���� Ȯ���ϴ� �κ�
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void Additem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        if (data.canStack)
        {
            Debug.Log("�κ��丮 ���� �߰�");
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            Debug.Log("�κ��丮 �߰�");
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        ThrowItem(data);

        CharacterManager.Instance.Player.itemData = null;
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPostion.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Consumable && slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Consumable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("���õ� �������� �����ϴ�.");
            return;
        }

        if (conditions == null)
        {
            Debug.LogError("Conditions ��ü�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.health:
                        conditions.health += selectedItem.consumables[i].value;
                        break;
                    case ConsumableType.hunger:
                        conditions.hunger += selectedItem.consumables[i].value;
                        break;
                    case ConsumableType.thirst:
                        conditions.thirst += selectedItem.consumables[i].value;
                        break;
                    case ConsumableType.speedup:
                        conditions.SpeedUp(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
        else
        {
            Debug.LogWarning("�Ҹ� ������ �������� ���ų� �߸��� Ÿ���Դϴ�.");
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    public void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void SetActivetrueInteractionButton()
    {
        grilledMeatButton.SetActive(true);
    }
    public void SetActivefalseInteractionButton()
    {
        grilledMeatButton.SetActive(false);
    }

    public void OnInteractionButton()
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("���õ� �������� �����ϴ�.");
            return;
        }

        if (conditions == null)
        {
            Debug.LogError("Conditions ��ü�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }
        if (selectedItem.type == ItemType.Interactive)
        {
            // ù ��° Interactives ����� Ÿ���� ������� ����
            switch (selectedItem.Interactives.type)
            {
                case InteractiveType.RawMeat:
                    // ������ ��ȣ�ۿ��ϴ� ����
                    CharacterManager.Instance.Player.itemData = selectedItem.Interactives.afterObj;
                    RemoveSelectedItem();
                    break;
                case InteractiveType.EmptyContainer:
                    CharacterManager.Instance.Player.itemData = selectedItem.Interactives.afterObj;
                    selectedItem = selectedItem.Interactives.afterObj;
                    RemoveSelectedItem();
                    break;
                default:
                    Debug.LogWarning("�� �� ���� InteractiveType�Դϴ�.");
                    break;
            }
            Additem();
        }
        else
        {
            Debug.LogWarning("�Ҹ� ������ �������� ���ų� �߸��� Ÿ���Դϴ�.");
        }
        
    }
    public void SetActivetrueInCraftingButton()
    {
        Crafting.SetActive(true);
    }
}
