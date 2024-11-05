using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;
    public LayerMask damageZoneLayer; // [Add] DamageZone ���̾�
    public TextMeshProUGUI damageText;  // [Add2] damageText ����


    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;
    public int damageAmount = 5; // [Add] DamageZone ������
    public float damageCheckInterval = 5f; // [Add] DamageZone���� �������� �ִ� ����
    private float lastDamageTime; // [Add] ������ ������ üũ �ð�

    private PlayerConditions playerConditions;

    void Start()
    {
        camera = Camera.main;
        playerConditions = GetComponent<PlayerConditions>();
        damageText.gameObject.SetActive(false); // [Add2] ���� �� damageText ��Ȱ��ȭ
        if (damageText == null)
        {
            Debug.LogError("damageText is not assigned in the Inspector.");
        }
        else
        {
            damageText.gameObject.SetActive(false); // [Add3] ���� �� damageText ��Ȱ��ȭ
        }
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                //promptText.gameObject.SetActive(false);
            }
            Ray damageRay = new Ray(transform.position + Vector3.up * 5f, Vector3.down); // [Add] �ϴÿ��� �Ʒ��� Ray �߻��Ͽ� DamageZome ����
                                                                                           
            if (Physics.Raycast(damageRay, out hit, maxCheckDistance, damageZoneLayer))
            {
                if (Time.time - lastDamageTime > damageCheckInterval)
                {
                    lastDamageTime = Time.time;
                    playerConditions.TakePhysicalDamage(damageAmount);

                    ShowDamageText(); // [Add2] ������ ���� �� �ؽ�Ʈ ǥ��
                }
            }
            else
            {
                //damageText.gameObject.SetActive(false); // ������ ������ ����� �ؽ�Ʈ �����
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && curInteractGameObject != null && curInteractable != null
    && curInteractGameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
    private void ShowDamageText()
    {
        Debug.Log("Showing damage text."); // ����� �α� �߰�
        damageText.gameObject.SetActive(true);
        StopCoroutine("HideDamageText");
        StartCoroutine("HideDamageText");
    }

    private IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(2f); // 2�� ���� �ؽ�Ʈ ǥ��
        Debug.Log("Hiding damage text."); // ����� �α� �߰�
        damageText.gameObject.SetActive(false);
    }


    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
