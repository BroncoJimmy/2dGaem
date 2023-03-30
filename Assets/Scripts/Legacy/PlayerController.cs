using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    public SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    [SerializeField] string currentAnimation;
    string[] playerAnims = { "Player_IdleD", "Player_IdleU", "Player_IdleX", "Player_WalkD", "Player_WalkU", "Player_WalkX" };
    [SerializeField] string maintain;
    int maintainNum;
    string[] maintainParams = {"isMovingYD", "isMovingYU", "isMovingX" };


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Globals.player = GameObject.FindGameObjectWithTag("Player");

    }

    void LateUpdate()
    {
        
        
    }
    private void FixedUpdate()
    {

        // Updates variable which stores the player's current animation
        for (int i = 0; i < playerAnims.Length; i++)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(playerAnims[i]))
            {
                Globals.currentPlayerAnim = playerAnims[i];
            }
        }
        currentAnimation = Globals.currentPlayerAnim;

        
        // Resets triggers which shouldn't be active.
        if (movementInput.y >= 0)
        {
            animator.SetBool("isMovingYD", false);
        }
        if (movementInput.y <= 0)
        {
            animator.SetBool("isMovingYU", false);
        }
        if (movementInput.x == 0)
        {
            animator.SetBool("isMovingX", false);
        }
        
        // Checks to see if an Idle animation should play.
        animator.SetBool("isIdle", isPlayerIdle());

        // Sets the "maintain" variable to hold the parameter of what should be the active animation 
        for (int i = 0; i < maintainParams.Length; i++)
        {
            if (animator.GetBool(maintainParams[i]))
            {

                maintain = maintainParams[i];
                maintainNum = i;
            }

        }

        // Checks to see if the current animation matches the parameter held in "maintain"
        if (Globals.currentPlayerAnim.Equals(playerAnims[maintainNum + 3]))
        {
            //Debug.Log(playerAnims[maintainNum + 3] + Globals.currentPlayerAnim);
            animator.SetBool("isStatic", true);
        } else
        {
            animator.SetBool("isStatic", false);
        }

        // Checks to see if the maintain variable should be reset (the parameter is no longer true and should no longer play the same animation)
        if  (!maintain.Equals("none") && !animator.GetBool(maintain))
        {
            maintain = "none";
            animator.SetBool("isStatic", false);

        } 

        //If movementInput != 0 then try to move
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(new Vector2(movementInput.x, movementInput.y));
            // If there is no animation to maintain, allow new triggers to be activated.
            if (maintain.Equals("none"))
            {
                
                if (movementInput.x != 0)
                {
                    animator.SetBool("isMovingX", true);

                } 
                else if (movementInput.y > 0)
                {
                    animator.SetBool("isMovingYU", true);

                }
                else if (movementInput.y < 0)
                {
                    animator.SetBool("isMovingYD", true);
                }
            }

        }


        //set direction of sprite
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        

    }


    private bool TryMove(Vector2 direction)
    {
        //Check for collisions
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }

    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    bool isPlayerIdle()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        return false;
    }
}

