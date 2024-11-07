using UnityEngine;

public class Test : MonoBehaviour
{
    public float damageAmount = 10f; 
    public float damageInterval = 1f;  
    private float _damageTimer = 0f;

    private void Update()
    {
        _damageTimer += Time.deltaTime;

        if (_damageTimer >= damageInterval)
        {
            Monster[] monsters = FindObjectsOfType<Monster>();

            foreach (Monster monster in monsters)
            {
                monster.TakeDamage(damageAmount);
            }

            _damageTimer = 0f; 
        }
    }
}
