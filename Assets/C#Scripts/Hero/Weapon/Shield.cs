using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Shield Instance { get; set; } // To collect and send data from this script
    public float direction;//directional variable
    [SerializeField] private float lifetime; // shield life time

    private BoxCollider2D boxCollider; //Strike collider

    public string TargetName;
    public GameObject target;


    private void Awake() //The action is performed before the start of the game and 1 time
    {
        //anim = GetComponent<Animator>(); // вытаскиваем информацию из компанента аниматор
        boxCollider = GetComponent<BoxCollider2D>(); // pull information from the box colider component
        Instance = this;
    }

    private void Update()
    {
        lifetime += Time.deltaTime; //increase the lifetime variable every second +1
        if (lifetime > 1.5f)
        {
            this.gameObject.SetActive(false);// when the variable reaches 1.5, the attack collider disappears
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetName = collision.gameObject.name;
        boxCollider.enabled = false; //disconnect the collider
        this.gameObject.SetActive(false);// when the variable reaches 1.5, the attack collider disappears
        target = GameObject.Find(TargetName);
        Debug.Log(target);
        if (target.CompareTag("Bomb")) target.GetComponent<Bomb>().PushFromPlayer();
        if (target.CompareTag("EvilWizard")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target.CompareTag("Mushroom")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target.CompareTag("Martial")) target.GetComponent<Enemy_Behavior>().Stun();
        if (target != null && target.layer == 7) target.GetComponent<Enemy_Behavior>().PushFromPlayer();
    }
    public void MeleeDirection(Vector3 _direction)// selecting a flight direction 
    {
        lifetime = 0;
        gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
        boxCollider.enabled = true; //activating the collider
    }
}
