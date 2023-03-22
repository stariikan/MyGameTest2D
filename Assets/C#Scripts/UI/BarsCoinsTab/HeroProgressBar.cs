using UnityEngine;
using UnityEngine.UI;//для UI

public class HeroProgressBar : MonoBehaviour
{
    public Image progressBar; //картинка прогресс бара
    public float maxProgress; //100% заполнености прогресс бара
    private float currentProgress; //хп в реальном времени

    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        if (gameObject.name == "HP") UpdateHPProgressBar();
        if (gameObject.name == "MP") UpdateMPProgressBar();
        if (gameObject.name == "Stamina") UpdateStaminaProgressBar();
    }
    public void UpdateHPProgressBar()
    {
        maxProgress = Hero.Instance.maxHP;
        currentProgress = Hero.Instance.curentHP; //смотрим какое сейчас кол-во хп
        if (progressBar == null) // проверка на то, выбрана картинка прогресс бара или нет
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBar.fillAmount = currentProgress / maxProgress; //уменьшаем погрессбар (не забываем в юнити выставить настройки у изображения fillAmount (пример 90 хп/100 хп =0,9 полоска уменьшиться на 10%
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
