using UnityEngine;

public class Monster : MonoBehaviour
{
    protected float Health;          // 체력
    protected float AttackDamage;    // 공격력
    protected float MoveSpeed;        // 이동속도
    protected float AttackSpeed;      // 공격속도
    protected float AttackRange;      // 공격범위

    public virtual void Initialize(float health, float attackDamage, float moveSpeed, float attackSpeed, float attackRange)
    {
        Health = health;                  // 체력 초기화
        AttackDamage = attackDamage;      // 공격력 초기화
        MoveSpeed = moveSpeed;            // 이동속도 초기화
        AttackSpeed = attackSpeed;        // 공격속도 초기화
        AttackRange = attackRange;        // 공격범위 초기화
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;  
        Debug.Log(gameObject.name + "가 " + damage + " 데미지를 받았습니다. 현재 체력: " + Health);

        if (Health <= 0)
        {
            Die();  
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "가 죽었습니다.");
        Destroy(gameObject); 
    }
}
