using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditions : MonoBehaviour
{
    [Header("HealthValue")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;

    [Header("HungerValue")]
    [SerializeField] public float hunger;
    [SerializeField] public float maxHunger;
    [SerializeField] public float autoDecreaseHunger;

    [Header("ThirstValue")]
    [SerializeField] public float thirst;
    [SerializeField] public float maxThirst;
    [SerializeField] public float autoDecreaseThirst;
    private void Start()
    {
        CharacterManager.Instance.Player.conditions = this;
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
    }
    // Update is called once per frame
    void Update()
    {
        DecreaseHunger();
        DecreaseThirst();
    }
    public void DecreaseThirst()
    {
        if (thirst > 0)
        {
            thirst -= autoDecreaseThirst * Time.deltaTime;
        }
        else
        {
            if ( hunger > 0)
            {
                hunger -= autoDecreaseThirst * Time.deltaTime;
            }
            else
            {
                health -= autoDecreaseThirst * Time.deltaTime;
            }

        }
    }
    public void DecreaseHunger()
    {
        if (hunger > 0)
        {
            hunger -= autoDecreaseHunger * Time.deltaTime;
        }
        else
        {
            health -= autoDecreaseHunger * Time.deltaTime;
        }
    }
}
