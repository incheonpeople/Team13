using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

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
    public GameObject DrinkingButton;

    private Player player;
    private PlayerConditions conditions;


    void Start()
    {
        player = CharacterManager.Instance.Player;
        conditions = player.conditions;
        dropPostion = CharacterManager.Instance.Player.dropItemPos;
        player.inventory += Toggle;

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
        DrinkingButton.SetActive(false);
    }

    //인벤토리 열리게 할지 확인하는 부분
    public void Toggle()
    {
        if(IsOpen())
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
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
        }
        ThrowItem(data);

        CharacterManager.Instance.Player.itemData = null;
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if(slots[i] != null)
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
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == data && slots[i].quantity < data.maxStackAmount)
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
        Instantiate(data.dropPrefab, dropPostion.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
}
