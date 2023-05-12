using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float tickTime;
    [SerializeField] float animTime;
    public bool isLive = false;
    bool isExploding = false;
    // Start is called before the first frame update
    void Start()
    {
        isLive = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isLive && GetComponent<CapsuleCollider2D>().isTrigger != false)
        {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            StartCoroutine(DelayedExplode());
        }
    }

    private IEnumerator DelayedExplode()
    {
        yield return new WaitForSeconds(tickTime);
        animator.SetTrigger("Flash");
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().Shake(1.0f);
        animator.SetTrigger("Explode");
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(animTime);
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            return;
        }
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Crate") && isLive)
        {
            collision.gameObject.SendMessage("damageTaken", 100);
        }

        if (collision.gameObject.tag == "Player" && !isLive)
        {
            collision.gameObject.GetComponent<PlayerScript>().numGrenades++;
            FontDisplay.instantiate("+1", transform.position, FontDisplay.bombColor , 1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Explode());

    }
}
