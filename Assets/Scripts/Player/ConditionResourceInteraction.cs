using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionResourceInteraction : MonoBehaviour
{
    [SerializeField]LayerMask layer;
    [SerializeField] private PlayerConditions conditions;
    // Start is called before the first frame update
    void Start()
    {
        conditions = GetComponent<PlayerConditions>();
    }

}
