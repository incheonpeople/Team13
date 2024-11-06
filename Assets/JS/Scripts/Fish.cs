using UnityEngine;

public class Fish : Monster
{
    public Transform Player;
    public GameObject deathPrefab;

    public float IdleMovementRange = 3f; 
    public float IdleMovementInterval = 2f;
    public float SwimSpeed = 5f;

    private Rigidbody _rb;
    private enum State { Idle, Swimming, Dead }
    private State _currentState;

    private LayerMask waterLayerMask;

    private const float MaxY = 3f;
    private const float MinY = 0.8f;

    private Vector3 _targetPosition;
    private float _idleMovementTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentState = State.Idle;

        Initialize("Fish", 10f, 0f, 5f, 0f, 0f);

        waterLayerMask = LayerMask.GetMask("Water");

        _rb.useGravity = false;

        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_currentState == State.Dead)
        {
            return;
        }

        if (Health <= 0 && _currentState != State.Dead)
        {
            Die();
        }

        if (_currentState != State.Swimming)
        {
            IdleMovement();
        }
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

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, waterLayerMask))
            {
                _targetPosition.y = hit.point.y;
            }

            _targetPosition.y = Mathf.Clamp(_targetPosition.y, MinY, MaxY);

            _idleMovementTimer = 0f;
        }

        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0; 

            _rb.MovePosition(Vector3.MoveTowards(transform.position, _targetPosition, SwimSpeed * Time.deltaTime));

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * SwimSpeed);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (Health > 0)
        {
            Vector3 pushDirection = (transform.position - Player.position).normalized;
            pushDirection.y = 0;

            _rb.AddForce(pushDirection * 5f, ForceMode.Impulse);
        }

        if (Health <= 0 && _currentState != State.Dead)
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
        Debug.Log("물고기가 죽었습니다.");

        if (deathPrefab != null)
        {
            Instantiate(deathPrefab, transform.position, Quaternion.identity); 
        }

        Destroy(gameObject);
    }
}
