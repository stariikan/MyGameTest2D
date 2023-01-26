using UnityEngine;
using System.Collections.Generic;

public class levelGen2 : MonoBehaviour
{
    //This script generates a castle with multiple rooms using a for loop, each iteration of the loop generates a new room. Inside the GenerateRoom() function, it uses nested for loops to generate the walls and floor for the room, and also generates a random door, random furniture and random enemies. The script uses a number of public variables to control the size of the rooms, the number of rooms, and the number of furniture and enemies.
    //You can use this script and attach it to a empty game object in your scene, and fill the wallPrefabs, floorPrefabs, doorPrefabs, furniturePrefabs, and enemyPrefabs arrays with the sprites you want to use for the level generation.
    //Please note that this is a complex example of level generation, it might require additional functionality such as collision detection, user input

    public GameObject[] wallPrefabs;
    public GameObject[] floorPrefabs;
    public GameObject[] doorPrefabs;
    public GameObject[] furniturePrefabs;
    public GameObject[] enemyPrefabs;
    public int roomWidth = 10;
    public int roomHeight = 10;
    public int maxRooms = 10;
    public int minFurniture = 2;
    public int maxFurniture = 5;
    public int minEnemies = 1;
    public int maxEnemies = 3;

    private Vector2 lastRoomPos;
    private List<GameObject> generatedRooms;

    private void Start()
    {
        generatedRooms = new List<GameObject>();
        lastRoomPos = transform.position;
        for (int i = 0; i < maxRooms; i++)
        {
            GenerateRoom();
        }
    }

    private void GenerateRoom()
    {
        Vector2 newRoomPos = lastRoomPos;
        newRoomPos.x += Random.Range(0, roomWidth);
        newRoomPos.y += Random.Range(0, roomHeight);

        GameObject newRoom = new GameObject("Room");
        newRoom.transform.position = newRoomPos;
        generatedRooms.Add(newRoom);
        lastRoomPos = newRoomPos;

        // Generate walls
        for (int x = -1; x <= roomWidth; x++)
        {
            for (int y = -1; y <= roomHeight; y++)
            {
                if (x == -1 || x == roomWidth || y == -1 || y == roomHeight)
                {
                    GameObject newWall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], new Vector2(x, y) + newRoomPos, Quaternion.identity);
                    newWall.transform.parent = newRoom.transform;
                }
                else
                {
                    GameObject newFloor = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Length)], new Vector2(x, y) + newRoomPos, Quaternion.identity);
                    newFloor.transform.parent = newRoom.transform;
                }
            }
        }

        // Generate door
        Vector2 doorPos = new Vector2(Random.Range(0, roomWidth - 1), -1) + newRoomPos;
        GameObject newDoor = Instantiate(doorPrefabs[Random.Range(0, doorPrefabs.Length)], doorPos, Quaternion.identity);
        newDoor.transform.parent = newRoom.transform;

        // Generate furniture
        int numFurniture = Random.Range(minFurniture, maxFurniture);
        for (int i = 0; i < numFurniture; i++)
        {
            Vector2 furniturePos = new Vector2(Random.Range(0, roomWidth - 1), Random.Range(0, roomHeight - 1)) + newRoomPos;
            GameObject newFurniture = Instantiate(furniturePrefabs[Random.Range(0, furniturePrefabs.Length)], furniturePos, Quaternion.identity);
            newFurniture.transform.parent = newRoom.transform;
        }

        // Generate enemies
        int numEnemies = Random.Range(minEnemies, maxEnemies);
        for (int i = 0; i < numEnemies; i++)
        {
            Vector2 enemyPos = new Vector2(Random.Range(0, roomWidth - 1), Random.Range(0, roomHeight - 1)) + newRoomPos;
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemyPos, Quaternion.identity);
            newEnemy.transform.parent = newRoom.transform;
        }
    }
}