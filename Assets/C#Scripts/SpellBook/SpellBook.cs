using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{

    private float maxHP = 1; //Максимальные жизни скелета
    public float currentHP;
    public static SpellBook Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    private Animator anim;
    public bool chestOpen = false;
    public int rewardForKill = 20;//награда за победу над врагом
    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        open
    }
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        chestOpen = false;
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
    }
    public void TakeDamage(float dmg) //Метод для получения дамага где (int dmg) это значение можно будет вводить при вызове метода (то есть туда можно будет вписать урон)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("open");//анимация получения демейджа
            currentHP -= dmg;
            Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKill); //вызов метода для увелечения очков
            anim.SetTrigger("open");//анимация смерти
            chestOpen = true;
            Debug.Log("Open" + gameObject.name);
        }
    }
    public virtual void Die() //Метод удаляет этот игровой обьект, вызывается через аниматор сразу после завершения анимации смерти
    {
        Destroy(this.gameObject); ;//уничтожить этот игровой обьект
    }
}
