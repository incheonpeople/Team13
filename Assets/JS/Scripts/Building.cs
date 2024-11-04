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
            }
        }
    }

    void UseItem()
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

        bool isValidPosition = false; // 설치 가능 여부 초기화

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;

            // "Floor" 태그를 가진 오브젝트인지 확인
            if (hit.collider.CompareTag("Floor"))
            {
                isValidPosition = true; // 설치 가능
            }
        }

        // 청사진 위치 설정
        currentBlueprint.transform.position = targetPosition;

        // 마우스 휠로 회전
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentBlueprint.transform.Rotate(Vector3.up, scroll * 100f); // 회전 속도 조정
        }

        // 재질 설정
        if (isValidPosition)
        {
            SetBlueprintMaterial(currentBlueprint, transparentMaterial); // 설치 가능
        }
        else
        {
            SetBlueprintMaterial(currentBlueprint, invalidPlacementMaterial); // 설치 불가 시 머티리얼 설정
        }
    }

    void PlaceBlueprint()
    {
        // 설치가 가능한 경우에만 배치
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (currentBlueprint != null && Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Floor"))
        {
            GameObject placedObject = Instantiate(currentBlueprint);
            SetBlueprintMaterial(placedObject, opaqueMaterial); // 불투명

            AddColliderToObject(placedObject);

            Destroy(currentBlueprint);
        }
    }

    void AddColliderToObject(GameObject obj)
    {
        // 메쉬 콜라이더 추가
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.convex = false; // Convex를 false로 설정

        // 메쉬 설정 - blueprintPrefab의 메쉬를 가져오는 방식
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
