using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Globals
{
    public static Vector2 lookDirection;
    public static float mouseRatio;
    public static float[] limitArray = new float[2];
    public static string currentPlayerAnim = "Player_IdleD";
    public static GameObject player;
    public static GameObject gun;
    public static bool playerHoldingGun;
    public static int PLAYER_LAYER { get { return 3; } }

    public static int WATER_LAYER { get { return 4; } }
    public static int ENEMY_LAYER { get { return 6; } }
    public static int FLYING_LAYER { get { return 9; } }
    public static int ITEM_LAYER { get { return 8; } }


    public static System.Random random = new System.Random();

    public static double GenerateSkewedRandomNumber(double a, double b, double skewness)
    {
        double mu = (a + b) / 2;
        double sigma = (b - a) / (2 * skewness);

        double x = random.NextDouble();
        double y = random.NextDouble();

        double z = Mathf.Sqrt(-2 * Mathf.Log((float)x)) * Mathf.Cos(2 * Mathf.PI * (float)y);
        double result = Mathf.Exp((float)(mu + sigma * z));

        if (result < a || result > b)
        {
            result = GenerateSkewedRandomNumber(a, b, skewness);
        }
        
        return result;
    }
}

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.01f;
    public ContactFilter2D movementFilter;
    public Vector2 movementInput;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip walkingAudio;
    float walkCountdown = 0;

    [HideInInspector] public bool isAttacking;

    Collider2D bodyCollider;

    [HideInInspector] public List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    [SerializeField] string currentAnimation;
    public bool isStatic;

    [SerializeField] Camera mainCam;
    [SerializeField] CameraShake camShake;
    Vector2 mousePos;
    public Vector3 up = new Vector3(0, 0, 1);
    [SerializeField] bool isDashAvailable = true;
    public int numGrenades = 0;

    int meleeDamage { get; set; }

    private void Awake()
    {
        //Debug.Log("Player set to: " + GameObject.FindGameObjectWithTag("Player"));
        Globals.player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().Shake(3.0f);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyCollider = GetComponent<Collider2D>();
        numGrenades = 5;
        meleeDamage = 25;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(SwordAttack());
        }
        walkCountdown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {

        if (isStatic)
        {
            return;
        }

        // Checks to see if an Idle animation should play.
        animator.SetBool("isWalking", !isPlayerIdle());

        

        //If movementInput != 0 then try to move
        if (movementInput != Vector2.zero && !GetComponent<DashAbility>().isDashing)
        {
            bool success = TryMove(new Vector2(movementInput.x, movementInput.y));
            /*if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }*/
            animator.SetBool("isWalking", true);
            if (walkCountdown <= 0)
            {
                walkCountdown = 0.375f;
                audioSource.PlayOneShot(walkingAudio);
            }
        }


        // Converts mouse position to Vector2 coordinates on the screen
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Creates a Vector that points from gun to mouse. Limits arc with limitVector function.
        Globals.lookDirection = mousePos - new Vector2(transform.position.x, transform.position.y);

    }

    void LateUpdate()
    {
        float pointAngle = Mathf.Atan2(Globals.lookDirection.y, Globals.lookDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(pointAngle + 180, up);

    }

    private bool TryMove(Vector2 direction)
    {
        //Check for collisions
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime);
        //int count = Physics2D.CircleCast(GetComponent<DashAbility>().colliderTransform.position, bodyCollider.bounds.extents.x + 0.01f, direction, movementFilter, castCollisions, moveSpeed * Time.deltaTime);
        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            // Debug.Log("blocked");
            return false;

        }

    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>().normalized;
    }

    private IEnumerator SwordAttack()
    {
        Globals.gun.SendMessage("ChangeRenderer", false);
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        //Globals.gun.SendMessage("ChangeRenderer", true);
    }


    private void ReloadDash()
    {
        isDashAvailable = true;
    }

    bool isPlayerIdle()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Crate"))
        {
            collision.gameObject.SendMessage("damageTaken", meleeDamage);

        }
    }

    public void TransitionScene()
    {
        Invoke("BossRoom", 0.1f);
    }

    private void BossRoom()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.position = new Vector3(2f, 0.9f, 0);
    }

}