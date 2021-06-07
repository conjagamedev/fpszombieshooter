using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] zombies; 
    private Vector3 spawnPos; 
    private Quaternion playerRot; 
    public GameManager gameManager;        
    private GameObject zombieGO;
    private string currentEnemy; 

    public Text rndTxt; 

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        
    }

    private void Update()
    {
        if(gameManager.roundBegin == true)
        {
            InvokeRepeating("SpawnZombie", 2.0f, gameManager.spawnTimer);
            rndTxt.text = "" + gameManager.currentRound;
            gameManager.roundBegin = false; 
        }
        if(gameManager.roundEnd == true)
        {
            CancelInvoke(); 
            gameManager.roundEnd = false; 
            gameManager.zombiesKilledInTotal = gameManager.zombiesKilledInTotal + gameManager.zombiesKilledInRound; 
            gameManager.zombiesKilledInRound = 0;
            gameManager.zombiesSpawnedInRound = 0; 
            StartCoroutine(BeginRound()); 
        }

        if(gameManager.maxZombiesInRound == gameManager.zombiesSpawnedInRound)
        {
            CancelInvoke();
        }

        if(gameManager.zombiesKilledInRound >= gameManager.maxZombiesInRound && gameManager.roundEnd == false 
            && gameManager.currentZombiesAlive == 0 && gameManager.roundBegin != true)
        {
            gameManager.roundEnd = true; 
            gameManager.currentRound++; 
            gameManager.maxZombiesInRound = gameManager.maxZombiesInRound + Random.Range(4,7);
            if(gameManager.spawnTimer > 0.7f)
                gameManager.spawnTimer = gameManager.spawnTimer - 0.1f; 

        }
        
    }

    IEnumerator BeginRound()
    {
        yield return new WaitForSeconds(3f);
        gameManager.roundBegin = true; 
    }
    

    private void SpawnZombie()
    {
        int rand = Random.Range(0,4); 
        if(rand == 0)  
            spawnPos = spawnPoints[0].transform.position; 
        if(rand == 1)
            spawnPos = spawnPoints[1].transform.position;
        if(rand == 2)
            spawnPos = spawnPoints[2].transform.position;
        if(rand == 3)
            spawnPos = spawnPoints[3].transform.position;

        playerRot = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform.rotation;
        
        int randSpawn = Random.Range(0,5); 
        
        if(gameManager.currentRound < 5)
        {
            zombieGO = Instantiate(zombies[0], spawnPos, playerRot);
            currentEnemy = "basic";    
        }
        if(gameManager.currentRound >= 5)
        {
            if(randSpawn < 4)
            {
                zombieGO = Instantiate(zombies[0], spawnPos, playerRot);
                currentEnemy = "basic";
            }
            else
            {
                zombieGO = Instantiate(zombies[1], spawnPos, playerRot);
                currentEnemy = "new";
            }  
        }

        gameManager.currentZombiesAlive++;
        gameManager.zombiesSpawnedInRound++;
        
        int randLvl = Random.Range(0,2);
        
        if(randLvl == 0 && currentEnemy == "basic")
            zombieGO.GetComponent<ZombieLocomotion>().level = 1;
        if(randLvl == 1  && currentEnemy == "basic")
            zombieGO.GetComponent<ZombieLocomotion>().level = 2; 
        
        
        if(currentEnemy == "new")
            zombieGO.GetComponentInChildren<ZombieLocomotion>().level = 2; 
    }
    
}
