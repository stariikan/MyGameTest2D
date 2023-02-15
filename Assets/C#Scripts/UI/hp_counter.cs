using UnityEngine;
using UnityEngine.UI;//��� UI

public class hp_counter : MonoBehaviour
{
    public Image progressBarHP; //�������� �������� ����
    public float maxProgress = 100f; //100% ������������ �������� ����
    private float currentProgress; //�� � �������� �������

    void Start()
    {
        currentProgress = maxProgress; // ������� ���� ������������ ���-�� �� � ������������� ��������
    }
    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        UpdateProgressBar();
    }
    public void UpdateProgressBar()
    {
        currentProgress = Hero.Instance.hp; //������� ����� ������ ���-�� ��
        if (progressBarHP == null) // �������� �� ��, ������� �������� �������� ���� ��� ���
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBarHP.fillAmount = currentProgress / maxProgress; //��������� ���������� (�� �������� � ����� ��������� ��������� � ����������� fillAmount (������ 90 ��/100 �� =0,9 ������� ����������� �� 10%
        }
    }
}
