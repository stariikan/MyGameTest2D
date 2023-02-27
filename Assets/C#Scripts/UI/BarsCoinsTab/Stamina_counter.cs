using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class Stamina_counter : MonoBehaviour
{
    public Image progressBarStamina;
    public float maxProgress = 100f;
    private float currentProgress;

    void Start()
    {
        currentProgress = maxProgress;
    }

    public void UpdateProgressBar()
    {
        currentProgress = HeroAttack.Instance.currentStamina;

        if (progressBarStamina == null)
        {
            Debug.LogError("progressBarStamina is not set!");
            return;
        }
        else
        {
            progressBarStamina.fillAmount = currentProgress / maxProgress;
        }
    }
    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        UpdateProgressBar();
    }
}
