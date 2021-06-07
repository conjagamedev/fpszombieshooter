using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int currentRound; 
    private int maxRound = 999; 
    public bool roundBegin;
    public bool roundEnd; 
    public int zombiesKilledInRound; 
    public int zombiesKilledInTotal; 
    public int maxZombiesInRound; 
    public int currentZombiesAlive; 
    public int zombiesSpawnedInRound;
    public float spawnTimer = 1.5f;  

    public int cash; 
    public Text cashTxt; 

    public int headshotCount; 
    
    private void Update()
    {
        cashTxt.text = "$" + cash; 
    }

    public void UpCash(int cashGain)
    {
        cash = cash + cashGain; 
    }

    public void DownCash(int cashLoss)
    {
        cash = cash - cashLoss;
    }

    public void UpHeadShot(int headshotGain)
    {
        headshotCount = headshotCount + headshotGain; 
    }
    
}
