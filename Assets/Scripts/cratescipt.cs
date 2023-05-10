using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cratescipt : MonoBehaviour
{
    Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] int zHealth = 20;
    public Material flashMaterial;
    public static Material defaultMaterial;
    [SerializeField] bool isFlashing;

    [SerializeField] GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void damageTaken(int Damage)
    {
        FontDisplay.instantiate("-" + Damage, new Vector2(transform.position.x, transform.position.y), FontDisplay.damagedColor, 1);
        if (renderer.material != flashMaterial)
        {
            GetComponent<SpriteRenderer>().material = flashMaterial;
            Invoke("endFlash", 0.5f);

        }

        zHealth -= Damage;

        if (zHealth <= 0)
        {
            cDeath();
        }
    }

    void cDeath()
    {
        animator.SetTrigger("Destroyed");
        EnemyGeneration.loadedObstacles.Remove(gameObject);
        Instantiate(items[Random.Range(0, 2)], transform.position, Quaternion.identity);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);

    }

    void endFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
}
