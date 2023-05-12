using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject enemy in EnemyGeneration.loadedEnemies)
            {
                Destroy(enemy);
            }
            GameObject.FindGameObjectWithTag("Summary").SendMessage("Activate");
        }
    }


}
