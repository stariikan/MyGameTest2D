using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Для возможности работать со сценами
using UnityEngine.SceneManagement; 


public class LvLGeneration : MonoBehaviour
{
    // В эти три переменные занесем каждый спрайт блока по отдельности: стартовый, промежуточный, промежуточный2 и конечный.
    public Sprite startBlock;
    public Sprite midBlock;
    public Sprite midBlock2;
    public Sprite endBlock;
    public GameObject enemyForGeneration;
    public GameObject trapsForGeneration;
    public GameObject chestForGeneration;

    public static LvLGeneration Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    public int Level = 1; // Добавим одну числовую переменную completeLevels, с помощью которой будем указывать количество пройденных уровней.
    public int coin; // кол-во очков

    private void Start() // В методе Start мы будем запускать генерацию уровня во время старта игры.
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        if (Level == 0)
        {
            Level = 1;
        }
        StartCoroutine(OnGeneratingRoutine());
    }
    private IEnumerator OnGeneratingRoutine() //В методе OnGeneratingRoutine, будем выполнять сам процесс генерации уровня. Так как уровни у нас могут быть как большими, так и маленькими и генерироваться разное количество времени, процесс генерации мы поместим в корутину, чтобы игра не “зависала” во время работы “генератора”
    {
        Vector2 size = new Vector2(4, 4); //Для начала в методе OnGeneratingRoutine объявим две векторные переменные: size, где укажем размер блоков по длине и высоте и position, где укажем точку, откуда будет начинать строится уровень. Теперь можно построить стартовый блок.
        Vector2 position = new Vector2(0, 0);

        GameObject newBlock = new GameObject("Start block");// создаем новый обьект
        newBlock.transform.position = position;// присваиваем позицию новомоу обьекту
        newBlock.transform.localScale = size;// присваиваем разамер в соответсвие со спрайтом
        SpriteRenderer renderer = newBlock.AddComponent<SpriteRenderer>(); //добавляем компонент SpriteRenderer
        BoxCollider2D boxCollider2D = newBlock.AddComponent<BoxCollider2D>();//добавляем компонент BoxCollider2D
        boxCollider2D.size = new Vector2(1.274357f, 0.1442559f);//задаем размер BoxCollider2D 
        renderer.sprite = this.startBlock;//используем спрайт который мы засунули в startBlock

        int count = this.Level + 3; // Числовая переменная count будет указывать какое кол - во промежуточных блоков необходимо построить, это число будет зависеть от количества пройденных уровней и, чтобы их изначально не было слишком мало на первых уровнях, еще пяти(5) дополнительных блоков.

        // Также как мы строили стартовый блок, также строим и промежуточные: создаем новый GameObject, добавляем ему компонент SpriteRenderer, указываем спрайт для отображения на сцене и задаем размер и позицию.
        // Так как промежуточные блоки строятся по горизонтали, значит и позицию необходимо с каждым новым блоком сдвигать немного вправо.Для того чтобы узнать на сколько ее необходимо сдвинуть, воспользуемся переменной size, где указаны размеры блоков.
        // Высота блока по Y в переменной position также смещается вверх, либо вниз, в зависимости от размера блока, умноженного на случайное число от -1 до 1. Метод Random.Range генерирует ЦЕЛЫЕ числа от минимального до максимально (ИСКЛЮЧИТЕЛЬНО), это значит, что максимальное указанное число никогда достигнуто не будет. Завершаем цикл постройки промежуточных блоков новым WaitForEndOfFrame.
        for (int i = 0; i < count; i++)
        {
            newBlock = new GameObject("Middle block");// создаем новый обьект
            renderer = newBlock.AddComponent<SpriteRenderer>();//добавляем компонент SpriteRenderer
            BoxCollider2D collider2D = newBlock.AddComponent<BoxCollider2D>();//добавляем компонент BoxCollider2D
            collider2D.size = new Vector2 (1.274357f, 0.1442559f);//задаем размер BoxCollider2D 
            renderer.sprite = this.midBlock;//используем спрайт который мы засунули в midBlock

            newBlock.transform.localScale = size; //задаем размер обьекта
            position.x += size.x; //позиция по X, чтобы всегда была чуть дальше чем прошлый
            position.y += size.y * Random.Range(-0.5f, 0.5f); //позиция по Y, рандомная
            newBlock.transform.position = position; // присваиваем позицию новомоу обьекту

            GameObject enemy = Instantiate(enemyForGeneration, new Vector2(position.x + Random.Range(-1, 2), position.y + 4), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            enemy.name = "Enemy" + Random.Range(1, 100);
            //Instantiate(trapsForGeneration, new Vector2(position.x + Random.Range(0, 1), position.y + 0.8f), Quaternion.identity);// Клонирования обьекта(ловушка) и его координаты)

            yield return new WaitForEndOfFrame(); //ожидания установки блоков
        }

        newBlock = new GameObject("End block");// создаем новый обьект
        renderer = newBlock.AddComponent<SpriteRenderer>();//добавляем компонент SpriteRenderer
        BoxCollider2D boxCollider = newBlock.AddComponent<BoxCollider2D>();//добавляем компонент BoxCollider2D
        boxCollider.size = new Vector2(1.274357f, 0.1442559f);//задаем размер BoxCollider2D 
        renderer.sprite = this.endBlock;//используем спрайт который мы засунули в endBlock
        Instantiate(chestForGeneration, new Vector2(position.x + Random.Range(0, 1), position.y + 0.8f), Quaternion.identity);

        position.x += size.x; //позиция по X, чтобы всегда была чуть дальше чем прошлый
        position.y += size.y * Random.Range(-1, 1); //позиция по Y, рандомная
        newBlock.transform.position = position; // присваиваем позицию новомоу обьекту
        newBlock.transform.localScale = size; //задаем размер обьекта

        yield return new WaitForEndOfFrame(); //ожидания установки блоков
    }
    public void PlusCoin(int count) //сколько будем плюсовать монеток
    {
        coin += count;
    }
    public void coin_Counter() //Метод который просто вызывает значение переменной Coin, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(coin);
    }

    private void Awake()
    {
        Instance = this;
    }
  
    public void Restart()
    {
        Level = 1;
        SaveSerial.Instance.ResetData();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
    }
    public void CompleteLevel() // Добавим метод CompleteLevel, который будет увеличивать переменную completeLevels на одну единицу каждый раз, когда игрок пройдет очередной уровень.
    {
        this.Level += 1;
        //Debug.Log("Level: " + Level);
        GameObject.Find("EnemySkelet").GetComponent<Entity>().BoostHP();
        GameObject.Find("EnemySkelet").GetComponent<Entity>().BoostAttackDamage();
        GameObject.Find("EnemySkelet").GetComponent<Enemy_Skelet>().BoostSpeed();
        SaveSerial.Instance.SaveGame();
        SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
        //StartCoroutine(OnGeneratingRoutine());
    }
    private void Update()
    {
        if (Hero.Instance.playerDead == true)
        {
            Restart();
        }
        if (Chest.Instance.chestOpen == true)
        {
            CompleteLevel();
        }
    }
    
}
