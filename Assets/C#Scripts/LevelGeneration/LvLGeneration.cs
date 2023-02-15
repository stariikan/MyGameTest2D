using System.Collections;
using UnityEngine;

//Для возможности работать со сценами
using UnityEngine.SceneManagement; 


public class LvLGeneration : MonoBehaviour
{
    // В эти три переменные занесем каждый спрайт блока по отдельности: стартовый, промежуточный, промежуточный2 и конечный.
    public GameObject[] startBlock; //стартовая картина
    public GameObject[] startBorder; //стартовая рамка картини
    public GameObject[] midBlock; //мидл картины где генерятся враги
    public GameObject[] borderMid; //мидл рамка картины где генерятся враги
    public GameObject[] midBlock2; //пустое место между картинами
    public GameObject[] endBlock; //картина с сундуком
    public GameObject[] endBorder; // рамка картины с сундуком
    public GameObject[] enemyForGeneration;
    public GameObject[]  trapsForGeneration;
    public GameObject[] chestForGeneration;
    public GameObject[] powerUpForGeneration;
    public static LvLGeneration Instance { get; set; } //Для сбора и отправки данных из этого скрипта

    public int Level = 1; // Добавим одну числовую переменную completeLevels, с помощью которой будем указывать количество пройденных уровней.
    public int coin; // кол-во очков
    public bool key = false;

    private void Start() // В методе Start мы будем запускать генерацию уровня во время старта игры.
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        if (Level == 0)
        {
            Level = 1;
        }
        StartCoroutine(OnGeneratingRoutine()); //процесс генерации уровня
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
    private IEnumerator OnGeneratingRoutine() //В методе OnGeneratingRoutine, будем выполнять сам процесс генерации уровня. Так как уровни у нас могут быть как большими, так и маленькими и генерироваться разное количество времени, процесс генерации мы поместим в корутину, чтобы игра не “зависала” во время работы “генератора”
    {
        //Vector2 size = new Vector3(4, 4); //Для начала в методе OnGeneratingRoutine объявим две векторные переменные: size, где укажем размер блоков по длине и высоте и position, где укажем точку, откуда будет начинать строится уровень. Теперь можно построить стартовый блок.
        Vector3 position = new Vector3(0, 2, 110);
        position.x = 0; //позиция по X, чтобы всегда была чуть дальше чем прошлый
        position.y = 2; //позиция по Y
        position.z = 110;

        GameObject newstartBlock = Instantiate(startBlock[Random.Range(0, startBlock.Length)]); //герация перовой картины
        newstartBlock.name = "Start block";// создаем новый обьект
        newstartBlock.transform.position = new Vector3(position.x, position.y, position.z);// присваиваем позицию новомоу обьекту
        newstartBlock.layer = LayerMask.NameToLayer("Ground"); //Добавление слоя Земля к созданному блоку
        GameObject newFogStart = Instantiate(startBorder[Random.Range(0, startBorder.Length)], new Vector3(position.x, position.y, position.z - 5), Quaternion.identity);

       
        
        //GameObject newFoggy = Instantiate(midBlock2[Random.Range(0, midBlock2.Length)], new Vector3(position.x + 6.05f, position.y, 108), Quaternion.identity);
        //newFoggy.name = "Start fog";// создаем новый обьект
        //newFoggy.layer = LayerMask.NameToLayer("Ground"); //Добавление слоя Земля к созданному блоку


        int count = this.Level; // Числовая переменная count будет указывать какое кол - во промежуточных блоков необходимо построить, это число будет зависеть от количества пройденных уровней и, чтобы их изначально не было слишком мало на первых уровнях, еще пяти(5) дополнительных блоков.

        for (int i = 0; i < count; i++)
        {


            position.x += 9.8f; //позиция по X, чтобы всегда была чуть дальше чем прошлый
            position.y = 2; //позиция по Y, рандомная
            position.z = 110;
            GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // создаем новый обьект
            newMidBlock.name = "Middle block" + Random.Range(1, 999);
            newMidBlock.layer = LayerMask.NameToLayer("Ground");//Добавление слоя Земля к созданному блоку
            GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

           // GameObject newMidBlock2 = Instantiate(midBlock2[Random.Range(0, midBlock2.Length)], new Vector3(position.x + 6.128f, position.y, position.z - 2), Quaternion.identity);// создаем новый обьект
           // newMidBlock2.name = "Mid block" + Random.Range(1, 999);
           // newMidBlock2.layer = LayerMask.NameToLayer("Ground"); //Добавление слоя Земля к созданному блоку

            GameObject enemy = Instantiate(enemyForGeneration[Random.Range(0, enemyForGeneration.Length)], new Vector3(position.x, position.y, position.z - 1), Quaternion.identity); //Клонирования обьекта (враг) и его координаты)
            enemy.name = "Enemy" + Random.Range(1, 999);

            yield return new WaitForEndOfFrame(); //ожидания установки блоков
        }

        GameObject newEndBlock = Instantiate(endBlock[Random.Range(0, endBlock.Length)], new Vector3(position.x + 9.8f, position.y, 110), Quaternion.identity);
        newEndBlock.layer = LayerMask.NameToLayer("Ground");//Добавление слоя Земля к созданному блоку
        newEndBlock.name = "End block";// создаем новый обьект
        GameObject newFogEnd = Instantiate(endBorder[Random.Range(0, endBorder.Length)], new Vector3(position.x + 9.8f, position.y, 105), Quaternion.identity);


        GameObject newChest = Instantiate(chestForGeneration[Random.Range(0, chestForGeneration.Length)], new Vector3(position.x + 9.8f, position.y, position.z - 2), Quaternion.identity);
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
    public void FindKey() //сколько будем плюсовать ключей
    {
        key = true;
    }
    public void UseKey() 
    {
        key = false;        
    }
    public void key_Counter() //Метод который просто вызывает значение переменной Coin, нужен мне был для передачи этого числа в скрипт с каунтером жизней
    {
        Debug.Log(key);
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
        this.Level += 1;;
        GameObject.Find("Mushroom").GetComponent<Entity_Mushroom>().BoostHP();
        GameObject.Find("Mushroom").GetComponent<Entity_Mushroom>().BoostAttackDamage();
        GameObject.Find("Mushroom").GetComponent<Enemy_Mushroom>().BoostSpeed();
        SaveSerial.Instance.SaveGame();
        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Single);
    }
}
