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
    public float baseSpeed; //속도 증가 포션에 사용하기 위한 속도, MoveSpeed랑 같은 것임
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
    // 받는 데미지 변수
    public Image DamageOverlay;  // UI Image
    public float DamageOverlayDuration = 0.5f;  // 화면 빨개지는 지속시간

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

        // 디버그 레이 그리기
        foreach (Ray ray in raysa)
        {
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red); // 레이를 빨간색으로 그립니다. 길이는 1로 설정.
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
    // 데미지를 받을 때 호출
    public void TakeDamage(float damage)
    {
        conditions.health -= damage;  // 데미지만큼 현재 체력 감소
        Debug.Log("플레이어가 " + damage + " 데미지를 받았습니다. 현재 체력: " + conditions.health);

        // 화면 빨갛게 표시
        StartCoroutine(DamageOverlayCoroutine());

        // 체력이 0 이하일 경우
        if (conditions.health <= 0)
        {
            Die();  // 사망 메서드 호출
        }
    }

    private void Die()
    {
        Debug.Log("플레이어가 죽었습니다.");
        // 사망시 로직 추가하는 곳
    }

    private IEnumerator DamageOverlayCoroutine()
    {
        if (DamageOverlay != null)
        {
            DamageOverlay.color = new Color(1, 0, 0, 0.3f);  // 불투명 빨간색
            yield return new WaitForSeconds(DamageOverlayDuration);  // 대기
            DamageOverlay.color = new Color(1, 0, 0, 0);  // 다시 투명하게
        }
    }

    void OnApplicationQuit()  // 게임 종료 시 마우스 커서 복원
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
