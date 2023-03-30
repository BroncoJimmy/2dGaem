using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{

    public bool isDashing = false;
    [SerializeField] bool isDashAvailable = true;
    [SerializeField] int dashReloadTime = 3;
    [SerializeField] float dashSpeed =  1f;
    [SerializeField] float dashingTime = 0.1f;
    Vector2 dashVector;
    int dashCount;

    [SerializeField] Animator animator;
    [SerializeField] TrailRenderer tr;
    [SerializeField] Collider2D bodyCollider;
    public Transform colliderTransform;
    PlayerScript player_script;


    // Start is called before the first frame update
    void Start()
    {
        player_script = GetComponent<PlayerScript>();
        animator = GetComponent<Animator>();
        //dashSpeed = player_script.moveSpeed * 10f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashAvailable && !isDashing)
        {

            dashVector = new Vector2(Globals.lookDirection.x * (1 / Globals.lookDirection.magnitude), Globals.lookDirection.y * (1/Globals.lookDirection.magnitude));

            
            StartCoroutine(Dash());
            
            /*isDashing = true;
            isDashAvailable = false;
            TryDash(Globals.lookDirection);
            Invoke("ReloadDash", dashReloadTime);*/
            
        }
        if (isDashing)
        {
             /*new Vector2(Globals.lookDirection.x * (dashSpeed / Globals.lookDirection.magnitude), Globals.lookDirection.y * (dashSpeed / Globals.lookDirection.magnitude ))*/;
            dashCount++;
            TryDash(dashVector);
        }

        //player_script.animator.SetBool("isDashing", isDashing);
        animator.SetBool("isDashing", isDashing);

    }

    // No idea what the fuck an IEnumerator is but the tutorial told me to do it.
    private IEnumerator Dash()
    {
        Vector3 startPosition = Globals.player.transform.position;
        dashCount = 0;
        //Debug.Log("FPS: " + (double)(1.0 / Time.deltaTime));
        isDashAvailable = false;
        isDashing = true;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        tr.emitting = false;
        //Debug.Log("DISTANCE: " + (startPosition - Globals.player.transform.position).magnitude + "; DASH COUNT: " + dashCount);

        yield return new WaitForSeconds(dashReloadTime);
        isDashAvailable = true;
    }
    
    private bool TryDash(Vector2 direction)
    {
        //Debug.Log(direction.magnitude);
        //Check for collisions
        int count = player_script.rb.Cast(direction, player_script.movementFilter, player_script.castCollisions, dashSpeed * Time.deltaTime + 0.02f);
        //int count = Physics2D.CircleCast(colliderTransform.position, bodyCollider.bounds.extents.x + 0.01f, direction, player_script.movementFilter, player_script.castCollisions, dashSpeed * Time.deltaTime);


        if (count == 0)
        {
            player_script.rb.MovePosition(new Vector2(bodyCollider.transform.position.x, bodyCollider.transform.position.y) + direction * dashSpeed * Time.deltaTime);

            /*Debug.Log("True");
            Debug.DrawLine(bodyCollider.transform.position, new Vector2(bodyCollider.transform.position.x, bodyCollider.transform.position.y) + direction * (Time.deltaTime + bodyCollider.bounds.extents.x + 0.01f), Color.white, 3f);
            if (!Input.GetKey(KeyCode.C))
            {  }*/
            return true;
        }
        else
        {
            //Debug.Log("blocked");
            isDashing = false;
            return false;

        }

    }
}
