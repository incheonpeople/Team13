using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    public Transform player; 
    public float detectionRange = 5f; // Ž�� ����
    public float moveSpeed = 20f; // �̵� �ӵ�
    public float attackRange = 2.3f; // ���� ����
    public float attackSpeed = 1f; // ���� ���� (��)
    private float lastAttackTime; // ������ ���� �ð�

    private Animator animator;
    private Rigidbody rb; 
    private enum State { Idle, Chasing, Attacking, Dead }
    private State currentState;

    public float maxHealth = 100f; 
    private float currentHealth; 

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); 
        currentState = State.Idle; 
        currentHealth = maxHealth; 
    }

    private void Update()
    {
        if (currentState == State.Dead)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chasing; 
                }
                break;

            case State.Chasing:
                if (distanceToPlayer < attackRange)
                {
                    currentState = State.Attacking;
                }
                else if (distanceToPlayer > detectionRange)
                {
                    currentState = State.Idle; 
                    animator.SetBool("RunForward", false); 
                    animator.SetBool("Idle", true); 
                }
                else
                {
                    MoveTowardsPlayer(distanceToPlayer);
                }
                break;

            case State.Attacking:
                AttackPlayer();
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chasing; 
                }
                break;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

        animator.SetBool("RunForward", true); 
        animator.SetBool("Idle", false); 
    }

    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackSpeed)
        {
            animator.SetBool("Attack3", true);
            Debug.Log("�����߽��ϴ�");

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                float damage = 10f; 
                playerScript.TakeDamage(damage); 
            }

            lastAttackTime = Time.time; 

            StartCoroutine(WaitForAttackAnimation());
        }
    }
    private IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("���Ͱ� " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        animator.SetBool("RunForward", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Attack3", false);

        animator.SetBool("Death", true); 
        currentState = State.Dead;
        Debug.Log("���Ͱ� �׾����ϴ�.");

        StartCoroutine(WaitForDeathAnimation());
    }

    private System.Collections.IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
