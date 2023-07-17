using System.Collections;
using UnityEngine;

//For the ability to work with scenes
using UnityEngine.SceneManagement; 


public class LvLGeneration : MonoBehaviour
{
    // These three variables are used to record each block sprite individually: start, intermediate, intermediate2 and end.
    public GameObject[] startBlock; //start picture
    public GameObject[] startBorder; //start picture frame
    public GameObject[] midBlock; //midle pictures where enemies are generated
    public GameObject[] borderMid; //middle picture frame where enemies are generated
    public GameObject[] endBlock; // painting with a book
    public GameObject[] endBorder; // picture frame with chest
    public GameObject[] enemyForGeneration;
    public GameObject[] bossForGeneration;
    public GameObject[] chestForGeneration;
    public GameObject[] powerUpForGeneration;
    public static LvLGeneration Instance { get; set; } // To collect and send data from this script

    public int Level = 1; // Let's add a numeric variable completeLevels, which will be used to specify the number of levels completed.
    public int coin; // number of points
    public bool key = false;
    private int enemyCheat; // Cheat to generate enemies
    private string activeSceneName;
    private bool playerDead = false;
    private bool chestOpen = false;

    private void Start() // In the Start method we will run the level generation at the start of the game.
    {
        SaveSerial.Instance.LoadGame();
        coin = SaveSerial.Instance.playerCoin;
        Level = SaveSerial.Instance.passedLvl;
        enemyCheat = SaveSerial.Instance.enemyCheat;
        activeSceneName = SceneManager.GetActiveScene().name;

        if (Level == 0)
        {
            Level = 1;
        }
        if (activeSceneName == "startLevel")
        {
            StartCoroutine(OnGeneratingRoutine()); // level generation process
        }
        
    }
    private void Update()
    {
        playerDead = Hero.Instance.playerDead;
        if (playerDead == true)
        {
            DeadScreen();
        }
        chestOpen = SpellBook.Instance.chestOpen;
        if (chestOpen == true)
        {
            CompleteLevel();
        }
    }
    private IEnumerator OnGeneratingRoutine() // In the OnGeneratingRoutine method, we will perform the level generation process itself. Since the levels can be both large and small and can be generated for different amounts of time, we will place the generation process in a coroutine, so that the game does not "freeze" while the "generator" is running
    {
        Vector3 position = new Vector3(0, 2, 110);
        position.x = 0; // position by X so that it is always slightly further than the last one
        position.y = 2; //Y position
        position.z = 110;

        GameObject newstartBlock = Instantiate(startBlock[Random.Range(0, startBlock.Length)]); //Graduation of the feather picture
        newstartBlock.name = "Start block";// create a new object
        newstartBlock.transform.position = new Vector3(position.x, position.y, position.z);//assign a position to a new object
        newstartBlock.layer = LayerMask.NameToLayer("Ground"); //Adding the Earth layer to the created block
        GameObject newBorderStart = Instantiate(startBorder[Random.Range(0, startBorder.Length)], new Vector3(position.x, position.y, position.z - 5), Quaternion.identity);


        int count = this.Level; // the numeric variable count will indicate how many intermediate blocks need to be built, this number will depend on the number of levels completed and, so that they are not too few at first, an additional five(5) blocks.

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 0)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[Random.Range(0, enemyForGeneration.Length)], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 1)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[0], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 2)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[1], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 3)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[2], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 4)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[3], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 5)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[4], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 6)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[5], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }
        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 7)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(enemyForGeneration[6], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }
        if (Level != 10 && Level != 20 && Level != 30 && Level != 40 && enemyCheat == 8)
        {
            for (int i = 0; i < count; i++)
            {
                position.x += 9.8f; // position by X so that it is always slightly further than the last one
                position.y = 2; // Y position, random
                position.z = 110;
                GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
                newMidBlock.name = "Middle block" + Random.Range(1, 999);
                newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
                GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

                GameObject enemy = Instantiate(bossForGeneration[Random.Range(0, bossForGeneration.Length)], new Vector3(position.x, -1, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
                enemy.name = "Enemy" + Random.Range(1, 999);
                enemy.gameObject.SetActive(true);

                yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
            }
        }

        if (Level == 10 || Level == 20 || Level == 30 || Level == 40) //Боссы
        {
            position.x += 9.8f; // position by X so that it is always slightly further than the last one
            position.y = 2; // Y position, random
            position.z = 110;
            GameObject newMidBlock = Instantiate(midBlock[Random.Range(0, midBlock.Length)], new Vector3(position.x, position.y, 110), Quaternion.identity); // create a new object
            newMidBlock.name = "Middle block" + Random.Range(1, 999);
            newMidBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created block
            GameObject newBorderMid = Instantiate(borderMid[Random.Range(0, borderMid.Length)], new Vector3(position.x, position.y, 105), Quaternion.identity);

            GameObject enemy = Instantiate(bossForGeneration[Random.Range(0, bossForGeneration.Length)], new Vector3(position.x, position.y + 0.5f, position.z - 1), Quaternion.identity); //Clone an object (enemy) and its coordinates)
            enemy.name = "Enemy" + Random.Range(1, 999);
            enemy.gameObject.SetActive(true);

            yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
        }

            GameObject newEndBlock = Instantiate(endBlock[Random.Range(0, endBlock.Length)], new Vector3(position.x + 9.8f, position.y, 110), Quaternion.identity);
        newEndBlock.layer = LayerMask.NameToLayer("Ground");//Adding the Earth layer to the created blockу
        newEndBlock.name = "End block";// create a new object
        GameObject newBorderEnd = Instantiate(endBorder[Random.Range(0, endBorder.Length)], new Vector3(position.x + 9.8f, position.y, 105), Quaternion.identity);

        GameObject newChest = Instantiate(chestForGeneration[Random.Range(0, chestForGeneration.Length)], new Vector3(position.x + 9.8f, -1, position.z - 2), Quaternion.identity);

        yield return new WaitForEndOfFrame(); // waiting for blocks to be installed
    }
    public void PlusCoin(int count) // how many coins will be added
    {
        coin += count;
    }
    public void coin_Counter() //Method which simply calls the Coin variable, I needed to pass this number to the life counter script
    {
        Debug.Log(coin);
    }
    public void FindKey() // how many keys to add
    {
        key = true;
    }
    public void UseKey() 
    {
        key = false;        
    }
    public void key_Counter() //Method which simply calls the Coin variable, I needed to pass this number to the life counter script
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
        if (activeSceneName == "startLevel")
        {
            SceneManager.LoadScene("startLevel", LoadSceneMode.Single);
        }
    }
    public void DeadScreen()
    {
        Level = 1;
        SaveSerial.Instance.ResetData();
        SceneManager.LoadScene("DeadScreen", LoadSceneMode.Single);
    }
    public void CompleteLevel() // Let's add a CompleteLevel method that will increase the completeLevels variable by one unit each time the player completes another level.
    {
        this.Level += 1;
        SaveSerial.Instance.SaveGame();
        SaveSerial.Instance.SaveLastGame();
        SceneManager.LoadScene("LevelComplete", LoadSceneMode.Single);
    }
}
