using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Health;          // ü��
    public float AttackDamage;    // ���ݷ�
    public float MoveSpeed;        // �̵��ӵ�
    public float AttackSpeed;      // ���ݼӵ�
    public float AttackRange;      // ���ݹ���

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

    public virtual void Die() // protected�� public���� ����
    {
        Debug.Log(gameObject.name + "�� �׾����ϴ�.");
        Destroy(gameObject);
    }
}
