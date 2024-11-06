using UnityEngine;

public class Fish : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float IdleMovementInterval = 2f;
    public float IdleMovementRange = 5f;
    public float LayerHeight = 0f; 

    private Vector3 _targetPosition;
    private float _idleMovementTimer = 0f;
    private Rigidbody _rb;
    private LayerMask _waterLayer; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();


        _rb.useGravity = false;

        _waterLayer = LayerMask.GetMask("Water");
    }

    private void Update()
    {
        if (IsInWaterLayer())
        {
            IdleMovement();
        }
    }

    private bool IsInWaterLayer()
    {
        return Physics.Raycast(transform.position, Vector3.down, 10f, _waterLayer);
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


            _targetPosition.y = LayerHeight;

            _idleMovementTimer = 0f;
        }

        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0;

            _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);
        }
    }
}
