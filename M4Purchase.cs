using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M4Purchase: MonoBehaviour
{
    private GameManager gameManager; 
    private Collider playerCol; 
    public GameObject m4Rifle; 

    private bool inTrigger;
    private bool purchased;  

    private Animation UIanimation; 
    public Text uiTxt; 
    private WeaponSwitching weaponSwitching; 
    private Animation animation; 
    private AudioSource audio; 

    private int m4AmmoCost = 3000;
    private int m4WeaponCost = 6000;
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

        if(weaponSwitching.activeGun != "M4_Carbine")
        {
            if(weaponSwitching.otherGun != "M4_Carbine")
            {
                ammoPriceMTxt.gameObject.active = false; 
            }

        }

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
        if(inTrigger && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();
            
            if(gun.gameObject.name == "M4_Carbine")
            {
                if(gameManager.cash >= m4AmmoCost && gun.overallAmmo < gun.totalMaxAmmo )
                {
                    uiTxt.text = "[MB2] PURCHASE M4 RIFLE AMMO";
                }

                if(gameManager.cash < m4AmmoCost && gun.overallAmmo < gun.totalMaxAmmo)
                {
                    uiTxt.text = "REQUIRE MORE CASH";
                }
                
                if(gun.overallAmmo >= gun.totalMaxAmmo)
                {
                    uiTxt.text = "M4 RIFLE AMMO FULL";
                }                
            }
            
            if(gun.gameObject.name != "M4_Carbine")
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

                if(gunName == "M4_Carbine")
                {
                    if(gameManager.cash >= m4AmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "[MB2] PURCHASE M4 RIFLE AMMO";
                    }

                    if(gameManager.cash < m4AmmoCost && secondGun.overallAmmo < secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "REQUIRE MORE CASH";
                    }
                    
                    if(secondGun.overallAmmo >= secondGun.totalMaxAmmo)
                    {
                        uiTxt.text = "M4 RIFLE AMMO FULL";
                    }
                }
            }
        }
        if(Input.GetButton("Fire2") && inTrigger && !purchased && hasGun)
        {
            Gun gun = GameObject.Find("Player").GetComponentInChildren<Gun>();

            if(gameManager.cash >= m4AmmoCost && gun.overallAmmo < gun.totalMaxAmmo && gun.gameObject.name == "M4_Carbine")
            {
                purchased = true; 
            
                
                if(!audio.isPlaying)
                    audio.Play();
                    
                gun.audio.PlayOneShot(gun.reloadAudioClip);
                gameManager.DownCash(m4AmmoCost);
                gun.overallAmmo = gun.totalMaxAmmo;
                animation.Play();
                purchased = false;            
            }
            else if(gun.gameObject.name != "M4_Carbine")
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

        if(!hasGun && inTrigger && gameManager.cash >= m4WeaponCost)
        {
            uiTxt.text = "[MB2] PURCHASE M4 RIFLE";
        }

        // ---------- PURCHASE THE GUN ---------- //
        if(!hasGun && inTrigger && Input.GetButton("Fire2") && !purchased) 
        {
            if(gameManager.cash >= m4WeaponCost)
            {
                purchased = true; 
                hasGun = true; 
                WeaponSwitching weaponSwitching = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();

                GameObject rifleGO = Instantiate(m4Rifle, weaponSwitching.gameObject.transform.position, transform.rotation);
                rifleGO.name = "M4_Carbine";

                weaponSwitching.otherGun = weaponSwitching.activeGun; 
                weaponSwitching.activeGun = rifleGO.name;
                weaponSwitching.secondGunScript = rifleGO.GetComponent<Gun>(); 
                                                
                if(GameObject.Find("WeaponHolder").transform.childCount >= 2)
                {
                    //weaponSwitching.secondGunScript = rifleGO.GetComponent<Gun>();
                    Destroy(GameObject.Find(weaponSwitching.otherGun));
                    //weaponSwitching.secondGunScript = rifleGO.GetComponent<Gun>();
                    //weaponSwitching.activeGun = weaponSwitching.otherGun;  
                    //weaponSwitching.activeGun = rifleGO.name; 
                }
                    
                if(weaponSwitching.selectedWeapon == 0)
                {
                    weaponSwitching.selectedWeapon = 1;
                    weaponSwitching.SelectWeapon();
                    Animation anim = rifleGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }
                else if(weaponSwitching.selectedWeapon == 1)
                {
                    weaponSwitching.selectedWeapon = 0; 
                    weaponSwitching.SelectWeapon();
                    Animation anim = rifleGO.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }

                rifleGO.transform.parent = GameObject.Find("WeaponHolder").transform;
                gameManager.DownCash(m4WeaponCost);
                weaponPriceMTxt.gameObject.active = false; 
                ammoPriceMTxt.gameObject.active = true; 

                if(!audio.isPlaying)
                    audio.Play();

                purchased = false; 
            }
        }



        if(!hasGun && inTrigger && gameManager.cash < m4WeaponCost)
        {
            uiTxt.text = "REQUIRE MORE CASH";
        }
    }

    
}
