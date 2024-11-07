using UnityEngine;
using System.Collections;

public class Bear : Monster
{
    public float DetectionRange = 10f;  // 탐지 범위
    public float IdleMovementRange = 3f; 
    public float IdleMovementInterval = 3f; 

    private float _lastAttackTime;     
    private Animator _animator;         
    private Rigidbody _rb;              
    private Vector3 _targetPosition;   
    private float _idleMovementTimer;  

    private GameObject player;
    public GameObject deathPrefab;
    public enum State { Idle, Chasing, Attacking, Dead } 
    public State _currentState;   
    BearAudio bearAudio;

    private void Start()
    {
        bearAudio = GetComponent<BearAudio>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize("Bear", 100f, 10f, 10f, 2f, 3f); // 이름, 체력, 공격력, 이동속도, 공격속도, 공격범위

        _lastAttackTime = Time.time; 
        _idleMovementTimer = 0f; 
        SetNewTargetPosition(); 

        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (_currentState == State.Dead)
        {
            return; 
        }

        if (player == null)
        {
            return; 
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (_currentState)
        {
            case State.Idle:

                if (distanceToPlayer < DetectionRange)
                {
                    _currentState = State.Chasing;
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("RunForward", true);
                }
                else
                {
                    IdleMovement(); 
                }
                break;

            case State.Chasing:

                if (distanceToPlayer < AttackRange)
                {
                    _currentState = State.Attacking;
                    _animator.SetBool("RunForward", false);
                    _animator.SetBool("Attack3", true); 
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

                if (distanceToPlayer > AttackRange)
                {
                    _currentState = State.Chasing;
                    _animator.SetBool("Attack3", false); 
                }
                else
                {
                    AttackPlayer(); 
                }
                break;
        }
    }

    private void IdleMovement()
    {
        _idleMovementTimer += Time.deltaTime;

        if (_idleMovementTimer >= IdleMovementInterval)
        {
            SetNewTargetPosition();
            _idleMovementTimer = 0f; 
        }

        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0;

            _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime); 

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

            _animator.SetBool("WalkForward", true);
            _animator.SetBool("Idle", false);
        }
        else
        {
            _animator.SetBool("WalkForward", false);
            _animator.SetBool("Idle", true);
        }
    }

    private void SetNewTargetPosition()
    {
        _targetPosition = transform.position + new Vector3(
            Random.Range(-IdleMovementRange, IdleMovementRange),
            0, 
            Random.Range(-IdleMovementRange, IdleMovementRange)
        );
        _targetPosition.y = transform.position.y; 
    }

    private void MoveTowardsPlayer(float distanceToPlayer)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; 

        _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime); 

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

        _animator.SetBool("WalkForward", false);
        _animator.SetBool("RunForward", true); 
    }

    private void AttackPlayer()
    {
        if (Time.time >= _lastAttackTime + AttackSpeed)
        {
            _lastAttackTime = Time.time; 

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.controller.TakeDamage(AttackDamage);
            }

            _animator.SetBool("Attack3", true);
            StartCoroutine(ResetAttackAnimation());
            bearAudio.bearaudio();
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length); 
        _animator.SetBool("Attack3", false); 
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
        if (_currentState == State.Dead)
        {
            return;
        }

        _currentState = State.Dead;

        _animator.SetBool("Death", true);

        Debug.Log("곰이 죽었습니다.");

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity))
        {
           
            spawnPosition = hit.point + Vector3.up * 1f;  
        }

        for (int i = 0; i < 3; i++)
        {
            if (deathPrefab != null)
            {
                Instantiate(deathPrefab, spawnPosition, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}
