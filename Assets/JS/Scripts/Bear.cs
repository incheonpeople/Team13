using UnityEngine;
using System.Collections;

public class Bear : Monster
{
    public Transform Player;
    public float DetectionRange = 10f;
    public float IdleMovementRange = 3f; 
    public float IdleMovementInterval = 3f; 

    private float _lastAttackTime;
    private Animator _animator;
    private Rigidbody _rb;
    private enum State { Idle, Chasing, Attacking, Dead, Wandering }
    private State _currentState;

    private Vector3 _wanderTarget;
    private float _wanderTimer;
    private float _wanderDuration = 2f; 
    private float _wanderTimeElapsed;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize(100f, 10f, 20f, 1f, 2.3f); 

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
                _animator.SetBool("WalkForward", false); 
                if (distanceToPlayer < DetectionRange)
                {
                    _currentState = State.Chasing;  // 추적
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
                _animator.SetBool("WalkForward", false); 
                _animator.SetBool("RunForward", true); 
                if (distanceToPlayer < AttackRange)
                {
                    _currentState = State.Attacking;
                }
                else if (distanceToPlayer > DetectionRange)
                {
                    _currentState = State.Idle; 
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
                    _currentState = State.Chasing; // 다시 추적
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

        _animator.SetBool("WalkForward", true); 
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

        _animator.SetBool("WalkForward", false); 
        _animator.SetBool("RunForward", true);
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
                playerScript.controller.TakeDamage(AttackDamage);
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

    public override void Die()
    {
        _animator.SetBool("RunForward", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("WalkForward", false);
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
