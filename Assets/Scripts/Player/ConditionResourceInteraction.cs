using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionResourceInteraction : MonoBehaviour
{
    [SerializeField] public GameObject eButton;
    [SerializeField] public GameObject waterFill;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerConditions conditions;
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private float autoThirst;
    [SerializeField] private float drinkwater;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        autoThirst = CharacterManager.Instance.Player.conditions.autoDecreaseThirst;
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
        if ( Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            waterFill.SetActive(true);
            eButton.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                CharacterManager.Instance.Player.conditions.thirst += drinkwater * Time.deltaTime;
                CharacterManager.Instance.Player.conditions.autoDecreaseThirst = 0;
            }
            else
            {
                CharacterManager.Instance.Player.conditions.autoDecreaseThirst = autoThirst;
            }
        }
        else
        {
            waterFill.SetActive(false);
            eButton.SetActive(false); 
        }
    }
}
