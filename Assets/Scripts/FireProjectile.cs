using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] int hitDamage;
    [SerializeField] float fireSpeed;
    [SerializeField] GameObject fireballCharge;
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

    public void Charge()
    {
        // Debug.Log("Charging");
        GameObject newFireball = Instantiate(fireballCharge, firePoint.position + firePoint.up*0.1f, firePoint.rotation);
        Destroy(newFireball, 1);
    }

    public void Shoot()
    {
        
        GameObject newBullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newBullet.GetComponent<Bullet>().damageAmount = hitDamage;
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(firePoint.up * fireSpeed, ForceMode2D.Impulse);
    }

    public void Shoot(Transform newPoint, Color color, float fireDistance)
    {
        GameObject newBullet = Instantiate(projectile, newPoint.position + newPoint.up * 0.2f, newPoint.rotation);
        newBullet.GetComponent<Bullet>().damageAmount = hitDamage;
        newBullet.GetComponent<SpriteRenderer>().color = color;
        newBullet.GetComponent<Bullet>().fireRange = fireDistance;
        Rigidbody2D bulletBody = newBullet.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(newPoint.up * fireSpeed, ForceMode2D.Impulse);
    }

    public void Spray(Transform point, int amount)
    {

    }
}
