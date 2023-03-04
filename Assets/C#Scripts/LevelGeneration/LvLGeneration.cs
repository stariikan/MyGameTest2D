using System.Collections;
using UnityEngine;

//��� ����������� �������� �� �������
using UnityEngine.SceneManagement; 


public class LvLGeneration : MonoBehaviour
{
    // � ��� ��� ���������� ������� ������ ������ ����� �� �����������: ���������, �������������, �������������2 � ��������.
    public GameObject[] startBlock; //��������� �������
    public GameObject[] startBorder; //��������� ����� �������
    public GameObject[] midBlock; //���� ������� ��� ��������� �����
    public GameObject[] borderMid; //���� ����� ������� ��� ��������� �����
    public GameObject[] endBlock; //������� � ��������
    public GameObject[] endBorder; // ����� ������� � ��������
    public GameObject[] enemyForGeneration;
    public GameObject[] trapsForGeneration;
    public GameObject[] chestForGeneration;
    public GameObject[] powerUpForGeneration;
    public static LvLGeneration Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    public int Level = 1; // ������� ���� �������� ���������� completeLevels, � ������� ������� ����� ��������� ���������� ���������� �������.
    public int coin; // ���-�� �����
    public bool key = false;

    private void Start() // � ������ Start �� ����� ��������� ��������� ������ �� ����� ������ ����.
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        if (Level == 0)
        {
            Level = 1;
        }
        StartCoroutine(OnGeneratingRoutine()); //������� ��������� ������
    }
    private void Update()
    {
        if (Hero.Instance.playerDead == true)
        {
            DeadScreen();
        }
        if (Chest.Instance.chestOpen == true)
        {
            CompleteLevel();
        }
    }
    private IEnumerator OnGeneratingRoutine() //� ������ OnGeneratingRoutine, ����� ��������� ��� ������� ��������� ������. ��� ��� ������ � ��� ����� ���� ��� ��������, ��� � ���������� � �������������� ������ ���������� �������, ������� ��������� �� �������� � ��������, ����� ���� �� ���������� �� ����� ������ ������������
    {
        Vector3 position = new Vector3(0, 2, 110);
        position.x = 0; //������� �� X, ����� ������ ���� ���� ������ ��� �������
        position.y = 2; //������� �� Y
        position.z = 110;

        GameObject newstartBlock = Instantiate(startBlock[Random.Range(0, startBlock.Length)]); //������� ������� �������
        newstartBlock.name = "Start block";// ������� ����� ������
        newstartBlock.transform.position = new Vector3(position.x, position.y, position.z);// ����������� ������� ������� �������
        newstartBlock.layer = LayerMask.NameToLayer("Ground"); //���������� ���� ����� � ���������� �����
        GameObject newBorderStart = Instantiate(startBorder[Random.Range(0, startBorder.Length)], new Vector3(position.x, position.y, position.z - 5), Quaternion.identity);


        int count = this.Level; // �������� ���������� count ����� ��������� ����� ��� - �� ������������� ������ ���������� ���������, ��� ����� ����� �������� �� ���������� ���������� ������� �, ����� �� ���������� �� ���� ������� ���� �� ������ �������, ��� ����(5) �������������� ������.

        for (int i = 0; i < count; i++)
        {


            position.x += 9.8f; //������� �� X, ����� ������ ���� ���� ������ ��� �������
            position.y = 2; //������� �� Y, ���������
            position.z = 110;
            GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // ������� ����� ������
            newMidBlock.name = "Middle block" + Random.Range(1, 999);
            newMidBlock.layer = LayerMask.NameToLayer("Ground");//���������� ���� ����� � ���������� �����
            GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

            GameObject enemy = Instantiate(enemyForGeneration[Random.Range(0, enemyForGeneration.Length)], new Vector3(position.x, position.y, position.z - 1), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            enemy.name = "Enemy" + Random.Range(1, 999);

            yield return new WaitForEndOfFrame(); //�������� ��������� ������
        }

        GameObject newEndBlock = Instantiate(endBlock[Random.Range(0, endBlock.Length)], new Vector3(position.x + 9.8f, position.y, 110), Quaternion.identity);
        newEndBlock.layer = LayerMask.NameToLayer("Ground");//���������� ���� ����� � ���������� �����
        newEndBlock.name = "End block";// ������� ����� ������
        GameObject newBorderEnd = Instantiate(endBorder[Random.Range(0, endBorder.Length)], new Vector3(position.x + 9.8f, position.y, 105), Quaternion.identity);


        GameObject newChest = Instantiate(chestForGeneration[Random.Range(0, chestForGeneration.Length)], new Vector3(position.x + 9.8f, position.y, position.z - 2), Quaternion.identity);
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
    public void FindKey() //������� ����� ��������� ������
    {
        key = true;
    }
    public void UseKey() 
    {
        key = false;        
    }
    public void key_Counter() //����� ������� ������ �������� �������� ���������� Coin, ����� ��� ��� ��� �������� ����� ����� � ������ � ��������� ������
    {
        Debug.Log(key);
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
    public void DeadScreen()
    {
        SceneManager.LoadScene("DeadScreen", LoadSceneMode.Single);
    }
    public void Boost_Moushroom()
    {
        GameObject.Find("Mushroom").GetComponent<Entity_Mushroom>().BoostHP();
        GameObject.Find("Mushroom").GetComponent<Entity_Mushroom>().BoostAttackDamage();
        GameObject.Find("Mushroom").GetComponent<Enemy_Mushroom>().BoostSpeed();
    }
    public void Boost_Skeleton()
    {
        GameObject.Find("Skeleton").GetComponent<Entity_Skeleton>().BoostHP();
        GameObject.Find("Skeleton").GetComponent<Entity_Skeleton>().BoostAttackDamage();
        GameObject.Find("Skeleton").GetComponent<Enemy_Skeleton>().BoostSpeed();
    }
    public void Boost_Goblin()
    {
        GameObject.Find("Goblin").GetComponent<Entity_Goblin>().BoostHP();
        GameObject.Find("Goblin").GetComponent<Entity_Goblin>().BoostAttackDamage();
        GameObject.Find("Goblin").GetComponent<Enemy_Goblin>().BoostSpeed();
    }
    public void CompleteLevel() // ������� ����� CompleteLevel, ������� ����� ����������� ���������� completeLevels �� ���� ������� ������ ���, ����� ����� ������� ��������� �������.
    {
        this.Level += 1;;
        //Boost Enemy
        Boost_Moushroom();
        Boost_Skeleton();
        Boost_Goblin();

        SaveSerial.Instance.SaveGame();
        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Single);
    }
}
