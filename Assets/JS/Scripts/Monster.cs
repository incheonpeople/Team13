using UnityEngine;

public class Monster : MonoBehaviour
{
    protected float Health;          // ü��
    protected float AttackDamage;    // ���ݷ�
    protected float MoveSpeed;        // �̵��ӵ�
    protected float AttackSpeed;      // ���ݼӵ�
    protected float AttackRange;      // ���ݹ���

    public virtual void Initialize(float health, float attackDamage, float moveSpeed, float attackSpeed, float attackRange)
    {
        Health = health;                  // ü�� �ʱ�ȭ
        AttackDamage = attackDamage;      // ���ݷ� �ʱ�ȭ
        MoveSpeed = moveSpeed;            // �̵��ӵ� �ʱ�ȭ
        AttackSpeed = attackSpeed;        // ���ݼӵ� �ʱ�ȭ
        AttackRange = attackRange;        // ���ݹ��� �ʱ�ȭ
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;  
        Debug.Log(gameObject.name + "�� " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + Health);

        if (Health <= 0)
        {
            Die();  
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "�� �׾����ϴ�.");
        Destroy(gameObject); 
    }
}
