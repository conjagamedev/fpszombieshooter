using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieLocomotion : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator animator; 
    private AudioSource audio;
    public AudioClip[] clips;  

    private Transform playerPos; 
    private PlayerHealth playerHealth; 
    [SerializeField]
    private bool isAttacking; 

    private float dist; 
    private float atkDist = 1.25f;
    public bool isDead; 
    private PlayerLocomotion playerLocomotion; 
    private Target target; 

    public GameObject dieEffect; 
    public int level; 
    public float speed; 

    private GameManager gameManager;

    private void Start() 
    {
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        target = GetComponent<Target>();
        playerLocomotion = GameObject.FindWithTag("Player").GetComponent<PlayerLocomotion>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if(level == 1)
        {
            speed = Random.Range(2f,2.5f);
            target.maxHealth = 50f; 
        }

        if(level == 2)
        {
            speed = Random.Range(3f,3.5f);
            target.maxHealth = 100f;
        }

        target.health = target.maxHealth; 
        agent.speed = speed;

        InvokeRepeating("SpawnGrowlAudio", Random.Range(2,5), Random.Range(5,30));
    }

    private void Update()
    {
        if(!isDead)
        {
            if(playerLocomotion.isGrounded != null)
            {
                if(playerPos.position.y > 2.15f)
                {
                    atkDist = 3f;
                }
                else
                {
                    atkDist = 2f;
                }
            }

            dist = Vector3.Distance(playerPos.position, transform.position);

            if(agent.destination != playerPos.position && dist >= atkDist && !isAttacking)
            {
                //agent.destination = playerPos.position;
                agent.SetDestination(playerPos.position);
            }


            if(dist < atkDist && !isAttacking)
            {
                isAttacking = true; 
                StartCoroutine(AttackAnimation());
            }

            if(level == 1)
            {
                if(agent.velocity.magnitude > 0 && !isAttacking)
                {
                    animator.SetBool("isWalk", true);
                }
                
                if(agent.velocity.magnitude == 0 && !isAttacking)
                {
                    animator.SetBool("isWalk", false);
                }
            }
            if(level == 2)
            {
                if(agent.velocity.magnitude > 0 && !isAttacking)
                {
                    animator.SetBool("isRun", true);
                }
                if(agent.velocity.magnitude == 0 && !isAttacking)
                {
                    animator.SetBool("isRun", false);
                }
            }
        }
    }

    public void Hit()
    {
        StartCoroutine(HitAnimation());
    }

    public void SpawnGrowlAudio()
    {
        int rand = Random.Range(0,11);
        if(rand <= 5)
            return;
        else if(rand <= 8)
            audio.PlayOneShot(clips[0]);
        else
            audio.PlayOneShot(clips[1]);
    }

    IEnumerator HitAnimation()
    {
        agent.speed = 0; 
        animator.SetBool("beenShot", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("beenShot", false);
        agent.speed = speed; 
    }

    IEnumerator AttackAnimation()
    { 
        agent.speed = 0;
        transform.LookAt(playerPos);
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("isAttack", false);
        isAttacking = false; 
        agent.speed = speed;
    }

    public void DamagePlayer()
    {
        playerHealth.PlayerTakeDamage(10);
    }

    public void StartDeath()
    {
        GetComponent<Rigidbody>().mass = 0.5f; 
        StartCoroutine(Die());
        GameObject dieGO = Instantiate(dieEffect, transform.position, Quaternion.LookRotation(transform.forward));
        Destroy(dieGO, 0.7f);

    }

    public void AttackSound()
    {
        audio.PlayOneShot(clips[3]);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.35f);
        if(isDead)
        {
            isDead = false;
            int rand;
            if(GameObject.FindWithTag("Player").GetComponentInChildren<Gun>().lastShotLand == "Head")
            {
                rand = Random.Range(50,80) * 2; 
                gameManager.UpHeadShot(1);
            }
            else
            {
                rand = Random.Range(50,80);
            }

            gameManager.UpCash(rand);
            gameManager.zombiesKilledInRound++;
            gameManager.currentZombiesAlive--;
        }
        Destroy(gameObject);
        
    }
    
}
