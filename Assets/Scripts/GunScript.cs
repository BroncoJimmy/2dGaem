using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    SpriteRenderer renderer;
    [SerializeField] float offset;
    float xOffset;
    float yOffset;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootEffect;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireSpeed = 20f;
    AmmoSystem ammoSystem;

    [SerializeField] public int gunDamage = 20;

    [SerializeField] GameObject grenade;
    [SerializeField] bool isThrowing;
    [SerializeField] float throwSpeed = 0;

    DashAbility dash_script;

    private void Awake()
    {
        dash_script = Globals.player.GetComponent<DashAbility>();
        ammoSystem = Globals.player.GetComponent<AmmoSystem>();
        Globals.gun = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        ammoSystem.pistolAmmo = 20;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (dash_script.isDashing)
        {
            renderer.enabled = false;

        } else if (!renderer.enabled)
        {
            renderer.enabled = true;
        }*/
        
        transform.rotation = Globals.player.transform.rotation;
        
        xOffset = ((int)(Globals.lookDirection.x * (offset / Globals.lookDirection.magnitude) * 100)) / 100f;
        yOffset = ((int)(Globals.lookDirection.y * (offset / Globals.lookDirection.magnitude) * 100)) / 100f;

        transform.position = Globals.player.transform.position + new Vector3(xOffset, yOffset, -2);

        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        } else if (!animator.GetBool("isStatic"))
        {
            animator.SetBool("isStatic", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Globals.player.GetComponent<PlayerScript>().numGrenades > 0)
        {
            isThrowing = true;
            throwSpeed = 0;
        }
        
        if (isThrowing)
        {
            if (Input.GetMouseButtonUp(1) || throwSpeed > 1)
            {
                isThrowing = false;
                StartCoroutine(ThrowGrenade(throwSpeed));
            }
            else
            {
                throwSpeed += (Time.deltaTime/2);
            }
        }
        

    }

    public void Shoot()
    {

        //Only fire when you have ammo
        
        if (ammoSystem.pistolAmmo > 0 && renderer) {

        
        animator.SetTrigger("Fired");
        GameObject effect = Instantiate(shootEffect, firePoint.position, Quaternion.Euler(0,0, firePoint.eulerAngles.z + 180f));
        Destroy(effect, 0.1f);
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Bullet>().damageAmount = gunDamage;
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(-firePoint.up * fireSpeed, ForceMode2D.Impulse);
        ammoSystem.pistolAmmo --;
        }
    }

    public IEnumerator ThrowGrenade(float force)
    {
        Debug.Log("Thrown");
        GameObject newGrenade = Instantiate(grenade, firePoint.position, firePoint.rotation);
        Rigidbody2D grenadeBody = newGrenade.GetComponent<Rigidbody2D>();
        grenadeBody.AddForce(-firePoint.up * force, ForceMode2D.Impulse);
        yield return new WaitForEndOfFrame();
        newGrenade.GetComponent<Grenade>().isLive = true;
    }

    public void PlayerDeath()
    {
        Destroy(gameObject);
    }
    
    public void ChangeRenderer(bool on)
    {
        Debug.Log("Gun set to : " + on);
        renderer.enabled = on;
    }
}
