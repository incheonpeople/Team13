using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Resource resourcePrefab;  // Resource ������Ʈ ������ (������ �ڿ��� ����)
    public int resourceCount = 10;  // ������ Resource ������Ʈ�� ��
    public string spawnAreaTag = "SpawnArea";  // ������ ������ �±�
    public float spawnHeight = 1.0f;  // Resource ������Ʈ�� �����Ǵ� ����
    public int capacity; // �ڿ��� �ִ� �뷮

    private List<Resource> spawnedResources = new List<Resource>(); // ���� ������ �ڿ����� ������ ����Ʈ

    void Start()
    {
        SpawnResources();  // ���� ���� �� �ڿ� ����
    }

    void Update()
    {
        CheckAndRespawnResources();  // �� �����Ӹ��� �ڿ��� �˻��ϰ� �ʿ� �� �ٽ� ����
    }

    void SpawnResources()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag); // ������ �±׸� ���� ���� ������ ��� ã��

        if (spawnAreas.Length == 0)
        {
            return; // ���� ������ ������ �Լ� ����
        }

        int resourcesPerArea = resourceCount / spawnAreas.Length; // �� ���� ������ ����� �ڿ��� �⺻ ��
        int remainingResources = resourceCount % spawnAreas.Length; // ��� �� ���� �ڿ� ��

        foreach (GameObject spawnArea in spawnAreas)
        {
            Collider spawnCollider = spawnArea.GetComponent<Collider>(); // ���� ������ Collider�� ������
            Terrain terrain = spawnArea.GetComponent<Terrain>();

            if (spawnCollider == null || terrain == null)
            {
                continue; // Collider�� ������ ���� ������ �ǳʶ�
            }

            int spawnCount = resourcesPerArea; // �� ������ �⺻������ ��е� �ڿ� �� ����
            if (remainingResources > 0)
            {
                spawnCount++; // ���� �ڿ��� ������ �ϳ� �� ���
                remainingResources--;
            }

            SpawnResourcesInArea(spawnCollider, terrain, spawnCount); // ���� ���� ���� �ڿ� ����
        }
    }

    void SpawnResourcesInArea(Collider spawnArea, Terrain terrain, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x); // ���� ������ x ���� ������ ������ ��ġ ����
            float randomZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z); // ���� ������ z ���� ������ ������ ��ġ ����

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ); // ������ ��ġ�� ���� ���� �����Ͽ� ���� ���� ��ġ ����

            float terrianY = terrain.SampleHeight(randomPosition);
            randomPosition.y = terrianY;

            Resource newResource = Instantiate(resourcePrefab, randomPosition, Quaternion.identity); // �ڿ� �������� ����
            spawnedResources.Add(newResource); // ������ �ڿ��� ����Ʈ�� �߰�
        }
    }

    void CheckAndRespawnResources()
    {
        for (int i = spawnedResources.Count - 1; i >= 0; i--)
        {
            Resource resource = spawnedResources[i];

            if (resource == null || resource.capacity <= 0)  // �ڿ��� �Ҹ�� ��� üũ
            {
                if (resource != null)
                {
                    Destroy(resource.gameObject); // �Ҹ�� �ڿ� ��ü�� ����
                }
                spawnedResources.RemoveAt(i); // ����Ʈ���� ����
                SpawnResourceAtRandomLocation(); // ���ο� ��ġ���� �ڿ� ����
            }
        }

        int currentResourceCount = spawnedResources.Count; // ���� ������ �ڿ� �� Ȯ��

        if (currentResourceCount < resourceCount)
        {
            int resourcesToSpawn = resourceCount - currentResourceCount; // ������ �ڿ� �� ���

            for (int i = 0; i < resourcesToSpawn; i++)
            {
                SpawnResourceAtRandomLocation(); // ������ �ڿ��� ����
            }
        }
    }

    void SpawnResourceAtRandomLocation()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag); // ������ ������ ã��

        if (spawnAreas.Length > 0)
        {
            GameObject spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)]; // �������� �ϳ��� ���� ���� ����
            Collider spawnCollider = spawnArea.GetComponent<Collider>(); // ���õ� ������ Collider ��������

            if (spawnCollider == null)
            {
                return; // Collider�� ������ �Լ� ����
            }

            float randomX = Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x); // ���� ������ x ���� �� ������ ��ġ
            float randomZ = Random.Range(spawnCollider.bounds.min.z, spawnCollider.bounds.max.z); // ���� ������ z ���� �� ������ ��ġ

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ); // ���� ���� �����Ͽ� ���� ���� ��ġ ����

            Resource newResource = Instantiate(resourcePrefab, randomPosition, Quaternion.identity); // �ڿ� �������� ����
            spawnedResources.Add(newResource); // ����Ʈ�� �߰�
        }
    }
}

