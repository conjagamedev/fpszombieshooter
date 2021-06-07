using UnityEngine;
using UnityEngine.AI; 

public class Target : MonoBehaviour
{
    public float health = 50f;  
    public float maxHealth = 100f;
    public Animator animator; 
    
    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage (float amount)
    {
        health -= amount; 
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if(gameObject.tag == "Zombie")
        {
            animator.SetBool("isDead", true);
            animator.SetBool("isWalk", false); 
            animator.SetBool("beenShot", false);
            animator.SetBool("isAttack", false);
            gameObject.GetComponent<NavMeshAgent>().enabled = false; 
            gameObject.GetComponent<ZombieLocomotion>().isDead = true; 
            gameObject.GetComponent<ZombieLocomotion>().StartDeath();
        }
    }

}
