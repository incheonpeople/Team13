using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Resource resourcePrefab;  // Resource 오브젝트 프리팹 (생성할 자원의 원본)
    public int resourceCount = 10;  // 스폰할 Resource 오브젝트의 수
    public string spawnAreaTag = "SpawnArea";  // 스폰할 영역의 태그
    public float spawnHeight = 1.0f;  // Resource 오브젝트가 스폰되는 높이
    public int capacity; // 자원의 최대 용량

    private List<Resource> spawnedResources = new List<Resource>(); // 현재 생성된 자원들을 저장할 리스트

    void Start()
    {
        SpawnResources();  // 게임 시작 시 자원 스폰
    }

    void Update()
    {
        CheckAndRespawnResources();  // 매 프레임마다 자원을 검사하고 필요 시 다시 스폰
    }

    void SpawnResources()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag); // 지정된 태그를 가진 스폰 영역을 모두 찾음

        if (spawnAreas.Length == 0)
        {
            return; // 스폰 영역이 없으면 함수 종료
        }

        int resourcesPerArea = resourceCount / spawnAreas.Length; // 각 스폰 영역에 배분할 자원의 기본 수
        int remainingResources = resourceCount % spawnAreas.Length; // 배분 후 남은 자원 수

        foreach (GameObject spawnArea in spawnAreas)
        {
            Collider spawnCollider = spawnArea.GetComponent<Collider>(); // 스폰 영역의 Collider를 가져옴
            Terrain terrain = spawnArea.GetComponent<Terrain>();

            if (spawnCollider == null || terrain == null)
            {
                continue; // Collider가 없으면 스폰 영역을 건너뜀
            }

            int spawnCount = resourcesPerArea; // 각 영역에 기본적으로 배분된 자원 수 설정
            if (remainingResources > 0)
            {
                spawnCount++; // 남은 자원이 있으면 하나 더 배분
                remainingResources--;
            }

            SpawnResourcesInArea(spawnCollider, terrain, spawnCount); // 스폰 영역 내에 자원 생성
        }
    }

    void SpawnResourcesInArea(Collider spawnArea, Terrain terrain, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x); // 스폰 영역의 x 범위 내에서 무작위 위치 선택
            float randomZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z); // 스폰 영역의 z 범위 내에서 무작위 위치 선택

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ); // 무작위 위치에 높이 값을 적용하여 최종 스폰 위치 결정

            float terrianY = terrain.SampleHeight(randomPosition);
            randomPosition.y = terrianY;

            Resource newResource = Instantiate(resourcePrefab, randomPosition, Quaternion.identity); // 자원 프리팹을 생성
            spawnedResources.Add(newResource); // 생성된 자원을 리스트에 추가
        }
    }

    void CheckAndRespawnResources()
    {
        for (int i = spawnedResources.Count - 1; i >= 0; i--)
        {
            Resource resource = spawnedResources[i];

            if (resource == null || resource.capacity <= 0)  // 자원이 소모된 경우 체크
            {
                if (resource != null)
                {
                    Destroy(resource.gameObject); // 소모된 자원 객체를 제거
                }
                spawnedResources.RemoveAt(i); // 리스트에서 제거
                SpawnResourceAtRandomLocation(); // 새로운 위치에서 자원 생성
            }
        }

        int currentResourceCount = spawnedResources.Count; // 현재 스폰된 자원 수 확인

        if (currentResourceCount < resourceCount)
        {
            int resourcesToSpawn = resourceCount - currentResourceCount; // 부족한 자원 수 계산

            for (int i = 0; i < resourcesToSpawn; i++)
            {
                SpawnResourceAtRandomLocation(); // 부족한 자원을 스폰
            }
        }
    }

    void SpawnResourceAtRandomLocation()
    {
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(spawnAreaTag); // 스폰할 영역을 찾음

        if (spawnAreas.Length > 0)
        {
            GameObject spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)]; // 무작위로 하나의 스폰 영역 선택
            Collider spawnCollider = spawnArea.GetComponent<Collider>(); // 선택된 영역의 Collider 가져오기

            if (spawnCollider == null)
            {
                return; // Collider가 없으면 함수 종료
            }

            float randomX = Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x); // 스폰 영역의 x 범위 내 무작위 위치
            float randomZ = Random.Range(spawnCollider.bounds.min.z, spawnCollider.bounds.max.z); // 스폰 영역의 z 범위 내 무작위 위치

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ); // 높이 값을 적용하여 최종 스폰 위치 결정

            Resource newResource = Instantiate(resourcePrefab, randomPosition, Quaternion.identity); // 자원 프리팹을 생성
            spawnedResources.Add(newResource); // 리스트에 추가
        }
    }
}

