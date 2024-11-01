using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public float MaxHealth = 100f;  // �ִ� ü��
    private float _currentHealth;  // ���� ü��

    public float MoveSpeed = 5f;  // �⺻ �̵��ӵ�
    public float RunSpeed = 10f;  // �޸��� �̵��ӵ�
    public float MouseSensitivity = 100f;  // ���콺 ����

    public float JumpHeight = 2f;  // ���� ����
    private bool _isGrounded;  // �ٴڿ� �ִ��� ?
    private Rigidbody _rb;  // Rigidbody ����

    private float _xRotation = 0f;  // ī�޶� ȸ�� ����
    private Camera _playerCamera;  // �÷��̾� ī�޶�

    public float ShakingAmount = 0.2f;  // �޸��� �� ȭ�� ���� ũ��
    public float ShakingSpeed = 20f;  // �޸��� �� ���� �ӵ�
    private float _originalCameraY;  // ���� ī�޶� Y ��ġ

    // ���� ����
    public float AttackDamage = 1f;  // ���� ������

    // �޴� ������ ����
    public Image DamageOverlay;  // UI Image
    public float DamageOverlayDuration = 0.5f;  // ȭ�� �������� ���ӽð�

    void Start()
    {
        _currentHealth = MaxHealth;  // ���� ü���� �ִ� ü������ �ʱ�ȭ

        _playerCamera = Camera.main;  // ī�޶� ��������
        _rb = GetComponent<Rigidbody>();  // Rigidbody ��������

        // ���콺 Ŀ�� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _originalCameraY = _playerCamera.transform.localPosition.y;  // ī�޶� Y ��ġ ����

        // ȭ�� �ʱ�ȭ
        if (DamageOverlay != null)
        {
            DamageOverlay.color = new Color(1, 0, 0, 0);  // �����ϰ� �ʱ�ȭ
        }
    }

    void Update()
    {
        if (_playerCamera == null) return;  // ī�޶� ������ ����

        // ���콺 ȸ�� (ȭ�� ȸ��)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)  // ���콺 �������� �ִ� ��쿡�� ȸ��
        {
            mouseX *= MouseSensitivity * Time.deltaTime;
            mouseY *= MouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // ����
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);  // �ٴ� üũ

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))  // �ٴڿ� ���� ���� ����
        {
            _rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        // �̵�
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.LeftShift))  // �޸���
        {
            transform.position += move * RunSpeed * Time.deltaTime;

            // ī�޶� ����
            float newY = _originalCameraY + Mathf.Sin(Time.time * ShakingSpeed) * ShakingAmount;
            _playerCamera.transform.localPosition = new Vector3(_playerCamera.transform.localPosition.x, newY, _playerCamera.transform.localPosition.z);
        }
        else  // �Ϲ� �̵�
        {
            transform.position += move * MoveSpeed * Time.deltaTime;
            _playerCamera.transform.localPosition = new Vector3(_playerCamera.transform.localPosition.x, _originalCameraY, _playerCamera.transform.localPosition.z);  // ���� ��ġ�� ����
        }

        // Raycasting
        if (Input.GetButtonDown("Fire1"))  // ���� ���콺 Ŭ��
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 2f;  // Raycast�� �ִ� ���� ����

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("���� : " + hit.transform.name);  // �ӽ÷� ��Ŭ���� �ܼ�

                // ���� üũ
                Bear monster = hit.transform.GetComponent<Bear>();
                if (monster != null)
                {
                    // ���Ϳ��� ������ �ֱ�
                    monster.TakeDamage(AttackDamage);
                }
            }
        }
    }

    // �������� ���� �� ȣ��
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;  // ��������ŭ ���� ü�� ����
        Debug.Log("�÷��̾ " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + _currentHealth);

        // ȭ�� ������ ǥ��
        StartCoroutine(DamageOverlayCoroutine());

        // ü���� 0 ������ ���
        if (_currentHealth <= 0)
        {
            Die();  // ��� �޼��� ȣ��
        }
    }

    private void Die()
    {
        Debug.Log("�÷��̾ �׾����ϴ�.");
        // ����� ���� �߰��ϴ� ��
    }

    private IEnumerator DamageOverlayCoroutine()
    {
        if (DamageOverlay != null)
        {
            DamageOverlay.color = new Color(1, 0, 0, 0.3f);  // ������ ������
            yield return new WaitForSeconds(DamageOverlayDuration);  // ���
            DamageOverlay.color = new Color(1, 0, 0, 0);  // �ٽ� �����ϰ�
        }
    }

    void OnApplicationQuit()  // ���� ���� �� ���콺 Ŀ�� ����
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
