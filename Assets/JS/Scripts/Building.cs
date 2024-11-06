using UnityEngine;
using TMPro; // �ؽ�Ʈ ����

public class Building : MonoBehaviour
{
    public GameObject blueprintPrefab; // û����
    public Material transparentMaterial; // ������
    public Material opaqueMaterial; // ������
    public Material invalidPlacementMaterial; 
    public float placementRange = 5f; // ��ġ ����

    public GameObject[] prefabItemsToSpawn; 
    public int[] prefabItemCounts; 

    private GameObject currentBlueprint;
    private GameObject placedObject; 

    private Camera playerCamera; 
    private float destroyRange = 3f; 

    // UI ���� ����
    public TextMeshProUGUI blueprintStatusText; 
    public TextMeshProUGUI destroyStatusText;

    void Start()
    {
        playerCamera = Camera.main; 
        HideUI(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            PlaceBlueprint();
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            DestroyPlacedObjectAndSpawnPrefabs();
        }

        if (currentBlueprint != null)
        {
            MoveBlueprint();
        }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBlueprint();
                Destroy(gameObject);
            }
        }
    }

    public void UseItem()
    {
        if (currentBlueprint == null)
        {
            currentBlueprint = Instantiate(blueprintPrefab);
            SetBlueprintMaterial(currentBlueprint, transparentMaterial); // ������
        }
        else
        {
            Destroy(currentBlueprint);
            currentBlueprint = null;
        }
    }

    void MoveBlueprint()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 forward = playerCamera.transform.forward;
        Vector3 targetPosition = playerCamera.transform.position + forward * placementRange;

        bool isValidPosition = false;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;

            if (hit.collider.CompareTag("Floor"))
            {
                isValidPosition = true;
            }
        }

        currentBlueprint.transform.position = targetPosition;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentBlueprint.transform.Rotate(Vector3.up, scroll * 100f);
        }

        if (isValidPosition)
        {
            SetBlueprintMaterial(currentBlueprint, transparentMaterial);
        }
        else
        {
            SetBlueprintMaterial(currentBlueprint, invalidPlacementMaterial);
        }
    }

    void PlaceBlueprint()
    {
        if (currentBlueprint == null)
        {
            currentBlueprint = Instantiate(blueprintPrefab);
            SetBlueprintMaterial(currentBlueprint, transparentMaterial); 
            ShowBlueprintStatus("[F] Ű�� ���� ��ġ"); 
        }
        else
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Floor"))
            {
                placedObject = Instantiate(currentBlueprint);
                SetBlueprintMaterial(placedObject, opaqueMaterial);

                AddColliderToObject(placedObject);

                Destroy(currentBlueprint); 
                HideUI(); 
            }
        }
    }

    void DestroyPlacedObjectAndSpawnPrefabs()
    {
        if (placedObject != null && Vector3.Distance(playerCamera.transform.position, placedObject.transform.position) <= destroyRange)
        {
            Destroy(placedObject);
            placedObject = null;

            SpawnPrefabs();

            HideUI(); 
        }
    }

    void AddColliderToObject(GameObject obj)
    {
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.convex = false;

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
    }

    void SetBlueprintMaterial(GameObject obj, Material material)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    void SpawnPrefabs()
    {
        if (prefabItemsToSpawn == null || prefabItemCounts == null)
        {
            return;
        }

        for (int i = 0; i < prefabItemsToSpawn.Length; i++)
        {
            if (prefabItemsToSpawn[i] != null)
            {
                for (int j = 0; j < prefabItemCounts[i]; j++)
                {
                  
                    Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f; 
                    Instantiate(prefabItemsToSpawn[i], spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    void ShowBlueprintStatus(string message)
    {
        blueprintStatusText.text = message;
        blueprintStatusText.gameObject.SetActive(true); 
    }

    void HideUI()
    {
        blueprintStatusText.gameObject.SetActive(false);
        destroyStatusText.gameObject.SetActive(false);
    }

    void CheckDestroyRange()
    {
        if (placedObject != null)
        {
            float distance = Vector3.Distance(playerCamera.transform.position, placedObject.transform.position);
            if (distance <= destroyRange)
            {
                ShowDestroyStatus("[E] Ű�� ���� ����");
            }
            else
            {
                HideUI(); 
            }
        }
    }

    void ShowDestroyStatus(string message)
    {
        destroyStatusText.text = message;
        destroyStatusText.gameObject.SetActive(true); 
    }
}
