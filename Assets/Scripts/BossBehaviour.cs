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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileScript = GetComponent<FireProjectile>();
        enemyGeneration = GetComponent<EnemyGeneration>();
        transform.position = positions[0];
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            StartCoroutine(RedAttack());
        } else if (randomAttack == 2)
        {
            StartCoroutine(GreenAttack());
        }
    }

    public IEnumerator BlueAttack()
    {
        animator.SetBool("Blue", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("Blue", false);
    }

    public IEnumerator RedAttack()
    {
        animator.SetBool("Red", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("Red", false);
    }
    
    public IEnumerator GreenAttack()
    {
        animator.SetBool("Green", true);
        for (int i = 0; i < 3; i++)
        {
            Vector3 newPosition = new Vector2(Random.Range(0, 20) * .16f - 1.6f, Random.Range(0, 20) * .16f - 1.6f);
            EnemyGeneration.generateEnemy(enemies[Random.Range(0,3)], newPosition);
        }
        yield return new WaitForSeconds(2f);
        animator.SetBool("Green", false);
    }

    public void wasHit(GameObject weapon)
    {
        GetComponent<ZombieHealth>().damageTaken(weapon.GetComponent<Bullet>().damageAmount);

    }
}
