using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    public GameObject objectToSpawn;

    public int objectCount = 10;

    public string spawnAreaTag = "SpawnArea";

    public float spawnHeight = 1.0f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        SpawnObjects();
    }

    void Update()
    {
        CheckAndRespawnObjects();
    }

    void SpawnObjects()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag);

        if (spawnAreas.Length == 0)
        {
            return;
        }

        int objectsPerArea = objectCount / spawnAreas.Length;
        int remainingObjects = objectCount % spawnAreas.Length; 

        foreach (GameObject spawnArea in spawnAreas)
        {
            Collider spawnCollider = spawnArea.GetComponent<Collider>();

            if (spawnCollider == null)
            {
                continue;
            }

            int spawnCount = objectsPerArea;
            if (remainingObjects > 0)
            {
                spawnCount++;  
                remainingObjects--;
            }

            SpawnObjectsInArea(spawnCollider, spawnCount);
        }
    }

    void SpawnObjectsInArea(Collider spawnArea, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            float randomZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ);

            GameObject newObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

            spawnedObjects.Add(newObject);
        }
    }

    void CheckAndRespawnObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedObjects[i];

            if (obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (obj.transform.position.y <= -10f)
            {
                Destroy(obj);

                spawnedObjects.RemoveAt(i);

                SpawnObjectAtRandomLocation();
            }
        }

        int currentObjectCount = spawnedObjects.Count;

        if (currentObjectCount < objectCount)
        {
            int objectsToSpawn = objectCount - currentObjectCount;  

            for (int i = 0; i < objectsToSpawn; i++)
            {
                SpawnObjectAtRandomLocation();
            }
        }
    }


    void SpawnObjectAtRandomLocation()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag);

        if (spawnAreas.Length > 0)
        {
            GameObject spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

            Collider spawnCollider = spawnArea.GetComponent<Collider>();

            if (spawnCollider == null)
            {
                return;
            }

            float randomX = Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x);
            float randomZ = Random.Range(spawnCollider.bounds.min.z, spawnCollider.bounds.max.z);

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ);

            GameObject newObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

            spawnedObjects.Add(newObject);
        }
    }
}
