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

    private int completeLevels = 0; // ������� ���� �������� ���������� completeLevels, � ������� ������� ����� ��������� ���������� ���������� �������.

    private void Start() // � ������ Start �� ����� ��������� ��������� ������ �� ����� ������ ����.
    {
        StartCoroutine(OnGeneratingRoutine());
    }
    private IEnumerator OnGeneratingRoutine() //� ������ OnGeneratingRoutine, ����� ��������� ��� ������� ��������� ������. ��� ��� ������ � ��� ����� ���� ��� ��������, ��� � ���������� � �������������� ������ ���������� �������, ������� ��������� �� �������� � ��������, ����� ���� �� ���������� �� ����� ������ ������������
    {
        Vector2 size = new Vector2(1, 1); //��� ������ � ������ OnGeneratingRoutine ������� ��� ��������� ����������: size, ��� ������ ������ ������ �� ����� � ������ � position, ��� ������ �����, ������ ����� �������� �������� �������. ������ ����� ��������� ��������� ����.
        Vector2 position = new Vector2(0, 0);

        GameObject newBlock = new GameObject("Start block");
        newBlock.transform.position = position;
        newBlock.transform.localScale = size;
        SpriteRenderer renderer = newBlock.AddComponent<SpriteRenderer>();
        BoxCollider2D boxCollider2D = newBlock.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(1.274357f, 0.1442559f);
        renderer.sprite = this.startBlock;

        int count = this.completeLevels + 5; // �������� ���������� count ����� ��������� ����� ��� - �� ������������� ������ ���������� ���������, ��� ����� ����� �������� �� ���������� ���������� ������� �, ����� �� ���������� �� ���� ������� ���� �� ������ �������, ��� ����(5) �������������� ������.

        // ����� ��� �� ������� ��������� ����, ����� ������ � �������������: ������� ����� GameObject, ��������� ��� ��������� SpriteRenderer, ��������� ������ ��� ����������� �� ����� � ������ ������ � �������.
        // ��� ��� ������������� ����� �������� �� �����������, ������ � ������� ���������� � ������ ����� ������ �������� ������� ������.��� ���� ����� ������ �� ������� �� ���������� ��������, ������������� ���������� size, ��� ������� ������� ������.
        // ������ ����� �� Y � ���������� position ����� ��������� �����, ���� ����, � ����������� �� ������� �����, ����������� �� ��������� ����� �� -1 �� 1. ����� Random.Range ���������� ����� ����� �� ������������ �� ����������� (�������������), ��� ������, ��� ������������ ��������� ����� ������� ���������� �� �����. ��������� ���� ��������� ������������� ������ ����� WaitForEndOfFrame.
        for (int i = 0; i < count; i++)
        {
            newBlock = new GameObject("Middle block");
            renderer = newBlock.AddComponent<SpriteRenderer>();
            BoxCollider2D collider2D = newBlock.AddComponent<BoxCollider2D>();
            collider2D.size = new Vector2 (1.274357f, 0.1442559f);
            renderer.sprite = this.midBlock;

            newBlock.transform.localScale = size;
            position.x += size.x;
            position.y += size.y * Random.Range(-1, 2);
            newBlock.transform.position = position;

            yield return new WaitForEndOfFrame();
        }

        // ��������� � �������������� ����� ��������� ������ � �������� �����������, ��������� �����. ����� ��� � ��������� ����, �� ��������� ��������, �� ����� ��� � ������������� � � ������� ��������� ��������� �� ������.

        newBlock = new GameObject("End block");
        renderer = newBlock.AddComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = newBlock.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1.274357f, 0.1442559f);
        renderer.sprite = this.endBlock;

        position.x += size.x;
        position.y += size.y * Random.Range(-1, 2);
        newBlock.transform.position = position;
        newBlock.transform.localScale = size;

        yield return new WaitForEndOfFrame();
    }
    public void CompleteLevel() // ������� ����� CompleteLevel, ������� ����� ����������� ���������� completeLevels �� ���� ������� ������ ���, ����� ����� ������� ��������� �������.
    {
        this.completeLevels += 1;
        StartCoroutine(OnGeneratingRoutine());
    }
}
