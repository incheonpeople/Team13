using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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

    Player controller;
    private void Start()
    {
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

    private void Awake()
    {
        controller = GetComponent<Player>();
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
            else if(health > 0)
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
        else if (health > 0)
        {
            health -= autoDecreaseHunger * Time.deltaTime;
        }
    }
    public void SpeedUp(float amount)
    {
        controller.MoveSpeed += (amount);

        Invoke("SpeedReturn", 10f);
    }

    public void SpeedReturn()
    {
        controller.MoveSpeed = controller.BaseSpeed;
        //movespeed값을 원래 상태로 돌려줌
    }

}
