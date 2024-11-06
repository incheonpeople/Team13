using UnityEngine;

public class Fish : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float IdleMovementInterval = 2f;
    public float IdleMovementRange = 5f;
    public float LayerHeight = 0f; // ���ӿ� ���� ���� ���� ��

    private Vector3 _targetPosition;
    private float _idleMovementTimer = 0f;
    private Rigidbody _rb;
    private LayerMask _waterLayer;  // ���� Layer Mask

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // �߷� ��Ȱ��ȭ
        _rb.useGravity = false;

        // ���� Layer ���� (��: "Water"��� Layer�� ����Ѵٰ� ����)
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
        // ���� ������� ��ġ�� ���� Layer�� �ִ��� Ȯ��
        return Physics.Raycast(transform.position, Vector3.down, 10f, _waterLayer);
    }

    private void IdleMovement()
    {
        _idleMovementTimer += Time.deltaTime;

        // �ֱ������� ��ǥ ��ġ ����
        if (_idleMovementTimer >= IdleMovementInterval)
        {
            _targetPosition = transform.position + new Vector3(
                Random.Range(-IdleMovementRange, IdleMovementRange),
                0,
                Random.Range(-IdleMovementRange, IdleMovementRange)
            );

            // ������ y���� �����ϵ�, ���ӿ� ���� ���� �̵��ϵ��� ��
            _targetPosition.y = LayerHeight;

            _idleMovementTimer = 0f;
        }

        // ��ǥ �������� �̵�
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            direction.y = 0; // y�� �̵��� �����Ͽ� ���� �̵��� ����

            // ����� �̵�
            _rb.MovePosition(transform.position + direction * MoveSpeed * Time.deltaTime);
        }
    }
}
