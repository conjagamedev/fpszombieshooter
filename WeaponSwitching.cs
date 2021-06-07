using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0; 

    private AudioSource audio;

    public string activeGun;
    public string otherGun; 
    public Gun secondGunScript;  

    void Start() 
    {
        SelectWeapon();
        audio = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon; 

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else    
                selectedWeapon++; 
        }
        
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--; 
        }

        if(Input.GetButtonDown("Cycle Weapon") && selectedWeapon == 0)
        {
            print("pressed triangle");
            selectedWeapon = 1; 
        }
        else if(Input.GetButtonDown("Cycle Weapon") && selectedWeapon == 1)
        {
            selectedWeapon = 0;
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
            audio.Play();
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            {
                if(i == selectedWeapon)
                {
                    weapon.gameObject.SetActive(true);
                    otherGun = activeGun;  
                    activeGun = weapon.gameObject.name; 
                    Animation anim = weapon.gameObject.GetComponent<Animation>();
                    anim.Play("WeaponPull");
                }
                else    
                {
                    secondGunScript = weapon.gameObject.GetComponent<Gun>();
                    weapon.gameObject.SetActive(false);
                }
                i++;
            }
        }
    }
    
}
