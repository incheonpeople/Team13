using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public ItemSlot[] ItemCraftingSlot;

    public CraftManager craftManager;
    public GameObject CraftingWindow;
    public Transform slotPanel;
    public Transform dropPostion;

    [Header("Select Crafting")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedCraftingStatName;
    public TextMeshProUGUI selectedCraftingStatValue;
    public GameObject CraftingButton;
    public GameObject ExitButton;

    private Player player;
    [SerializeField] private PlayerConditions conditions;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    void Start()
    {
        craftManager = GetComponent<CraftManager>();

        player = CharacterManager.Instance.Player;
        
        conditions = CharacterManager.Instance.Player.condition;
        dropPostion = CharacterManager.Instance.Player.dropPosition;
        //CharacterManager.Instance.Player.controller.addItem += Additem;
        //player.controller.inventory += Toggle;
        CraftingWindow.SetActive(false);
        ItemCraftingSlot = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < ItemCraftingSlot.Length; i++)
        {
            ItemData itemData = craftManager.GetItemData(i);
            if (itemData == null)
            {
                break;
            }

            ItemCraftingSlot[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            ItemCraftingSlot[i].index = i;
            ItemCraftingSlot[i].icon.sprite = itemData.icon;                //자료구조.몇번째 인덱스
        }
        ClearSelectedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedCraftingStatName.text = string.Empty;
        selectedCraftingStatValue.text = string.Empty;

    }
    public void SetActiveExitCraftingButton()
    {
        CraftingWindow.SetActive(false);
    }
}
