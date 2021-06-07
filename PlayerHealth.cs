using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;  
    public Image damageOverlayImage; 
    private float r,g,b,a; 
    private float healStrength = 3f;
    private bool isHealing; 
    private Slider healthSlider; 

    private void Start() 
    {
        r = damageOverlayImage.color.r; 
        g = damageOverlayImage.color.g; 
        b = damageOverlayImage.color.b; 
        a = damageOverlayImage.color.a; 
        healthSlider = GameObject.Find("healthSlider").GetComponent<Slider>();
    }

    public void PlayerTakeDamage(float amount)
    {
        health -= amount;
        DamageSplash();

        if(health <= 0f)
        {
            Die();
            health = 0f; 
        }         
    }
    
    void Die()
    {
        //something here
        print("dead");
    }

    void DamageSplash()
    {
        if(health <= maxHealth-1 && health > maxHealth-(maxHealth/4))
            damageOverlayImage.color = new Color(r,g,b,0.1f);
        
        if(health <= maxHealth-(maxHealth/4) && health > maxHealth-(maxHealth/2))
            damageOverlayImage.color = new Color(r,g,b,0.3f);
        
        if(health <= maxHealth-(maxHealth/2) && health > maxHealth-((maxHealth/4) * 3))
            damageOverlayImage.color = new Color(r,g,b,0.5f);
        
        if(health <= maxHealth-((maxHealth/4)) * 3 && health > maxHealth-((maxHealth/4) * 3.5f))
            damageOverlayImage.color = new Color(r,g,b,1f);
        
        if(health >= maxHealth)
            damageOverlayImage.color = new Color(r,g,b,0f);
    }

    void Update()
    {
        if(health < maxHealth && !isHealing)
        {
            InvokeRepeating("AutoHeal", 1f, 1f);
            isHealing = true;
        }

        if(health >= maxHealth)
        {
            CancelInvoke("AutoHeal");
            isHealing = false; 
        }

        healthSlider.maxValue = maxHealth; 
        healthSlider.minValue = 0;
        healthSlider.value = health; 
    }

    void AutoHeal()
    {
        health = health + healStrength;
        if(health > maxHealth)
            health = maxHealth;

        DamageSplash();
    }
    
}
