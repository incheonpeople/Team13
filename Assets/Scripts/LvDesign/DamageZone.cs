using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 5;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 캐릭터가 Damage Zone에 들어왔을 때
        {
            var damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damageCoroutine = StartCoroutine(ApplyPeriodicDamage(damagable));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    private IEnumerator ApplyPeriodicDamage(IDamagable damagable)
    {
        while (true)
        {
            damagable.TakePhysicalDamage(damageAmount);
            yield return new WaitForSeconds(5f);
            Debug.Log("ok");
        }
    }
}