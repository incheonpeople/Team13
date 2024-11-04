using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject blueprintPrefab; // û����
    public Material transparentMaterial; // ������
    public Material opaqueMaterial; // ������
    public Material invalidPlacementMaterial; // ��ġ �Ұ� �� ����� ��Ƽ����
    public float placementRange = 5f; // ��ġ ����

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 targetPosition = Camera.main.transform.position + forward * placementRange;

        bool isValidPosition = false; // ��ġ ���� ���� �ʱ�ȭ

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;

            // "Floor" �±׸� ���� ������Ʈ���� Ȯ��
            if (hit.collider.CompareTag("Floor"))
            {
                isValidPosition = true; // ��ġ ����
            }
        }

        // û���� ��ġ ����
        currentBlueprint.transform.position = targetPosition;

        // ���콺 �ٷ� ȸ��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentBlueprint.transform.Rotate(Vector3.up, scroll * 100f); // ȸ�� �ӵ� ����
        }

        // ���� ����
        if (isValidPosition)
        {
            SetBlueprintMaterial(currentBlueprint, transparentMaterial); // ��ġ ����
        }
        else
        {
            SetBlueprintMaterial(currentBlueprint, invalidPlacementMaterial); // ��ġ �Ұ� �� ��Ƽ���� ����
        }
    }

    void PlaceBlueprint()
    {
        // ��ġ�� ������ ��쿡�� ��ġ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (currentBlueprint != null && Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Floor"))
        {
            GameObject placedObject = Instantiate(currentBlueprint);
            SetBlueprintMaterial(placedObject, opaqueMaterial); // ������

            AddColliderToObject(placedObject);

            Destroy(currentBlueprint);
        }
    }

    void AddColliderToObject(GameObject obj)
    {
        // �޽� �ݶ��̴� �߰�
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.convex = false; // Convex�� false�� ����

        // �޽� ���� - blueprintPrefab�� �޽��� �������� ���
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
