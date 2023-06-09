using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class ZombieHealth : MonoBehaviour
{
    public int dmgXP;
    public GameSummary summary;
    [SerializeField] bool isDead = false;

    Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] int zHealth = 100;
    public Material flashMaterial;
    public static Material defaultMaterial;
    [SerializeField] bool isFlashing;
    [SerializeField] GameObject[] itemDrops;
    [SerializeField] float dropRate = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;
        summary = GameObject.FindGameObjectWithTag("Summary").GetComponent<GameSummary>();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void damageTaken(int Damage)
    {
        if (isDead) {
            return;
        }

        FontDisplay.instantiate("-" + Damage, new Vector2(transform.position.x, transform.position.y), FontDisplay.damagedColor, 1);
        if (renderer.material != flashMaterial)
        {
            GetComponent<SpriteRenderer>().material = flashMaterial;
            Invoke("endFlash", 0.5f);

        }

        zHealth -= Damage;

        if (zHealth <= 0)
        {
            zDeath();
            DamageXP dmgXP = Globals.player.GetComponent<DamageXP>();
            summary.kills += 1;
            dmgXP.shootXP += 10;
            isDead = true;
        }
    }

    void zDeath()
    {
        animator.SetTrigger("Death");
        EnemyGeneration.loadedEnemies.Remove(gameObject);
        //Debug.Log("# of enemies: " + EnemyGeneration.loadedEnemies.Count);
        // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_Walk"));
        if (Random.Range(1, 101) < dropRate * 100)
        {
            Instantiate(itemDrops[0], transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 2f);
        //Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        //Death animation?
    }

    void endFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
}
