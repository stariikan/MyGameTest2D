using UnityEngine;
using UnityEngine.UI;//��� UI

public class HeroProgressBar : MonoBehaviour
{
    public Image progressBar; //�������� �������� ����
    public float maxProgress; //100% ������������ �������� ����
    private float currentProgress; //�� � �������� �������

    void Update() //���������� �������� ���������� ��� ���������� ������� �����
    {
        if (gameObject.name == "HP") UpdateHPProgressBar();
        if (gameObject.name == "MP") UpdateMPProgressBar();
        if (gameObject.name == "Stamina") UpdateStaminaProgressBar();
    }
    public void UpdateHPProgressBar()
    {
        maxProgress = Hero.Instance.maxHP;
        currentProgress = Hero.Instance.curentHP; //������� ����� ������ ���-�� ��
        if (progressBar == null) // �������� �� ��, ������� �������� �������� ���� ��� ���
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBar.fillAmount = currentProgress / maxProgress; //��������� ���������� (�� �������� � ����� ��������� ��������� � ����������� fillAmount (������ 90 ��/100 �� =0,9 ������� ����������� �� 10%
        }
    }
    public void UpdateMPProgressBar()
    {
        maxProgress = Hero.Instance.maxMP;
        currentProgress = Hero.Instance.currentMP;

        if (progressBar == null)
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBar.fillAmount = currentProgress / maxProgress;
        }
    }
    public void UpdateStaminaProgressBar()
    {
        maxProgress = Hero.Instance.stamina;
        currentProgress = Hero.Instance.currentStamina;

        if (progressBar == null)
        {
            Debug.LogError("progressBarStamina is not set!");
            return;
        }
        else
        {
            progressBar.fillAmount = currentProgress / maxProgress;
        }
    }
}
