using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider;
    public SpriteRenderer spriteRenderer;

    // Center, TR, BR, BL, TL
    Vector3[] positions = new Vector3[5] { new Vector3(2.12f, 2.12f, 0), new Vector3(3.72f, 2.52f, 0), new Vector3(0.62f, 2.52f, 0), new Vector3(0.62f, 0.49f, 0), new Vector3(3.72f, 0.49f, 0) };

    FireProjectile projectileScript;
    EnemyGeneration enemyGeneration;

    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject spawnEffect;
    [SerializeField] GameObject child;
    [SerializeField] Transform firePoint;
    [SerializeField] List<Transform> firePoints = new List<Transform>();
    [SerializeField] GameObject deathTile;
    private Vector3 lookVector;
    private float pointAngle;

    float moveCountdown = 0f;
    float DELAY_TIME = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileScript = GetComponent<FireProjectile>();
        enemyGeneration = GetComponent<EnemyGeneration>();
        transform.position = positions[0];
        //StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0,0,1f));
        if (moveCountdown <= 0)
        {
            StartCoroutine(Attack());
            moveCountdown = DELAY_TIME;
        }
        moveCountdown -= Time.deltaTime;
    }

    public IEnumerator Attack()
    {
        animator.SetTrigger("Dissapear");
        

        yield return new WaitForSeconds(1f);
        transform.position = positions[Random.Range(0, 5)];
        animator.SetTrigger("Appear");

        yield return new WaitForSeconds(0.5f);
        int randomAttack = Random.Range(0, 3);
        Debug.Log(randomAttack);
        if (randomAttack == 0)
        {
            StartCoroutine(BlueAttack());
        } else if (randomAttack == 1)
        {
            StartCoroutine(RedAttack(11));
            StartCoroutine(RedAttack(11));
        } else if (randomAttack == 2)
        {
            StartCoroutine(GreenAttack());
        }
    }

    public IEnumerator BlueAttack()
    {
        animator.SetBool("Blue", true);
        for (int i = 0; i < 10; i++)
        {
            Vector2 playerPos = new Vector2((int) (Globals.player.transform.position.x / .32f), (int)(Globals.player.transform.position.y / .32f));
            Instantiate(deathTile, new Vector2(playerPos.x * .32f + 0.16f, playerPos.y * .32f + 0.16f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        
        animator.SetBool("Blue", false);
    }

    public IEnumerator RedAttack(int sprayCount)
    {
        animator.SetBool("Red", true);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
            lookVector = Globals.player.transform.position - transform.position;
            pointAngle = Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg - 90f;
            child.transform.eulerAngles = (new Vector3(0, 0, pointAngle));
            float angleOffset = 0;
            float angleTotal = 0;
            for (int j = 0; j < sprayCount; j++)
            {
                child.transform.Rotate(new Vector3(0, 0, angleOffset));

                Debug.Log("Rotation: " + child.transform.rotation.eulerAngles.z);
                projectileScript.Shoot(firePoint, Color.white, 5f);
                angleTotal += 5f;
                angleOffset += angleOffset >= 0 ? -angleTotal : angleTotal;
                //Debug.Log(firePoint.transform.rotation);
            }
            yield return new WaitForSeconds(0.5f);
        }
        animator.SetBool("Red", false);
    }
    
    public IEnumerator GreenAttack()
    {
        animator.SetBool("Green", true);
        int amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            Vector3 newPosition = new Vector2(Random.Range(0, 20) * .16f + 0.32f, Random.Range(1, 12) * .16f + 0.32f);
            GameObject effect = Instantiate(spawnEffect, newPosition, Quaternion.Euler(0, 0, Random.Range(0, 180)));
            effect.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.65f, 0.2f);
            Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSeconds(0.2f);
            EnemyGeneration.generateEnemy(enemies[Random.Range(0,3)], newPosition);
        }
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Green", false);
    }

    public void wasHit(GameObject weapon)
    {
        GetComponent<ZombieHealth>().damageTaken(weapon.GetComponent<Bullet>().damageAmount);

    }
}
