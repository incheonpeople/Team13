using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public float MaxHealth = 100f;  // 최대 체력
    private float _currentHealth;  // 현재 체력

    public float MoveSpeed = 5f;  // 기본 이동속도
    public float RunSpeed = 10f;  // 달리기 이동속도
    public float MouseSensitivity = 100f;  // 마우스 감도

    public float JumpHeight = 2f;  // 점프 높이
    private bool _isGrounded;  // 바닥에 있는지 ?
    private Rigidbody _rb;  // Rigidbody 변수

    private float _xRotation = 0f;  // 카메라 회전 각도
    private Camera _playerCamera;  // 플레이어 카메라

    public float ShakingAmount = 0.2f;  // 달리기 시 화면 흔들기 크기
    public float ShakingSpeed = 20f;  // 달리기 시 흔들기 속도
    private float _originalCameraY;  // 원래 카메라 Y 위치

    // 공격 변수
    public float AttackDamage = 1f;  // 공격 데미지

    // 받는 데미지 변수
    public Image DamageOverlay;  // UI Image
    public float DamageOverlayDuration = 0.5f;  // 화면 빨개지는 지속시간

    void Start()
    {
        _currentHealth = MaxHealth;  // 현재 체력을 최대 체력으로 초기화

        _playerCamera = Camera.main;  // 카메라 가져오기
        _rb = GetComponent<Rigidbody>();  // Rigidbody 가져오기

        // 마우스 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _originalCameraY = _playerCamera.transform.localPosition.y;  // 카메라 Y 위치 저장

        // 화면 초기화
        if (DamageOverlay != null)
        {
            DamageOverlay.color = new Color(1, 0, 0, 0);  // 투명하게 초기화
        }
    }

    void Update()
    {
        if (_playerCamera == null) return;  // 카메라가 없으면 종료

        // 마우스 회전 (화면 회전)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)  // 마우스 움직임이 있는 경우에만 회전
        {
            mouseX *= MouseSensitivity * Time.deltaTime;
            mouseY *= MouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // 점프
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);  // 바닥 체크

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))  // 바닥에 있을 때만 점프
        {
            _rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        // 이동
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.LeftShift))  // 달리기
        {
            transform.position += move * RunSpeed * Time.deltaTime;

            // 카메라 흔들기
            float newY = _originalCameraY + Mathf.Sin(Time.time * ShakingSpeed) * ShakingAmount;
            _playerCamera.transform.localPosition = new Vector3(_playerCamera.transform.localPosition.x, newY, _playerCamera.transform.localPosition.z);
        }
        else  // 일반 이동
        {
            transform.position += move * MoveSpeed * Time.deltaTime;
            _playerCamera.transform.localPosition = new Vector3(_playerCamera.transform.localPosition.x, _originalCameraY, _playerCamera.transform.localPosition.z);  // 원래 위치로 복원
        }

        // Raycasting
        if (Input.GetButtonDown("Fire1"))  // 왼쪽 마우스 클릭
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 2f;  // Raycast의 최대 범위 설정

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("감지 : " + hit.transform.name);  // 임시로 좌클릭시 콘솔

                // 몬스터 체크
                Bear monster = hit.transform.GetComponent<Bear>();
                if (monster != null)
                {
                    // 몬스터에게 데미지 주기
                    monster.TakeDamage(AttackDamage);
                }
            }
        }
    }

    // 데미지를 받을 때 호출
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;  // 데미지만큼 현재 체력 감소
        Debug.Log("플레이어가 " + damage + " 데미지를 받았습니다. 현재 체력: " + _currentHealth);

        // 화면 빨갛게 표시
        StartCoroutine(DamageOverlayCoroutine());

        // 체력이 0 이하일 경우
        if (_currentHealth <= 0)
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
}
