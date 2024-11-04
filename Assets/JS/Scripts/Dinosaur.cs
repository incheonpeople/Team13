using UnityEngine;
using System.Collections;

public class Dinosaur : Monster
{
    public Transform Player;
    public float DetectionRange = 12f;
    public float IdleMovementRange = 5f;
    public float IdleMovementInterval = 4f;

    private float _lastAttackTime;
    private Animator _animator;
    private Rigidbody _rb;
    private enum State { Idle, Chasing, Attacking, Dead, Wandering }
    private State _currentState;

    private Vector3 _wanderTarget;
    private float _wanderTimer;
    private float _wanderDuration = 5f; 
    private float _wanderTimeElapsed;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize(500f, 50f, 30f, 1f, 4.5f); // 체력, 공격력, 이동속도, 공격속도, 공격범위

        SetWanderTarget();
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
                _animator.SetBool("Idle", true);
                _animator.SetBool("Walk", false);
                if (distanceToPlayer < DetectionRange)
                {
                    _currentState = State.Chasing;  
                }
                else
                {
                    _wanderTimer -= Time.deltaTime;
                    if (_wanderTimer <= 0)
                    {
                        _currentState = State.Wandering;
                        _wanderTimeElapsed = 0; 
                    }
                }
                break;

            case State.Wandering:
                Wander();
                _wanderTimeElapsed += Time.deltaTime;

                if (_wanderTimeElapsed >= _wanderDuration)
                {
                    _currentState = State.Idle; 
                }
                break;

            case State.Chasing:
                _animator.SetBool("Idle", false);
                _animator.SetBool("Walk", false);
                _animator.SetBool("Run", true);
                if (distanceToPlayer < AttackRange)
                {
                    _currentState = State.Attacking;
                }
                else if (distanceToPlayer > DetectionRange)
                {
                    _currentState = State.Idle; 
                    _animator.SetBool("Run", false);
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
                    _currentState = State.Chasing;
                }
                break;
        }
    }

    private void Wander()
    {
        MoveTowardsWanderTarget();

        if (Vector3.Distance(transform.position, _wanderTarget) < 0.5f)
        {
            SetWanderTarget();
        }
    }

    private void MoveTowardsWanderTarget()
    {
        Vector3 direction = (_wanderTarget - transform.position).normalized;
        direction.y = 0;

        _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

        _animator.SetBool("Walk", true);
        _animator.SetBool("Idle", false);
    }

    private void SetWanderTarget()
    {
        _wanderTarget = transform.position + new Vector3(Random.Range(-IdleMovementRange, IdleMovementRange), 0, Random.Range(-IdleMovementRange, IdleMovementRange));
        _wanderTimer = IdleMovementInterval; 
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        direction.y = 0;

        _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

        _animator.SetBool("Walk", false);
        _animator.SetBool("Run", true);
    }

    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= AttackRange && Time.time >= _lastAttackTime + AttackSpeed)
        {
            _animator.SetBool("Attack", true);
            _animator.SetBool("Run", false);
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
        _animator.SetBool("Attack", false); 
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
        _animator.SetBool("Run", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Death", true);

        _currentState = State.Dead;
        Debug.Log("공룡이 죽었습니다.");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(2.7f);
        Destroy(gameObject);
    }
}
