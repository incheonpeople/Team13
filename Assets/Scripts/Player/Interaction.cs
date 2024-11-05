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
    public LayerMask damageZoneLayer; // [Add] DamageZone 레이어
    public TextMeshProUGUI damageText;  // [Add2] damageText 정의


    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;
    public int damageAmount = 5; // [Add] DamageZone 데미지
    public float damageCheckInterval = 5f; // [Add] DamageZone에서 데미지를 주는 간격
    private float lastDamageTime; // [Add] 마지막 데미지 체크 시간

    private PlayerConditions playerConditions;

    void Start()
    {
        camera = Camera.main;
        playerConditions = GetComponent<PlayerConditions>();
        damageText.gameObject.SetActive(false); // [Add2] 시작 시 damageText 비활성화
        if (damageText == null)
        {
            Debug.LogError("damageText is not assigned in the Inspector.");
        }
        else
        {
            damageText.gameObject.SetActive(false); // [Add3] 시작 시 damageText 비활성화
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
            Ray damageRay = new Ray(transform.position + Vector3.up * 5f, Vector3.down); // [Add] 하늘에서 아래로 Ray 발사하여 DamageZome 감지
                                                                                           
            if (Physics.Raycast(damageRay, out hit, maxCheckDistance, damageZoneLayer))
            {
                if (Time.time - lastDamageTime > damageCheckInterval)
                {
                    lastDamageTime = Time.time;
                    playerConditions.TakePhysicalDamage(damageAmount);

                    ShowDamageText(); // [Add2] 데미지 받을 때 텍스트 표시
                }
            }
            else
            {
                //damageText.gameObject.SetActive(false); // 데미지 구역을 벗어나면 텍스트 숨기기
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
        Debug.Log("Showing damage text."); // 디버그 로그 추가
        damageText.gameObject.SetActive(true);
        StopCoroutine("HideDamageText");
        StartCoroutine("HideDamageText");
    }

    private IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(2f); // 2초 동안 텍스트 표시
        Debug.Log("Hiding damage text."); // 디버그 로그 추가
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
