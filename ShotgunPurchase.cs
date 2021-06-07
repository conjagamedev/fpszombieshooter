using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunPurchase : MonoBehaviour
{
    private GameManager gameManager; 
    private Collider playerCol; 
    public GameObject shotgun; 

    private bool inTrigger;
    private bool purchased;  

    private Animation UIanimation; 
    public Text uiTxt; 
    private WeaponSwitching weaponSwitching; 
    private Animation animation; 
    private AudioSource audio; 

    private int shotgunAmmoCost = 1600;
    private int shotgunWeaponCost = 3200;
    public TMPro.TextMeshPro ammoPriceMTxt; 
    public TMPro.TextMeshPro weaponPriceMTxt; 
    private bool hasGun; 

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
        if(weaponSwitching.activeGun != "Shotgun")
        {
            if(weaponSwitching.otherGun != "Shotgun")
            {
                weaponPriceMTxt.gameObject.active = true; 
                ammoPriceMTxt.gameObject.active = false; 
            }
        }
        else if(weaponSwitching.activeGun == "Shotgun" || weaponSwitching.otherGun == "Shotgun")
        {
            ammoPriceMTxt.gameObject.active = true; 
            weaponPriceMTxt.gameObject.active = false; 
        }
        if(inTrigger && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();
            
            if(gun.gameObject.name == "Shotgun")
            {
                if(gameManager.cash >= shotgunAmmoCost && gun.overallAmmo < gun.totalMaxAmmo )
                {
                    uiTxt.text = "[MB2] PURCHASE 870 SHOTGUN AMMO";
                }

                if(gameManager.cash < shotgunAmmoCost && gun.overallAmmo < gun.totalMaxAmmo)
                {
                    uiTxt.text = "REQUIRE MORE CASH";
                }
                
                if(gun.overallAmmo >= gun.totalMaxAmmo)
                {
                    uiTxt.text = "870 SHOTGUN AMMO FULL";
                }                
            }
            
            if(gun.gameObject.name != "Shotgun")
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
                
                if(gunName == "Shotgun")
                {
                    if(gameManager.cash >= shotgunAmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "[MB2] PURCHASE 870 SHOTGUN RIFLE AMMO";
                    }

                    if(gameManager.cash < shotgunAmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "REQUIRE MORE CASH";
                    }
                    
                    if(secondGun.overallAmmo >= secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "870 SHOTGUN AMMO FULL";
                    }
                }
            }
        }
        if(Input.GetButton("Fire2") && inTrigger && !purchased && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();

            if(gameManager.cash >= shotgunAmmoCost && gun.overallAmmo < gun.totalMaxAmmo && gun.gameObject.name == "Shotgun")
            {
                purchased = true; 
            
                
                if(!audio.isPlaying)
                    audio.Play();
                    
                gun.audio.PlayOneShot(gun.reloadAudioClip);
                gameManager.DownCash(shotgunAmmoCost);
                gun.overallAmmo = gun.totalMaxAmmo;
                animation.Play();
                purchased = false;            
            }
            else if(gun.gameObject.name != "Shotgun")
            {
                if(weaponSwitching.selectedWeapon == 0)
                {
                    weaponSwitching.selectedWeapon = 1;
                    weaponSwitching.SelectWeapon();
                }
                else
                {
                    weaponSwitching.selectedWeapon = 0; 
                    weaponSwitching.SelectWeapon();
                }
            }


        }

        if(!hasGun && inTrigger && gameManager.cash >= shotgunWeaponCost)
        {
            uiTxt.text = "[MB2] PURCHASE 870 SHOTGUN";
        }

        // ---------- PURCHASE THE GUN ---------- //
        if(!hasGun && inTrigger && Input.GetButton("Fire2") && !purchased) 
        {
            if(gameManager.cash >= shotgunWeaponCost)
            {
                purchased = true; 
                hasGun = true; 
                
                WeaponSwitching weaponSwitching = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();
                
                if(GameObject.Find("WeaponHolder").transform.childCount == 2)
                    Destroy(GameObject.Find(weaponSwitching.activeGun));  

                GameObject shotgunGO = Instantiate(shotgun, weaponSwitching.gameObject.transform.position, transform.rotation);
                shotgunGO.name = "Shotgun";
                weaponSwitching.otherGun = weaponSwitching.activeGun; 
                weaponSwitching.activeGun = shotgunGO.name; 
                if(weaponSwitching.selectedWeapon == 0)
                {
                    weaponSwitching.selectedWeapon = 1;
                    weaponSwitching.SelectWeapon();
                    Animation anim = shotgunGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }
                else if(weaponSwitching.selectedWeapon == 1)
                {
                    weaponSwitching.selectedWeapon = 0; 
                    weaponSwitching.SelectWeapon();
                    Animation anim = shotgunGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }

                shotgunGO.transform.parent = GameObject.Find("WeaponHolder").transform;
                gameManager.DownCash(shotgunWeaponCost);
                weaponPriceMTxt.gameObject.active = false; 
                ammoPriceMTxt.gameObject.active = true; 

                if(!audio.isPlaying)
                    audio.Play();
                

                purchased = false; 
            }
        }



        if(!hasGun && inTrigger && gameManager.cash < shotgunWeaponCost)
        {
            uiTxt.text = "REQUIRE MORE CASH";
        }
    }
}
