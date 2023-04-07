using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider;
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.01f;
    private Vector3 moveVector;

    [SerializeField] float trackDistance = 2f;
    [SerializeField] float pointAngle;
    [SerializeField] float stopDistance = 0.15f;
    [HideInInspector] public Vector3 up = new Vector3(0, 0, 1);
    public Vector3 movingDirection;
    [SerializeField] float movingMagnitude;
    public Vector3 newPos;

    [SerializeField] bool canSeePlayer;
    public bool isStatic;
    [SerializeField] public float disMagnitude { get; set; }
    [SerializeField] float attackSpeed;
    public bool isLoaded { get; set; }

    [SerializeField] float attackDelayCountdown = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        newPos = transform.position;
        isLoaded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //animator.SetBool("isStatic", isStatic);
        if (isStatic)
        {
            return;
        }
        moveVector = Globals.player.transform.position - transform.position;

        if (lineOfSight(Globals.player.transform.position) && moveVector.magnitude < trackDistance)
        {
            newPos = Globals.player.transform.position;
            canSeePlayer = true;
        }
        else { moveVector = newPos - transform.position; canSeePlayer = false; }

        disMagnitude = moveVector.magnitude;
        movingMagnitude = disMagnitude;

        Debug.DrawRay(transform.position, newPos - transform.position, Color.green);

        pointAngle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg - 90f;

        if (moveVector.magnitude > stopDistance)
        {

            transform.rotation = Quaternion.AngleAxis(pointAngle, up);

            bool success = TryMove(moveVector);

            if (!success && !canSeePlayer)
            {
                Vector3 direction = alterCourse(moveVector);

            }
            else
            {
                movingDirection = moveVector;
            }

        }
        else if (moveVector.magnitude != 0 && moveVector.magnitude < stopDistance * 0.75f)
        {
            bool success = TryMove(-moveVector);

        }

        if ((Globals.player.transform.position - transform.position).magnitude < stopDistance + 0.01f && attackDelayCountdown <= 0)
        {
            transform.rotation = Quaternion.AngleAxis(pointAngle, up);
            animator.SetTrigger("Attack");
            GetComponent<FireProjectile>().Shoot();
            attackDelayCountdown = attackSpeed;


            
        }
        
        

    }

    private void Update()
    {
        if (attackDelayCountdown > 0)
        {
            attackDelayCountdown -= Time.deltaTime;
        }
    }

    public Vector3 alterCourse(Vector3 originalDirection)
    {
        if (!TryMove(movingDirection))
        {
            Vector3[] possibleDirecs = { new Vector3(moveSpeed, 0), new Vector3(0, moveSpeed), new Vector3(-moveSpeed, 0), new Vector3(0, -moveSpeed) };
            movingDirection = Vector3.zero;
            for (int i = 1; i < possibleDirecs.Length; i++)
            {
                Vector3 temp = possibleDirecs[i];
                int possibleIndex = i - 1;
                float currentMagnitude = (originalDirection - possibleDirecs[i]).magnitude;

                while (possibleIndex >= 0 && (originalDirection - possibleDirecs[possibleIndex]).magnitude > currentMagnitude)
                {
                    possibleDirecs[possibleIndex + 1] = possibleDirecs[possibleIndex];
                    possibleIndex--;

                }


                possibleDirecs[possibleIndex + 1] = temp;

            }
            bool moved = false;
            int index = 0;
            while (!moved && index < possibleDirecs.Length)
            {
                moved = TryMove(possibleDirecs[index]);
                if (moved)
                {
                    movingDirection = possibleDirecs[index];

                }


                index++;
            }


        }
        return movingDirection;
    }

    private bool lineOfSight(Vector3 targetLoc)
    {
        Vector3 distance = targetLoc - transform.position;

        if (collider.Raycast(targetLoc - transform.position, movementFilter, castCollisions, moveVector.magnitude) > 1)
        {
            return false;
        }
        else { return true; }
    }

    private bool TryMove(Vector2 direction)
    {
        //Check for collisions
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + 0.05f/*Collision offset*/);
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("X: " + castCollisions[0].collider.bounds.center.x + ", Y: " + castCollisions[0].collider.bounds.center.y);
        }*/
        Debug.DrawRay(transform.position, Globals.player.transform.position - transform.position, Color.red);
        if (count == 0)
        {
            // Movement is trimmed so the vector magnitude is always equal to moveSpeed
            rb.MovePosition(rb.position + new Vector2(direction.x * (moveSpeed / direction.magnitude), direction.y * (moveSpeed / direction.magnitude)) * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }

    }

    public IEnumerator Attack()
    {
        GetComponent<FireProjectile>().Shoot();
        yield return new WaitForSeconds(attackSpeed);
        if (animator.GetBool("isAttacking"))
        {
            StartCoroutine(Attack());
        }
    }

    public void wasHit(GameObject weapon)
    {
        animator.SetTrigger("isHit");
        GetComponent<ZombieHealth>().damageTaken(weapon.GetComponent<Bullet>().damageAmount);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.SendMessage("damageTaken", 20);
        }
    }

}
