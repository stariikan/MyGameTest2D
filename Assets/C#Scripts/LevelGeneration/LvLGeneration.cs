using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvLGeneration : MonoBehaviour
{
    // � ��� ��� ���������� ������� ������ ������ ����� �� �����������: ���������, �������������, �������������2 � ��������.
    public Sprite startBlock;
    public Sprite midBlock;
    public Sprite midBlock2;
    public Sprite endBlock;
    public GameObject enemyForGeneration;
    public GameObject trapsForGeneration;

    private int completeLevels = 0; // ������� ���� �������� ���������� completeLevels, � ������� ������� ����� ��������� ���������� ���������� �������.

    private void Start() // � ������ Start �� ����� ��������� ��������� ������ �� ����� ������ ����.
    {
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

        int count = this.completeLevels + 10; // �������� ���������� count ����� ��������� ����� ��� - �� ������������� ������ ���������� ���������, ��� ����� ����� �������� �� ���������� ���������� ������� �, ����� �� ���������� �� ���� ������� ���� �� ������ �������, ��� ����(5) �������������� ������.

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
            position.y += size.y * Random.Range(-2, 2); //������� �� Y, ���������
            newBlock.transform.position = position; // ����������� ������� ������� �������

            GameObject enemy = Instantiate(enemyForGeneration, new Vector2(position.x + Random.Range(-2, -1), position.y + 4), Quaternion.identity); //������������ ������� (����) � ��� ����������)
            enemy.name = "Enemy" + Random.Range(1, 100);
            Instantiate(trapsForGeneration, new Vector2(position.x + Random.Range(0, 1), position.y + 0.8f), Quaternion.identity);// ������������ �������(�������) � ��� ����������)

            yield return new WaitForEndOfFrame(); //�������� ��������� ������
        }

        newBlock = new GameObject("End block");// ������� ����� ������
        renderer = newBlock.AddComponent<SpriteRenderer>();//��������� ��������� SpriteRenderer
        BoxCollider2D boxCollider = newBlock.AddComponent<BoxCollider2D>();//��������� ��������� BoxCollider2D
        boxCollider.size = new Vector2(1.274357f, 0.1442559f);//������ ������ BoxCollider2D 
        renderer.sprite = this.endBlock;//���������� ������ ������� �� �������� � endBlock

        position.x += size.x; //������� �� X, ����� ������ ���� ���� ������ ��� �������
        position.y += size.y * Random.Range(-1, 1); //������� �� Y, ���������
        newBlock.transform.position = position; // ����������� ������� ������� �������
        newBlock.transform.localScale = size; //������ ������ �������

        yield return new WaitForEndOfFrame(); //�������� ��������� ������
    }
    public void CompleteLevel() // ������� ����� CompleteLevel, ������� ����� ����������� ���������� completeLevels �� ���� ������� ������ ���, ����� ����� ������� ��������� �������.
    {
        this.completeLevels += 1;
        StartCoroutine(OnGeneratingRoutine());
    }
}
