using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionItem : MonoBehaviour
{
    [SerializeField] public int ammoCount;
    [SerializeField] SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        ammoCount = 2 + Random.Range(1, 9);

        renderer.color = new Color(1, 1f - (float)((ammoCount - 3) * 0.05), 1f - (float)((ammoCount-3) * 0.1));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {

            Globals.player.GetComponent<AmmoSystem>().pistolAmmo += ammoCount;
            FontDisplay.instantiate("+" + ammoCount, transform.position, FontDisplay.bulletColor, 1);
            Destroy(gameObject);
        }
    }
}
