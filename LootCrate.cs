using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    private Animation animation; 
    private Collider playerCol; 

    bool inTrigger;
    bool boxOpened; 
    bool inAnim; 

    void Start()
    {
        animation = GetComponent<Animation>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<Collider>();
    } 

    void OnTriggerEnter(Collider col)
    {
        if(col == playerCol)
        {
            inTrigger = true; 
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col == playerCol)
        {
            inTrigger = false; 
            animation.Play("crateClose");
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire2") && inTrigger && !boxOpened && !inAnim)
        {
            inAnim = true; 
            animation.Play("crateOpen");
        }

        if(Input.GetButtonDown("Fire2") && inTrigger && boxOpened && !inAnim)
        {
            inAnim = true; 
            animation.Play("crateClose");
        }
    }

    public void BoxOpened() 
    {
        boxOpened = true; 
        inAnim = false; 
    }

    public void BoxClosed()
    {
        boxOpened = false; 
        inAnim = false; 
    }
}
