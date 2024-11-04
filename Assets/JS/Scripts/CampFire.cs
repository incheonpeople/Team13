using UnityEngine;

public class CampFire : MonoBehaviour
{
    public GameObject blueprintPrefab; // 청사진
    public Material transparentMaterial; // 반투명
    public Material opaqueMaterial; // 불투명
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

        if (Physics.Raycast(ray, out hit))
        {
            if (Vector3.Distance(hit.point, transform.position) <= placementRange)
            {
                currentBlueprint.transform.position = hit.point; 
            }
        }
    }

    void PlaceBlueprint()
    {
        if (currentBlueprint != null)
        {
            GameObject placedObject = Instantiate(currentBlueprint);
            SetBlueprintMaterial(placedObject, opaqueMaterial); // 불투명

            AddColliderToObject(placedObject);

            Destroy(currentBlueprint); 
        }
    }

    void AddColliderToObject(GameObject obj)
    {
        Collider collider = obj.AddComponent<BoxCollider>(); // BoxCollider
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
