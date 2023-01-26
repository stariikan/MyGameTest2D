using UnityEngine;
using System.Collections.Generic;

public class ChatGPLeveL : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject[] enemyPrefabs;
    public int maxPlatforms = 20;
    public float horizontalMin = 6.5f;
    public float horizontalMax = 14f;
    public float verticalMin = -6f;
    public float verticalMax = 6;

    private Vector2 lastPlatformPos;
    private List<GameObject> generatedPlatforms;

    private void Start()
    {
        generatedPlatforms = new List<GameObject>();
        lastPlatformPos = transform.position;
        for (int i = 0; i < maxPlatforms; i++)
        {
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        Vector2 newPlatformPos = lastPlatformPos;
        newPlatformPos.x += Random.Range(horizontalMin, horizontalMax);
        newPlatformPos.y += Random.Range(verticalMin, verticalMax);

        GameObject newPlatform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], newPlatformPos, Quaternion.identity);
        generatedPlatforms.Add(newPlatform);
        lastPlatformPos = newPlatformPos;

        // Generate an enemy on the platform with a 50% chance
        if (Random.Range(0, 2) == 0)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], newPlatformPos, Quaternion.identity);
            newEnemy.transform.parent = newPlatform.transform;
        }
    }
}