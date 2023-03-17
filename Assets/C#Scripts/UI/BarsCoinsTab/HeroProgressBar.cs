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
        currentProgress = Hero.Instance.hp; //������� ����� ������ ���-�� ��
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
        maxProgress = HeroAttack.Instance.maxMP;
        currentProgress = HeroAttack.Instance.currentMP;

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
        maxProgress = HeroAttack.Instance.stamina;
        currentProgress = HeroAttack.Instance.currentStamina;

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
