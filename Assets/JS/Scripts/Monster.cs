using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    public float Health;          // 체력
    public float AttackDamage;    // 공격력
    public float MoveSpeed;       // 이동속도
    public float AttackSpeed;     // 공격속도
    public float AttackRange;     // 공격범위
    public string MonsterName;    // 몬스터 이름
    public virtual void Initialize(string name, float health, float attackDamage, float moveSpeed, float attackSpeed, float attackRange)
    {
        MonsterName = name;                // 몬스터 이름 초기화
        Health = health;                   // 체력 초기화
        AttackDamage = attackDamage;       // 공격력 초기화
        MoveSpeed = moveSpeed;             // 이동속도 초기화
        AttackSpeed = attackSpeed;         // 공격속도 초기화
        AttackRange = attackRange;         // 공격범위 초기화
    }

    // 데미지를 입었을 때 
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log(gameObject.name + "가 " + damage + " 데미지를 받았습니다. 현재 체력: " + Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    // 몬스터가 죽었을 때 
    public virtual void Die()
    {
        Debug.Log(gameObject.name + "가 죽었습니다.");

        Destroy(gameObject);
    }
}
