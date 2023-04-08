using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Material flashMaterial;
    [HideInInspector] public static Material defaultMaterial;
    public int playerHealth = 100;
    [SerializeField] GameObject deadPlayerPrefab;
    public void damageTaken(int Damage)
    {
        FontDisplay.instantiate("-" + Damage, new Vector2(transform.position.x - 0.02f, transform.position.y - 0.02f), FontDisplay.damagedColor, 1);
        GetComponent<SpriteRenderer>().material = flashMaterial;
        GameObject.FindGameObjectWithTag("Gun").GetComponent<SpriteRenderer>().material = flashMaterial;
        Invoke("endFlash", 0.5f);
        playerHealth -= Damage;

        if (playerHealth <= 0)
        {
            playerDeath();
        }
    }

    void playerDeath()
    {
        GetComponent<PlayerScript>().isStatic = true;
        GetComponent<Animator>().SetTrigger("Death");
        GameObject.FindGameObjectWithTag("Gun").SendMessage("PlayerDeath");
        GameObject playerPlacebo = Instantiate(deadPlayerPrefab, transform.position, transform.rotation);
        Globals.player = playerPlacebo;
        GameObject.FindGameObjectWithTag("MainCamera").SendMessage("PlayerChange", playerPlacebo);
        Invoke("displaySummary", 1.5f);
        Destroy(gameObject, 1.5f);

    }

    void endFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
        GameObject.FindGameObjectWithTag("Gun").GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void displaySummary()
    {
        GameObject.FindGameObjectWithTag("Summary").SendMessage("Activate");
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
