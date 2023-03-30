using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] int hitDamage;
    [SerializeField] float fireSpeed;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        
        GameObject newBullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Bullet>().damageAmount = hitDamage;
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(firePoint.up * fireSpeed, ForceMode2D.Impulse);
    }
}
