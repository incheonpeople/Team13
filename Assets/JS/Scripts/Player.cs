using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f; // �ִ� ü��
    private float currentHealth; // ���� ü��

    public float moveSpeed = 5f; // �⺻ �̵��ӵ�
    public float runSpeed = 10f; // �޸��� �̵��ӵ�
    public float mouseSensitivity = 100f; // ���콺 ����

    public float jumpHeight = 2f; // ���� ����
    private bool isGrounded; // �ٴڿ� �ִ��� ?
    private Rigidbody rb; // Rigidbody ���� �߰�

    private float xRotation = 0f;
    private Camera playerCamera;

    public float shakingAmount = 0.2f; // �޸��� �� ȭ�� ���� ũ��
    public float shakingSpeed = 20f; // �޸��� �� ���� �ӵ�
    private float originalCameraY; // ���� ī�޶� Y ��ġ

    // ���� ����
    public float attackDamage = 1f; // ���� ������

    // �޴� ������ ����
    public Image damageOverlay; // UI Image
    public float damageOverlayDuration = 0.5f; // ȭ�� �������� ���ӽð�

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ

        playerCamera = Camera.main;
        rb = GetComponent<Rigidbody>(); // Rigidbody ��������

        // ���콺 Ŀ�� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalCameraY = playerCamera.transform.localPosition.y; // ī�޶� Y ��ġ ����

        // ȭ�� �ʱ�ȭ
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0); // �����ϰ� �ʱ�ȭ
        }
    }

    void Update()
    {
        if (playerCamera == null) return; // ī�޶� ������ ����

        // ���콺 ȸ�� (ȭ�� ȸ��)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f) // ���콺 �������� �ִ� ��쿡�� ȸ��
        {
            mouseX *= mouseSensitivity * Time.deltaTime;
            mouseY *= mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // ���� 
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); // �ٴ� üũ

        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) // �ٴڿ� ���� ���� ����
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        // �̵�
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += move * runSpeed * Time.deltaTime;

            // ī�޶� ����
            float newY = originalCameraY + Mathf.Sin(Time.time * shakingSpeed) * shakingAmount;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newY, playerCamera.transform.localPosition.z);
        }
        else
        {
            transform.position += move * moveSpeed * Time.deltaTime;

            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalCameraY, playerCamera.transform.localPosition.z); // ���� ��ġ�� ����
        }

        // Raycasting ��� �ڵ� (������ ��� ��Ŭ���� ��ȣ�ۿ�)

        if (Input.GetButtonDown("Fire1")) // ���� ���콺 Ŭ��
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxDistance = 2f; // Raycast�� �ִ� ���� ����

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("���� : " + hit.transform.name); // �ӽ÷� ��Ŭ���� �ܼ�

                // ���� üũ
                MonsterAI monster = hit.transform.GetComponent<MonsterAI>();
                if (monster != null)
                {
                    // ���Ϳ��� ������ �ֱ�
                    monster.TakeDamage(attackDamage);
                }
            }
        }
    }

    // �������� ���� �� ȣ��
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ��������ŭ ���� ü�� ����
        Debug.Log("�÷��̾ " + damage + " �������� �޾ҽ��ϴ�. ���� ü��: " + currentHealth);

        // ȭ�� ������ ǥ��
        StartCoroutine(DamageOverlayCoroutine());

        // ü���� 0 ������ ���
        if (currentHealth <= 0)
        {
            Die(); // ��� �޼��� ȣ��
        }
    }
    private void Die()
    {
        Debug.Log("�÷��̾ �׾����ϴ�.");
        // ����� ���� �߰��ϴ� ��
    }
    private IEnumerator DamageOverlayCoroutine()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.3f); // ������ ������
            yield return new WaitForSeconds(damageOverlayDuration); // ���
            damageOverlay.color = new Color(1, 0, 0, 0); // �ٽ� �����ϰ�
        }
    }

    void OnApplicationQuit() // ���� ���� �� ���콺 Ŀ�� ����
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
