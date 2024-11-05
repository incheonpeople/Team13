using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable // [Add] 외부작용에 의해 받는 데미지를 위해 추가
{
    void TakePhysicalDamage(int damage);
}
public class PlayerConditions : MonoBehaviour, IDamagable
{ 

    [Header("HealthValue")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;

    [Header("DefenseValue")] // [Add] 방어력 추가
    [SerializeField] public int defense;

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
                hunger -= autoDecreaseThirst * 0.5f * Time.deltaTime;
            }
            else
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
        else
        {
            health -= autoDecreaseHunger * 0.1f * Time.deltaTime;
        }
    }
    public void TakePhysicalDamage(int damage) // [Add] 외부작용에 의해 받는 데미지를 위해 추가
    { 
        if (defense < 5)
        {
            int remainingDamage = damage - defense; // 방어력이 데미지보다 적을 때 남은 데미지 계산

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
