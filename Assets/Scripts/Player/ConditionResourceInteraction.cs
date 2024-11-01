using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionResourceInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerConditions conditions;
    [SerializeField] private float maxCheckDistance;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        conditions = GetComponent<PlayerConditions>();
        camera = Camera.main;
    }
    private void Update()
    {
        ResourceInteraction();
    }
    public void ResourceInteraction()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            Debug.Log("¹° ¹ß°ß");
        }
    }
}
