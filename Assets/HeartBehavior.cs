using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehavior : MonoBehaviour
{
    [SerializeField] int healingAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            Globals.player.GetComponent<PlayerHealth>().playerHealth += healingAmount;
            FontDisplay.instantiate("+" + healingAmount, transform.position, FontDisplay.damagedColor, 1);
            Destroy(gameObject);
        }
    }
}
