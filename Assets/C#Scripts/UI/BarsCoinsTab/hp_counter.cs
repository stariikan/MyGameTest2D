using UnityEngine;
using UnityEngine.UI;//для UI

public class hp_counter : MonoBehaviour
{
    public Image progressBarHP; //картинка прогресс бара
    public float maxProgress = 100f; //100% заполнености прогресс бара
    private float currentProgress; //хп в реальном времени

    void Start()
    {
        currentProgress = maxProgress; // вначале игры приравниваем кол-во хп к максимальному значению
    }
    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        UpdateProgressBar();
    }
    public void UpdateProgressBar()
    {
        currentProgress = Hero.Instance.hp; //смотрим какое сейчас кол-во хп
        if (progressBarHP == null) // проверка на то, выбрана картинка прогресс бара или нет
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBarHP.fillAmount = currentProgress / maxProgress; //уменьшаем погрессбар (не забываем в юнити выставить настройки у изображения fillAmount (пример 90 хп/100 хп =0,9 полоска уменьшиться на 10%
        }
    }
}
