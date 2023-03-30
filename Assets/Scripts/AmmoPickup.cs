using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmmoPickup : MonoBehaviour
{
    public AmmunitionItem ammoCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            AmmunitionItem ammoItem = GetComponent<AmmunitionItem>();
            int ammoCount = ammoItem.ammoCount;

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            AmmoSystem ammoSystem = playerObject.GetComponent<AmmoSystem>();

            ammoSystem.pistolAmmo += ammoCount;
            Destroy(gameObject);
        }
    }
}
