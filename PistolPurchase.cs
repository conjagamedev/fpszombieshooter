using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolPurchase : MonoBehaviour
{
    private GameManager gameManager; 
    private Collider playerCol;
    public GameObject pistol;  

    private bool inTrigger;
    private bool purchased;  

    private Animation UIanimation; 
    public Text uiTxt; 
    private WeaponSwitching weaponSwitching; 
    private Animation animation; 
    private AudioSource audio; 

    private bool hasGun; 
    private int pistolAmmoCost = 1200;
    private int pistolWeaponCost = 2400;
    public TMPro.TextMeshPro ammoPriceMTxt; 
    public TMPro.TextMeshPro weaponPriceMTxt;

    private void Start() 
    {
        animation = GetComponent<Animation>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<Collider>();
        weaponSwitching = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        UIanimation = GameObject.Find("uiTxt").GetComponent<Animation>(); 
        uiTxt = GameObject.Find("uiTxt").GetComponent<Text>();
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col == playerCol)
        {
            inTrigger = true; 
            UIanimation.Play("gameTxtEnter");

        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col == playerCol)
        {
            inTrigger = false; 
            UIanimation.Play("gameTxtExit");

        }
    }

    private void Update() 
    {
        if(weaponSwitching.activeGun != "Pistol" || weaponSwitching.otherGun != "Pistol")
        {
            ammoPriceMTxt.gameObject.active = false; 
            weaponPriceMTxt.gameObject.active = true; 
        }

        if(weaponSwitching.activeGun == "Pistol" || weaponSwitching.otherGun == "Pistol")
        {
            ammoPriceMTxt.gameObject.active = true; 
            weaponPriceMTxt.gameObject.active = false; 
        }

        if(GameObject.Find("Pistol") != null)
        {
            hasGun = true; 
        }
        if(GameObject.Find("Pistol") == null)
        {
            hasGun = false; 
        }


        if(inTrigger && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();
            
            if(gun.gameObject.name == "Pistol")
            {
                if(gameManager.cash >= pistolAmmoCost && gun.overallAmmo < gun.totalMaxAmmo && gun.gameObject.name == "Pistol")
                {
                    uiTxt.text = "[MB2] PURCHASE PISTOL AMMO";
                }

                if(gameManager.cash < pistolAmmoCost && gun.overallAmmo < gun.totalMaxAmmo && gun.gameObject.name == "Pistol")
                {
                    uiTxt.text = "REQUIRE MORE CASH";
                }
                
                if(gun.overallAmmo >= gun.totalMaxAmmo && gun.gameObject.name == "Pistol")
                {
                    uiTxt.text = "PISTOL AMMO FULL";
                }                
            }
            
            if(gun.gameObject.name != "Pistol")
            {
                string gunName ;
                Gun secondGun;

                if(weaponSwitching.secondGunScript != null)
                {
                    gunName = weaponSwitching.secondGunScript.gameObject.name; 
                    secondGun = weaponSwitching.secondGunScript.gameObject.GetComponent<Gun>();
                }
                else
                {
                    gunName = ""; 
                    secondGun = null; 
                }
                
                if(gunName == "Pistol")
                {
                    if(gameManager.cash >= pistolAmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "[MB2] PURCHASE PISTOL AMMO";
                    }

                    if(gameManager.cash < pistolAmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "REQUIRE MORE CASH";
                    }
                    
                    if(secondGun.overallAmmo >= secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "PISTOL AMMO FULL";
                    }
                }
            }


        }
        if(Input.GetButton("Fire2") && inTrigger && !purchased && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();

            if(gameManager.cash >= pistolAmmoCost && gun.overallAmmo < gun.totalMaxAmmo && gun.gameObject.name == "Pistol")
            {
                purchased = true; 

                
                if(!audio.isPlaying)
                    audio.Play();
                    
                gun.audio.PlayOneShot(gun.reloadAudioClip);
                gameManager.DownCash(pistolAmmoCost);
                gun.overallAmmo = gun.totalMaxAmmo;
                animation.Play();
                purchased = false; 
                print("bought full max ammo for pistol");           
            }
            else if(gun.gameObject.name != "Pistol")
            {
                if(weaponSwitching.selectedWeapon == 0)
                {
                    weaponSwitching.selectedWeapon = 1; 
                    weaponSwitching.SelectWeapon();
                }
                if(weaponSwitching.selectedWeapon == 1)
                {
                    weaponSwitching.selectedWeapon = 0;
                    weaponSwitching.SelectWeapon();
                }
            }


        }
        
        if(!hasGun && inTrigger && gameManager.cash >= pistolWeaponCost)
        {
            uiTxt.text = "[MB2] PURCHASE PISTOL";
        }

        // ---------- PURCHASE THE GUN ---------- //
        if(!hasGun && inTrigger && Input.GetButton("Fire2") && !purchased) 
        {
            if(gameManager.cash >= pistolWeaponCost)
            {
                purchased = true;  
                
                WeaponSwitching weaponSwitching = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();
                
                if(GameObject.Find("WeaponHolder").transform.childCount == 2)
                    Destroy(GameObject.Find(weaponSwitching.activeGun));

                GameObject pistolGO = Instantiate(pistol, weaponSwitching.gameObject.transform.position, transform.rotation);
                pistolGO.name = "Pistol";
                hasGun = true;
                weaponSwitching.otherGun = weaponSwitching.activeGun; 
                weaponSwitching.activeGun = pistolGO.name; 
                if(weaponSwitching.selectedWeapon == 0)
                {
                    weaponSwitching.selectedWeapon = 1;
                    weaponSwitching.SelectWeapon();
                    Animation anim = pistolGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }
                else if(weaponSwitching.selectedWeapon == 1)
                {
                    weaponSwitching.selectedWeapon = 0; 
                    weaponSwitching.SelectWeapon();
                    Animation anim = pistolGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }

                pistolGO.transform.parent = GameObject.Find("WeaponHolder").transform;
                gameManager.DownCash(pistolWeaponCost);
                weaponPriceMTxt.gameObject.active = false; 
                ammoPriceMTxt.gameObject.active = true; 

                if(!audio.isPlaying)
                    audio.Play();
                

                purchased = false; 
            }
        }



        if(!hasGun && inTrigger && gameManager.cash < pistolWeaponCost)
        {
            uiTxt.text = "REQUIRE MORE CASH";
        }
    }

    
}
