using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{

    private float maxHP = 1; //Maximum lives
    public float currentHP;
    public static SpellBook Instance { get; set; } // To collect and send data from this script
    private Animator anim;
    public bool chestOpen = false;
    public int rewardForKill = 20;// reward for defeating the enemy
    public enum States //Defining what states there are, named as in Unity Animator
    {
        idle,
        open
    }
    private States State //State machine creation, variable = State. State value can be passed or changed externally via get and set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    private void Start()
    {
        currentHP = maxHP;
        Instance = this;
        chestOpen = false;
        anim = GetComponent<Animator>(); //The anim variable takes information from the Animator component (Animator.Object)
                                         //to which the script is bound
    }
    public void TakeDamage(float dmg) //Method for receiving damage where (int dmg) this value can be entered when the method is called (i.e. damage can be entered there)
    {
        if (currentHP > 0)
        {
            anim.SetTrigger("open");// damage animation
            currentHP -= dmg;
            Debug.Log(currentHP + " " + gameObject.name);
        }
        else
        {
            return;
        }

        if (currentHP <= 0)
        {
            LvLGeneration.Instance.PlusCoin(rewardForKill); //call for a method to increase points
            anim.SetTrigger("open");//death animation
            chestOpen = true;
            Debug.Log("Open" + gameObject.name);
        }
    }
    public virtual void Die() //Method removes this game object, called by the animator immediately after the death animation ends
    {
        Destroy(this.gameObject); ;//destroy this game object
    }
}
