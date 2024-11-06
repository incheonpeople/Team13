using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Outline outline;
    public UIInventory inventory;
    public CraftManager Crafting;

    public int index;
    public bool equipped = false;
    public int quantity;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }
    private void OnEnable()
    {
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped; 
        }
    }
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);       
        quantityText.text = string.Empty;
    }
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }

    public void OnCraftingButton()
    {
        Crafting.SelectItem(index);
    }
}
