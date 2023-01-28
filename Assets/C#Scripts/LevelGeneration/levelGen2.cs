using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class levelGen2 : MonoBehaviour
{
    //This script generates a castle with multiple rooms using a for loop, each iteration of the loop generates a new room. Inside the GenerateRoom() function, it uses nested for loops to generate the walls and floor for the room, and also generates a random door, random furniture and random enemies. The script uses a number of public variables to control the size of the rooms, the number of rooms, and the number of furniture and enemies.
    //You can use this script and attach it to a empty game object in your scene, and fill the wallPrefabs, floorPrefabs, doorPrefabs, furniturePrefabs, and enemyPrefabs arrays with the sprites you want to use for the level generation.
    //Please note that this is a complex example of level generation, it might require additional functionality such as collision detection, user input
    public static levelGen2 Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    public int Level = 1; // ������� ���� �������� ���������� completeLevels, � ������� ������� ����� ��������� ���������� ���������� �������.
    public int coin; // ���-�� �����

    public GameObject[] wallPrefabs; //�����
    public GameObject[] floorPrefabs; //���
    //public GameObject[] doorPrefabs; //�����
    //public GameObject[] furniturePrefabs; //������
    public GameObject [] enemyPrefabs; // �����
    /// �� ��� ��� �������� � ������ ��������� ��� ������� �������
    public int roomWidth; //������ ����
    int maxRooms; // max ���-�� ���
    public int minFurniture; // ��� ���-�� ������
    public int maxFurniture; // ������������ ���-�� ������

    private Vector3 lastRoomPos;
    private List<GameObject> generatedRooms;

    private void Start()
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        if (Level == 0)
        {
            Level = 1;
        }
        
        generatedRooms = new List<GameObject>();
        lastRoomPos = transform.position;

        StartCoroutine(OnGeneratingRoutine());
    }

    private IEnumerator OnGeneratingRoutine() //� ������ OnGeneratingRoutine, ����� ��������� ��� ������� ��������� ������. ��� ��� ������ � ��� ����� ���� ��� ��������, ��� � ���������� � �������������� ������ ���������� �������, ������� ��������� �� �������� � ��������, ����� ���� �� ���������� �� ����� ������ ������������
    {
        Vector3 newRoomPos = lastRoomPos;
        newRoomPos.x += Random.Range(0, roomWidth);// ������� ����� ������� �� x
        newRoomPos.z = 110; // ������� ����� ������� �� Z

        GameObject newRoom = new GameObject("Room");
        newRoom.transform.position = newRoomPos;
        generatedRooms.Add(newRoom);
        lastRoomPos = newRoomPos;
        int numEnemies = maxRooms;
        int count = this.Level + 3;
        // Generate roomss
        for (int i = 0; i < count; i++)
        {
            int x = 0 + count;
            int y = 0;
            int z = 110;
            GameObject newWall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], new Vector3(x * 16, y, z) + newRoomPos, Quaternion.identity); //��������� ������
            GameObject newFloor = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Length)], new Vector3(((x + 0.5f) * 16), y, z + 1) + newRoomPos, Quaternion.identity); //��������� ���������
            
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, floorPrefabs.Length)], new Vector3(x * 16 , -3, 0), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            enemy.name = "Enemy" + Random.Range(1, 100);
        }

        // Generate door
        //Vector3 doorPos = new Vector3(Random.Range(0, roomWidth - 1) * 5, -1 * 5) + newRoomPos;
        //GameObject newDoor = Instantiate(doorPrefabs[Random.Range(0, doorPrefabs.Length)], doorPos, Quaternion.identity);

        // Generate furniture
        //int numFurniture = Random.Range(minFurniture, maxFurniture);
        //for (int i = 0; i < numFurniture; i++)
        //{
        //    Vector2 furniturePos = new Vector3(Random.Range(0, roomWidth - 1) * 5, Random.Range(0, roomHeight - 1) * 5) + newRoomPos;
        //    GameObject newFurniture = Instantiate(furniturePrefabs[Random.Range(0, furniturePrefabs.Length)], furniturePos, Quaternion.identity);
        //}
        yield return new WaitForEndOfFrame(); //�������� ��������� ������
    }
    public void PlusCoin(int count) //������� ����� ��������� �������
    {
        coin += count;
    }
    public void coin_Counter() //����� ������� ������ �������� �������� ���������� Coin, ����� ��� ��� ��� �������� ����� ����� � ������ � ��������� ������
    {
        Debug.Log(coin);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Restart()
    {
        Level = 1;
        SaveSerial.Instance.ResetData();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
    }
    public void CompleteLevel() // ������� ����� CompleteLevel, ������� ����� ����������� ���������� completeLevels �� ���� ������� ������ ���, ����� ����� ������� ��������� �������.
    {
        this.Level += 1;
        //Debug.Log("Level: " + Level);
        GameObject.Find("EnemySkelet").GetComponent<Entity>().BoostHP();
        GameObject.Find("EnemySkelet").GetComponent<Entity>().BoostAttackDamage();
        GameObject.Find("EnemySkelet").GetComponent<Enemy_Skelet>().BoostSpeed();
        SaveSerial.Instance.SaveGame();
        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Single);
        //StartCoroutine(OnGeneratingRoutine());
    }
    private void Update()
    {
        if (Hero.Instance.playerDead == true)
        {
            Restart();
        }
        if (Chest.Instance.chestOpen == true)
        {
            CompleteLevel();
        }
    }
}