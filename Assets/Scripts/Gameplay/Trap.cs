using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int damageAmount;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Globals.ENEMY_LAYER || (collision.gameObject.layer == Globals.PLAYER_LAYER && !collision.isTrigger))
        {
            animator.SetTrigger("Activate");
            if (collision.gameObject.tag.Equals("Enemy") || !collision.gameObject.GetComponent<DashAbility>().isDashing)
            {
                collision.gameObject.SendMessage("damageTaken", damageAmount);

            }
        }
        
    }
}
