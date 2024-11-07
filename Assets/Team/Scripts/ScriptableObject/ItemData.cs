using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Structure,
    Consumable,
    Equipable,
    Interactive,
    Craftable
}
public enum Craftable
{
    Yes,
    No
}
public enum InteractiveType
{
    RawMeat,        // 생고기용
    EmptyContainer  // 빈 물통용
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
public class ItemDataInteractive
{
    public InteractiveType type;
    public ItemData afterObj;
}

[Serializable]
public class ItemDataEquipable
{
    public EquipableType type;
    public float value;
}

[Serializable]
public class ItemCraftable
{
    public Craftable type;
    public ItemData ResourceItem;
    public int number;
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

    [Header("Interactives")]
    public ItemDataInteractive Interactives;

    [Header("Equipable")]
    public ItemDataEquipable[] equipables;
    public GameObject EquipPrefab;

    [Header("Structure")]
    public GameObject StructruePrefab;

    [Header("Craftable")]
    public ItemCraftable[] craftables;
}
