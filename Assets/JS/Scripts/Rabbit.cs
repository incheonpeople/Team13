using UnityEngine;
using System.Collections;

public class Rabbit : Monster
{
    public Transform Player;
    public float DetectionRange = 4f;
    public float IdleMovementRange = 3f;
    public float IdleMovementInterval = 3f;
    public float EscapeSpeed = 10.0f;
    public float PushBackForce = 5f;
    public float LiftForce = 2f;

    private Animator _animator;
    private Rigidbody _rb;
    private enum State { Idle, Running, Dead }
    private State _currentState;
    private Vector3 _targetPosition;
    private float _idleMovementTimer = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize("Rabbit", 10f, 0f, 5f, 0f, 0f);  // 체력, 공격력, 이동속도, 공격속도, 공격범위

        SetAnimatorParameters(State.Idle);
        _targetPosition = transform.position; 
    }

    private void Update()
    {
        if (_currentState == State.Dead)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer < DetectionRange)
        {
            if (_currentState != State.Running)
            {
                _currentState = State.Running;
                SetAnimatorParameters(State.Running);
            }
            MoveAwayFromPlayer();
        }
        else
        {
            if (_currentState != State.Idle)
            {
                _currentState = State.Idle;
                SetAnimatorParameters(State.Idle);
            }
            IdleMovement();
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    private void MoveAwayFromPlayer()
    {

        Vector3 direction = (transform.position - Player.position).normalized;
        direction.y = 0; 

        _rb.MovePosition(transform.position + direction * EscapeSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * EscapeSpeed);
    }

    private void IdleMovement()
    {
       
        _idleMovementTimer += Time.deltaTime;

        if (_idleMovementTimer >= IdleMovementInterval)
        {
            _targetPosition = transform.position + new Vector3(
                Random.Range(-IdleMovementRange, IdleMovementRange),
                0,
                Random.Range(-IdleMovementRange, IdleMovementRange)
            );
            _targetPosition.y = transform.position.y; 

            _idleMovementTimer = 0f; 
        }

        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0; 

            _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * MoveSpeed);

            SetAnimatorParameters(State.Running);
        }
        else
        {
            SetAnimatorParameters(State.Idle);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (Health > 0)
        {
            Vector3 pushDirection = (transform.position - Player.position).normalized;
            pushDirection.y = LiftForce;

            _rb.AddForce(pushDirection * PushBackForce, ForceMode.Impulse);
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        _currentState = State.Dead;
        SetAnimatorParameters(State.Dead);
        Debug.Log("토끼가 죽었습니다.");
        StartCoroutine(WaitForDeathAnimation());
    }

    private void SetAnimatorParameters(State state)
    {
        _animator.SetBool("Idle", state == State.Idle);
        _animator.SetBool("Run", state == State.Running);
        _animator.SetBool("Dead", state == State.Dead);
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
