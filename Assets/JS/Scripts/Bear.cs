using UnityEngine;
using System.Collections;

public class Bear : Monster
{
    public Transform Player;
    public float DetectionRange = 10f;
    private float _lastAttackTime;
    private Animator _animator;
    private Rigidbody _rb;
    private enum State { Idle, Chasing, Attacking, Dead }
    private State _currentState;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize(100f, 10f, 20f, 1f, 2.3f); // 체력, 공격력, 이동속도, 공격속도, 공격범위
    }

    private void Update()
    {
        if (_currentState == State.Dead)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        switch (_currentState)
        {
            case State.Idle:
                if (distanceToPlayer < DetectionRange)
                {
                    _currentState = State.Chasing;  // 추적
                }
                break;

            case State.Chasing:
                if (distanceToPlayer < AttackRange)
                {
                    _currentState = State.Attacking;
                }
                else if (distanceToPlayer > DetectionRange)
                {
                    _currentState = State.Idle;  // 대기 
                    _animator.SetBool("RunForward", false);
                    _animator.SetBool("Idle", true);
                }
                else
                {
                    MoveTowardsPlayer(distanceToPlayer);
                }
                break;

            case State.Attacking:
                AttackPlayer();
                if (distanceToPlayer > AttackRange)
                {
                    _currentState = State.Chasing;  // 다시 추적
                }
                break;
        }
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        direction.y = 0;

        _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

        _animator.SetBool("RunForward", true);
        _animator.SetBool("Idle", false);
    }

    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= AttackRange && Time.time >= _lastAttackTime + AttackSpeed)
        {
            _animator.SetBool("Attack3", true);
            Debug.Log("공격했습니다");

            Player playerScript = Player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(AttackDamage); 
            }

            _lastAttackTime = Time.time;
            StartCoroutine(WaitForAttackAnimation());
        }
    }

    private IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);  

        if (Health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        _animator.SetBool("RunForward", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Attack3", false);
        _animator.SetBool("Death", true);

        _currentState = State.Dead;
        Debug.Log("곰이 죽었습니다.");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
