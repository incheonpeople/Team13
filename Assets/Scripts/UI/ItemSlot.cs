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

    public int index;
    public bool equipped;
    public int quantity;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set()
    {
    }
    public void Clear()
    {

    }
}
