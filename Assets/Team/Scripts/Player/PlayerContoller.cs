using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float baseSpeed; //�ӵ� ���� ���ǿ� ����ϱ� ���� �ӵ�, MoveSpeed�� ���� ����
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXlock;
    public float maxXlock;
    public float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action addItem;
    public Action inventory;
    public Action InteractionInventroy;
    public Action ExitInteractionInventory;
    // �޴� ������ ����
    public Image DamageOverlay;  // UI Image
    public float DamageOverlayDuration = 0.5f;  // ȭ�� �������� ���ӽð�

    private Rigidbody _rigidbody;

    public PlayerConditions conditions;
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        conditions = gameObject.GetComponent<PlayerConditions>();
        baseSpeed = moveSpeed;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray[] raysa = new Ray[4]
    {
        new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
    };

        // ����� ���� �׸���
        foreach (Ray ray in raysa)
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red); // ���̸� ���������� �׸��ϴ�. ���̴� 1�� ����.
        }

        Move();
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXlock, maxXlock);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    // �������� ���� �� ȣ��
    public void TakeDamage(float damage)
    {
        conditions.health -= damage;  // ��������ŭ ���� ü�� ����
        Debug.Log("�÷��̾ " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + conditions.health);

        // ȭ�� ������ ǥ��
        StartCoroutine(DamageOverlayCoroutine());

        // ü���� 0 ������ ���
        if (conditions.health <= 0)
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


    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CampFire"))
        {
            InteractionInventroy?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CampFire"))
        {
            ExitInteractionInventory?.Invoke();
        }
    }
}
