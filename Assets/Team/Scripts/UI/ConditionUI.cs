using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUI : MonoBehaviour
{
    [SerializeField] public Image HealthImage;
    [SerializeField] public Image HungerImage;
    [SerializeField] public Image ThirstImage;
    [SerializeField] private PlayerConditions conditions;
    // Start is called before the first frame update
    void Start()
    {
        conditions = CharacterManager.Instance.Player.GetComponent<PlayerConditions>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthImage.fillAmount = conditions.health/conditions.maxHealth;
        HungerImage.fillAmount = conditions.hunger/conditions.maxHunger;
        ThirstImage.fillAmount = conditions.thirst/conditions.maxThirst;
    }
    

}
