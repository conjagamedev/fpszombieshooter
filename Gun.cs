using UnityEngine;
using System.Collections; 
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f; 
    public float range = 100f; 
    public float impactForce = 30f; 
    public float fireRate = 15f; 
    
    private Camera fpsCam; 
    public ParticleSystem muzzleFlash; 
    public GameObject stoneEffect; 
    public GameObject woodEffect; 
    public GameObject bloodEffect; 
    public GameObject cartrdigeEffect; 

    public int maxAmmoInClip = 10; 
    private int currentAmmo;
    public int overallAmmo; 
    public int totalMaxAmmo = 100; 
    public float reloadTime = 1f; 
    private bool isReloading = false; 

    private float nextTimeToFire = 0f; 
    private Animation animation; 
    [HideInInspector]
    public AudioSource audio; 
    public AudioClip shootAudioClip; 
    public AudioClip reloadAudioClip; 

    private AudioSource headshotAudio;

    private Text ammoTxt; 
    public string lastShotLand; 
    public TMPro.TextMeshPro currAmmoMTxt; 
    private Image weaponIcon; 
    public Sprite weaponSprite; 

    public bool isShooting; 

    void Start()
    {
        fpsCam = GameObject.Find("PLAYER CAMERA").GetComponent<Camera>();
        currentAmmo = maxAmmoInClip; 
        animation = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
        currAmmoMTxt = GetComponentInChildren<TMPro.TextMeshPro>();
        weaponIcon = GameObject.Find("weaponIcon").GetComponent<Image>();
        ammoTxt = GameObject.Find("ammoText").GetComponent<Text>();
    }

    void OnEnable() 
    {
        isReloading = false; 
    }
    void Update() 
    {
        if(isReloading)
            return; 

        if(currentAmmo <= 0 && overallAmmo > 0)
        {
            StartCoroutine(Reload());
            return; 
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f/fireRate; 
            Shoot();
        }

        if(Input.GetButton("Reload") && currentAmmo < maxAmmoInClip && overallAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        ammoTxt.text = "" + overallAmmo;
        currAmmoMTxt.text = "" + currentAmmo; 
        weaponIcon.sprite = weaponSprite;
        
        if(overallAmmo < 0)
            overallAmmo = 0;

    }

    void Shoot()
    {
        muzzleFlash.Play(); 
        GameObject cartGO = Instantiate(cartrdigeEffect, transform.position, Quaternion.LookRotation(transform.forward));
        Destroy(cartGO, 0.7f);
        
        if(this.name == "Pistol")
        {
            animation.Play("PistolShoot");
        }

        if(this.name == "M4_Carbine")
        {
            animation.Play("M4CarbineShoot");
        }

        if(this.name == "Shotgun")
        {
            animation.Play("ShotgunShoot");
        }
        
        audio.PlayOneShot(shootAudioClip);
        currentAmmo--;

        RaycastHit hit; 

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if(hit.transform.tag != null)
            {
                if(hit.collider.tag == "Zombie")
                {
                    hit.transform.gameObject.GetComponent<ZombieLocomotion>().Hit();
                    GameObject bloodGO = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodGO, 0.7f);
                    if(hit.collider.GetComponent<Target>().health > 0)
                        hit.transform.gameObject.GetComponent<Target>().TakeDamage(damage);
                    audio.PlayOneShot(hit.collider.GetComponent<ZombieLocomotion>().clips[2]);
                    
                    lastShotLand = "Body";
                }

                if(hit.collider.tag == "EnemyHead")
                {
                    hit.transform.gameObject.GetComponentInParent<ZombieLocomotion>().Hit(); 
                    GameObject bloodGO = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodGO, 0.7f);
                    hit.transform.gameObject.GetComponentInParent<Target>().TakeDamage(damage * 4);
                    headshotAudio = hit.collider.GetComponent<AudioSource>(); 
                    Target zombieTarget = hit.collider.GetComponentInParent<Target>();
                    if(zombieTarget.health < zombieTarget.maxHealth / 4)
                        headshotAudio.PlayOneShot(headshotAudio.clip);
                    else
                        audio.PlayOneShot(hit.collider.GetComponentInParent<ZombieLocomotion>().clips[2]);
                    
                    lastShotLand = "Head";
                }
                
                if(hit.transform.tag == "Stone")
                {
                    GameObject stoneGO = Instantiate(stoneEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(stoneGO, 0.7f);  
                }
                
                
                if(hit.transform.tag == "Wood")
                {
                    GameObject woodGO = Instantiate(woodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(woodGO, 0.7f);  
                }

            }

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce); 
            }

        }

        //----recoil??---
        /*Transform fpsTrans = fpsCam.GetComponent<Transform>();
        Quaternion curRot = fpsTrans.rotation; 
        float RotX = fpsCam.transform.rotation.x; 
        float RotY = fpsCam.transform.rotation.y; 
        float RotZ = fpsCam.transform.rotation.z; 
        MouseLook ml = fpsCam.GetComponent<MouseLook>();
        Quaternion newRot = Quaternion.Euler(RotX - Random.Range(20,40),RotY,RotZ);
        fpsTrans.Rotate(Vector3.right, Random.Range(0.5f,1.5f));*/

    }

    IEnumerator Reload()
    {
        if(overallAmmo > 0)
        {
            isReloading = true; 
            print("reload");
            if(gameObject.name == "Pistol")
            {
                animation.Play("PistolReload");
                audio.PlayOneShot(reloadAudioClip);
                yield return new WaitForSeconds(1f);
            }
            else if(gameObject.name == "M4_Carbine")
            {
                animation.Play("M4CarbineReload");
                audio.PlayOneShot(reloadAudioClip); 
                yield return new WaitForSeconds(2f);
            }
            else if(gameObject.name == "Shotgun")
            {
                animation.Play("ShotgunReload");
                audio.PlayOneShot(reloadAudioClip); 
                yield return new WaitForSeconds(1.5f);
            }
            int ammoDifference = maxAmmoInClip - currentAmmo; 
                
            if(overallAmmo < 0)
                overallAmmo = 0; 

            if(overallAmmo >= maxAmmoInClip)
                currentAmmo = maxAmmoInClip;
            else
            {
                currentAmmo = currentAmmo + overallAmmo; 
                
                if(currentAmmo > maxAmmoInClip)
                    currentAmmo = maxAmmoInClip; 
            }
            
            overallAmmo = overallAmmo - ammoDifference;
            isReloading = false; 
        }
    
    }

}
