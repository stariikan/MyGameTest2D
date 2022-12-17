using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static Hero Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    public float maxSpeed = 10f; //Максимальная скорость
    private bool flipRight = true; //Поворот спрайта на право, состояние = правда, нужно для поворота спрайта во время смены движения
    public bool isGrounded = true; //Находиться ли обьект на земле, а точнее соприкосается ли он с другим обьектом имеющим Collision2D 
    public float gravityScale = 10; //Сила притяжения или чем ниже тем выше прыжок
    public float fallingGravityScale = 40; //Сила притяжение при падении чем выше тем сильнее игровой обьекс тянет вниз
    [SerializeField] public int hp = 100; //Количество жизней
    private Rigidbody2D rb; //Тело с физической переменной к которому принадлежит скрипт, переменная = rb
    private Animator anim; //Переменная благодаря которой анимирован обьект, переменная = anim
    private States State //Создание стейтмашины, переменная = State. Значение состояния может быть передано или изминено извне благодаря get и set
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
    public enum States //Определения какие бывают состояния, указал названия как в Аниматоре Unity
    {
        idle,
        run,
        jump 
    }
    public void GetDamage() //Мы создаем новый метод GetDamage() 
                            //Пишет изменившееся значение в лог и 
    {
        hp -= 10;//Отнимает int 10 из переменной hp (жизни).
        Debug.Log(hp);//Пишет изменившееся значение в лог
        if (hp < 0) //Если жизней меньше 0,
        {
            Destroy(this.gameObject);//то смерть и уничтожение gameObject, это публичный метод из скрипта Entity 
        }
    }

    public void Hero_hp() //Метод который просто вызывает значение переменной HP, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(hp);
    }
    void Awake() //Awake используется для инициализации любых переменных или игрового состояния перед началом игры.
                 //Awake вызывается только один раз за все время существования экземпляра сценария.
                 //Вызов Awake происходит после инициализации всех объектов, поэтому можно безопасно обращаться к другим объектам
                 //или запрашивать их, используя, например, GameObject.
    {
        rb = GetComponent<Rigidbody2D>(); //Переменная rb получает компонент Rigidbody2D (Физика game.Object)
                                          //к которому привязан скрипт
        anim = GetComponent<Animator>(); //Переменная anim получает информацию из компонента Animator (Анимация game.Object)
                                         //к которому привязан скрипт
        Instance = this; //'this' - это ключевое слово, обозначающее класс, в котором выполняется код.
                         //Насколько мне известно, оно никогда не требуется, но делает код более читабельным this. transform. position and transform.
    }
    private void OnCollisionEnter2D(Collision2D collision) //OnCollisionEnter вызывается, когда этот колайдер/тело начинает касаться другого тела/коллайдера.
                                                           //В отличие от OnTriggerEnter, OnCollisionEnter передается класс Collision, а не Collider.
                                                           //Класс Collision содержит информацию, например, о точках контакта и скорости удара.
    {
        isGrounded = true; //Если персонаж касается другого тела, считается что он на земле
    }

    void Update() //Update = выполнение функции каждый каждый кадр.
    {
        float move = Input.GetAxis("Horizontal");//Используем Float потому-что значение 0.111..., тут берется ввод по Горизонтали (стрелки и A D)
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);//Тут указно что берется компонент Rigidbody2D
                                                                                                                    //у нашего game.Object и благодоря new Vector2
                                                                                                                    //изминяетя позиция game.Object помноженая (*)
                                                                                                                    //максимальную скорость которую мы указали в переменной
                                                                                                                    //по оси x
                                                                                                                    //velocity = Единица часто считаются метрами, но могут быть миллиметрами или световыми годами.
                                                                                                                    //Также имеет скорость в X, Y и Z, определяя направление.

        if (move > 0 && !flipRight) //если движение больше нуля и произшло flipRight =не true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        else if (move < 0 && flipRight) //если движение больше нуля и произшло flipRight = true то нужно вызвать метод Flip (поворот спрайта)
        {
            Flip();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)// если происходит нажатие и отпускания (GetKeyDown, а не просто GetKey)
                                                          // кнопки Space и если isGrounded = true 
        {
            isGrounded = false; // то isGrounded меняется на false 
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1000, 0)); //берется компонент Rigidbody2D у game.Object
                                                                           //и добавляется усилие по (new Vetor3) в направлении вверх Y
        }
        if(rb.velocity.y >= 0) //Если скорость тела по оси Y больше или равно 0, то
        {
            rb.gravityScale = gravityScale; //тут как раз получается что от параметра задданного в переменной будет зависеть сила прыжка
        }
        else if (rb.velocity.y < 0) //если скорость персонажа по оси Y меньше 0 то
        {
            rb.gravityScale = fallingGravityScale; //гравитация начинает притягивать обьект в низ в зависимости
                                                   //от числа который мы ввели в переменной fallingGravityScale
        }
        if (isGrounded) State = States.idle;//если мы на земле State = idle
        if (Input.GetButton("Horizontal")) State = States.run;//если мы нажимаем на кнопки (стрелки или A D) то State = run
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) State = States.jump; //если мы нажимаем Space и мы на земле то State = jump
        if (!isGrounded) State = States.jump; //и если мы не на земле State = jump. Это все нужно чтобы менялась анимация

    }
        private void Flip() //Тут мы создаем метод Flip при вызове которого спрайт меняет направление
    {
        flipRight = !flipRight; //Когда запускается метод Flip переменная flipRight меняется на false
        Vector3 theScale = transform.localScale; //получение масштаб объекта
        theScale.x *= -1;//тут происходит переворот изображения например 140 меняется на -140 тем самым полностью измени направление спрайта (картинка отзеркаливается)
        transform.localScale = theScale; //Масштаб преобразования относительно родительского объекта GameObjects
    }


}

