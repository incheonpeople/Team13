using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public interface IDamagable // [Add] �ܺ��ۿ뿡 ���� �޴� �������� ���� �߰�
{
    void TakePhysicalDamage(int damage);
}
public class PlayerConditions : MonoBehaviour, IDamagable
{ 

    [Header("HealthValue")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;

    [Header("DefenseValue")] // [Add] ���� �߰�
    [SerializeField] public int defense;

    [Header("HungerValue")]
    [SerializeField] public float hunger;
    [SerializeField] public float maxHunger;
    [SerializeField] public float autoDecreaseHunger;

    [Header("ThirstValue")]
    [SerializeField] public float thirst;
    [SerializeField] public float maxThirst;
    [SerializeField] public float autoDecreaseThirst;

    PlayerController controller;
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
        controller = GetComponent<PlayerController>();
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
                hunger -= autoDecreaseThirst * 0.5f * Time.deltaTime;
            }
            else if(health > 0)
            {
                health -= autoDecreaseThirst * 0.1f * Time.deltaTime;
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
            health -= autoDecreaseHunger * 0.1f * Time.deltaTime;
        }
    }
    public void SpeedUp(float amount)
    {
        controller.moveSpeed += (amount);

        Invoke("SpeedReturn", 10f);
    }

    public void SpeedReturn()
    {
        controller.moveSpeed = controller.baseSpeed;
        //movespeed���� ���� ���·� ������
    }

    public void TakePhysicalDamage(int damage) // [Add] �ܺ��ۿ뿡 ���� �޴� �������� ���� �߰�
    { 
        if (defense < 5)
        {
            int remainingDamage = damage - defense; // ������ ���������� ���� �� ���� ������ ���

            if (remainingDamage > 0)
            {
                health -= remainingDamage;
            }

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }
    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
