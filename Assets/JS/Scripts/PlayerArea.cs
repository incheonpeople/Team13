using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� ������ �����߽��ϴ�.");
        }
        else if (other.CompareTag("Monster"))
        {
            PreventMonsterFromEntering(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� ������ ������ϴ�.");
        }
    }

    private void PreventMonsterFromEntering(Collider monster)
    {
        Rigidbody rb = monster.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero; 

            Vector3 pushDirection = (monster.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * 15f, ForceMode.Impulse); // �о�� ��
        }
        else
        {
            Vector3 pushDirection = (monster.transform.position - transform.position).normalized;
            monster.transform.position += pushDirection * 0.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            PreventMonsterFromEntering(other);
        }
    }
}
