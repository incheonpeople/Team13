using UnityEngine;

public class Fish : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float IdleMovementInterval = 2f;
    public float IdleMovementRange = 5f;
    public float LayerHeight = 0f; // 물속에 있을 때의 높이 값

    private Vector3 _targetPosition;
    private float _idleMovementTimer = 0f;
    private Rigidbody _rb;
    private LayerMask _waterLayer;  // 물속 Layer Mask

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // 중력 비활성화
        _rb.useGravity = false;

        // 물속 Layer 정의 (예: "Water"라는 Layer를 사용한다고 가정)
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
        // 현재 물고기의 위치가 물속 Layer에 있는지 확인
        return Physics.Raycast(transform.position, Vector3.down, 10f, _waterLayer);
    }

    private void IdleMovement()
    {
        _idleMovementTimer += Time.deltaTime;

        // 주기적으로 목표 위치 설정
        if (_idleMovementTimer >= IdleMovementInterval)
        {
            _targetPosition = transform.position + new Vector3(
                Random.Range(-IdleMovementRange, IdleMovementRange),
                0,
                Random.Range(-IdleMovementRange, IdleMovementRange)
            );

            // 물고기는 y축을 고정하되, 물속에 있을 때만 이동하도록 함
            _targetPosition.y = LayerHeight;

            _idleMovementTimer = 0f;
        }

        // 목표 지점까지 이동
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0; // y축 이동을 방지하여 수평 이동만 수행

            // 물고기 이동
            _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);
        }
    }
}
