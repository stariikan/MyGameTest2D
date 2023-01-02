using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ����������� �������� �� �������
using UnityEngine.SceneManagement; 


public class LvLGeneration : MonoBehaviour
{
    // � ��� ��� ���������� ������� ������ ������ ����� �� �����������: ���������, �������������, �������������2 � ��������.
    public Sprite startBlock;
    public Sprite midBlock;
    public Sprite midBlock2;
    public Sprite endBlock;
    public GameObject enemyForGeneration;
    public GameObject trapsForGeneration;
    public GameObject chestForGeneration;

    public static LvLGeneration Instance { get; set; } //��� ����� � �������� ������ �� ����� �������

    public int Level = 1; // ������� ���� �������� ���������� completeLevels, � ������� ������� ����� ��������� ���������� ���������� �������.
    public int coin; // ���-�� �����

    private void Start() // � ������ Start �� ����� ��������� ��������� ������ �� ����� ������ ����.
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        if (Level == 0)
        {
            Level = 1;
        }
        StartCoroutine(OnGeneratingRoutine());
    }
    private IEnumerator OnGeneratingRoutine() //� ������ OnGeneratingRoutine, ����� ��������� ��� ������� ��������� ������. ��� ��� ������ � ��� ����� ���� ��� ��������, ��� � ���������� � �������������� ������ ���������� �������, ������� ��������� �� �������� � ��������, ����� ���� �� ���������� �� ����� ������ ������������
    {
        Vector2 size = new Vector2(4, 4); //��� ������ � ������ OnGeneratingRoutine ������� ��� ��������� ����������: size, ��� ������ ������ ������ �� ����� � ������ � position, ��� ������ �����, ������ ����� �������� �������� �������. ������ ����� ��������� ��������� ����.
        Vector2 position = new Vector2(0, 0);

        GameObject newBlock = new GameObject("Start block");// ������� ����� ������
        newBlock.transform.position = position;// ����������� ������� ������� �������
        newBlock.transform.localScale = size;// ����������� ������� � ����������� �� ��������
        SpriteRenderer renderer = newBlock.AddComponent<SpriteRenderer>(); //��������� ��������� SpriteRenderer
        BoxCollider2D boxCollider2D = newBlock.AddComponent<BoxCollider2D>();//��������� ��������� BoxCollider2D
        boxCollider2D.size = new Vector2(1.274357f, 0.1442559f);//������ ������ BoxCollider2D 
        renderer.sprite = this.startBlock;//���������� ������ ������� �� �������� � startBlock

        int count = this.Level + 3; // �������� ���������� count ����� ��������� ����� ��� - �� ������������� ������ ���������� ���������, ��� ����� ����� �������� �� ���������� ���������� ������� �, ����� �� ���������� �� ���� ������� ���� �� ������ �������, ��� ����(5) �������������� ������.

        // ����� ��� �� ������� ��������� ����, ����� ������ � �������������: ������� ����� GameObject, ��������� ��� ��������� SpriteRenderer, ��������� ������ ��� ����������� �� ����� � ������ ������ � �������.
        // ��� ��� ������������� ����� �������� �� �����������, ������ � ������� ���������� � ������ ����� ������ �������� ������� ������.��� ���� ����� ������ �� ������� �� ���������� ��������, ������������� ���������� size, ��� ������� ������� ������.
        // ������ ����� �� Y � ���������� position ����� ��������� �����, ���� ����, � ����������� �� ������� �����, ����������� �� ��������� ����� �� -1 �� 1. ����� Random.Range ���������� ����� ����� �� ������������ �� ����������� (�������������), ��� ������, ��� ������������ ��������� ����� ������� ���������� �� �����. ��������� ���� ��������� ������������� ������ ����� WaitForEndOfFrame.
        for (int i = 0; i < count; i++)
        {
            newBlock = new GameObject("Middle block");// ������� ����� ������
            renderer = newBlock.AddComponent<SpriteRenderer>();//��������� ��������� SpriteRenderer
            BoxCollider2D collider2D = newBlock.AddComponent<BoxCollider2D>();//��������� ��������� BoxCollider2D
            collider2D.size = new Vector2 (1.274357f, 0.1442559f);//������ ������ BoxCollider2D 
            renderer.sprite = this.midBlock;//���������� ������ ������� �� �������� � midBlock

            newBlock.transform.localScale = size; //������ ������ �������
            position.x += size.x; //������� �� X, ����� ������ ���� ���� ������ ��� �������
            position.y += size.y * Random.Range(-0.5f, 0.5f); //������� �� Y, ���������
            newBlock.transform.position = position; // ����������� ������� ������� �������

            GameObject enemy = Instantiate(enemyForGeneration, new Vector2(position.x + Random.Range(-1, 2), position.y + 4), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            enemy.name = "Enemy" + Random.Range(1, 100);
            //Instantiate(trapsForGeneration, new Vector2(position.x + Random.Range(0, 1), position.y + 0.8f), Quaternion.identity);// ������������ �������(�������) � ��� ����������)

            yield return new WaitForEndOfFrame(); //�������� ��������� ������
        }

        newBlock = new GameObject("End block");// ������� ����� ������
        renderer = newBlock.AddComponent<SpriteRenderer>();//��������� ��������� SpriteRenderer
        BoxCollider2D boxCollider = newBlock.AddComponent<BoxCollider2D>();//��������� ��������� BoxCollider2D
        boxCollider.size = new Vector2(1.274357f, 0.1442559f);//������ ������ BoxCollider2D 
        renderer.sprite = this.endBlock;//���������� ������ ������� �� �������� � endBlock
        Instantiate(chestForGeneration, new Vector2(position.x + Random.Range(0, 1), position.y + 0.8f), Quaternion.identity);

        position.x += size.x; //������� �� X, ����� ������ ���� ���� ������ ��� �������
        position.y += size.y * Random.Range(-1, 1); //������� �� Y, ���������
        newBlock.transform.position = position; // ����������� ������� ������� �������
        newBlock.transform.localScale = size; //������ ������ �������

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
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
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
