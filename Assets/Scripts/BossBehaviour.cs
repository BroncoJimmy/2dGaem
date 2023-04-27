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
    Vector3[] positions = new Vector3[5] { new Vector3(0, -0.06f, 0), new Vector3(1.444f, 0.178f, 0), new Vector3(1.444f, -1.7f, 0), new Vector3(-1.5f, -1.7f, 0), new Vector3(-1.5f, 0.178f, 0) };

    FireProjectile projectileScript;
    EnemyGeneration enemyGeneration;

    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject child;
    [SerializeField] Transform firePoint;
    [SerializeField] List<Transform> firePoints = new List<Transform>();
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
        } else if (randomAttack == 2)
        {
            StartCoroutine(GreenAttack());
        }
    }

    public IEnumerator BlueAttack()
    {
        animator.SetBool("Blue", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Blue", false);
    }

    public IEnumerator RedAttack(int sprayCount)
    {
        animator.SetBool("Red", true);
        
        yield return new WaitForSeconds(1.5f);
        lookVector = Globals.player.transform.position - transform.position;
        pointAngle = Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg - 90f;
        child.transform.eulerAngles = (new Vector3(0, 0, pointAngle));
        float angleOffset = 0;
        float angleTotal = 0;
        for (int i = 0; i < sprayCount; i++)
        {
            child.transform.Rotate(new Vector3(0, 0, angleOffset));

            Debug.Log("Rotation: " + child.transform.rotation.eulerAngles.z);
            projectileScript.Shoot(firePoint, Color.red, 5f);
            angleTotal += 5f;
            angleOffset += angleOffset >= 0 ? -angleTotal : angleTotal;
            //Debug.Log(firePoint.transform.rotation);
        }
        animator.SetBool("Red", false);
    }
    
    public IEnumerator GreenAttack()
    {
        animator.SetBool("Green", true);
        int amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            Vector3 newPosition = new Vector2(Random.Range(0, 20) * .16f - 1.6f, Random.Range(0, 20) * .16f - 1.6f);
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
