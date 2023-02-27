using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//для UI

public class mp_counter : MonoBehaviour
{
    public Image progressBarMP;
    public float maxProgress = 100f;
    private float currentProgress;

    void Start()
    {
        currentProgress = maxProgress;
    }

    public void UpdateProgressBar()
    {
        currentProgress = HeroAttack.Instance.currentMP;
        
        if (progressBarMP == null)
        {
            Debug.LogError("progressBarMP is not set!");
            return;
        }
        else
        {
            progressBarMP.fillAmount = currentProgress / maxProgress;
        }
    }
    void Update() //Обновление значения происходит при обновлении каждого кадра
    {
        UpdateProgressBar();
    }
}
