using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Structure,
    Consumable,
    Equipable
}
public enum ConsumableType
{
    health,
    hunger,
    thirst,
    speedup
}

public enum EquipableType
{
    Damaga,
    Defend,
    Speed
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[Serializable]
public class ItemDataEquipable
{
    public EquipableType type;
    public float value;
}

[CreateAssetMenu (fileName = "Item", menuName = "NewItem")]
public class ItemData : ScriptableObject
{
    [Header ("info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equipable")]
    public ItemDataEquipable[] equipables; 
}
