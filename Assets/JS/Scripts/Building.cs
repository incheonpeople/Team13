using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject blueprintPrefab; // 청사진
    public Material transparentMaterial; // 반투명
    public Material opaqueMaterial; // 불투명
    public Material invalidPlacementMaterial; // 설치 불가 시 사용할 머티리얼
    public float placementRange = 5f; // 설치 범위

    private GameObject currentBlueprint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseItem();
        }

        if (currentBlueprint != null)
        {
            MoveBlueprint();

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
            SetBlueprintMaterial(currentBlueprint, transparentMaterial); // 반투명
        }
        else
        {
            Destroy(currentBlueprint);
            currentBlueprint = null;
        }
    }

    void MoveBlueprint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 targetPosition = Camera.main.transform.position + forward * placementRange;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (currentBlueprint != null && Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Floor"))
        {
            GameObject placedObject = Instantiate(currentBlueprint);
            SetBlueprintMaterial(placedObject, opaqueMaterial);

            AddColliderToObject(placedObject);

            Destroy(currentBlueprint);
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
}
