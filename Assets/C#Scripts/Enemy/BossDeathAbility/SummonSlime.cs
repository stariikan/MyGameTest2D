using System.Collections;
using UnityEngine;

public class SummonSlime : MonoBehaviour
{
    public GameObject[] guards;
    public float direction;//directional variable
    public Rigidbody2D rb; //physical body
    private Animator anim; //variable by which the object is animated
    public static SummonSlime Instance { get; set; } //to collect and send data from this script

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>(); //variable rb gets the Rigidbody2D component (physics game.Object) to which the script is bound
        anim = this.gameObject.GetComponent<Animator>(); //variable anim receives information from the Animator component (animation.Object) to which the script is bound
    }
    public void SummonGuards()
    {
        Vector3 pos = transform.position;
        GameObject guard1 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1.5f, pos.y, pos.z), Quaternion.identity); // object (enemy) and its coordinates)
        guard1.name = "Enemy" + Random.Range(1, 999);
        GameObject guard2 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 1f, pos.y, pos.z), Quaternion.identity); // object (enemy) and its coordinates)
        guard2.name = "Enemy" + Random.Range(1, 999);
        GameObject guard3 = Instantiate(guards[Random.Range(0, guards.Length)], new Vector3(pos.x + 2f, pos.y, pos.z), Quaternion.identity); // object (enemy) and its coordinates)
        guard3.name = "Enemy" + Random.Range(1, 999);

    }
    public void SummonDirection(Vector3 _direction)// selecting a flight direction 
    {
        this.gameObject.SetActive(true); //activate the game object
        this.gameObject.transform.position = _direction;
        anim.SetTrigger("summon");
    }
    public void SummonOff()
    {
        this.gameObject.SetActive(false);
    }
}
