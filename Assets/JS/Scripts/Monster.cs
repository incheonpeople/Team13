using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    public float Health;          // ü��
    public float AttackDamage;    // ���ݷ�
    public float MoveSpeed;       // �̵��ӵ�
    public float AttackSpeed;     // ���ݼӵ�
    public float AttackRange;     // ���ݹ���
    public string MonsterName;    // ���� �̸�
    public virtual void Initialize(string name, float health, float attackDamage, float moveSpeed, float attackSpeed, float attackRange)
    {
        MonsterName = name;                // ���� �̸� �ʱ�ȭ
        Health = health;                   // ü�� �ʱ�ȭ
        AttackDamage = attackDamage;       // ���ݷ� �ʱ�ȭ
        MoveSpeed = moveSpeed;             // �̵��ӵ� �ʱ�ȭ
        AttackSpeed = attackSpeed;         // ���ݼӵ� �ʱ�ȭ
        AttackRange = attackRange;         // ���ݹ��� �ʱ�ȭ
    }

    // �������� �Ծ��� �� 
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log(gameObject.name + "�� " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    // ���Ͱ� �׾��� �� 
    public virtual void Die()
    {
        Debug.Log(gameObject.name + "�� �׾����ϴ�.");

        Destroy(gameObject);
    }
}
