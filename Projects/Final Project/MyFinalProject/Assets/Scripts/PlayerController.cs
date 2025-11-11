using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private Vector2 lastDir = Vector2.down; // default face down

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();   // animator on child sprite
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // movement params
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("IsMoving", isMoving);
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);

        // remember last facing dir when moving
        if (isMoving)
        {
            lastDir = moveInput;
            anim.SetFloat("LastX", lastDir.x);
            anim.SetFloat("LastY", lastDir.y);
        }

        // actions (1=Water, 2=Cut, 3=Gather)
        if (Input.GetKeyDown(KeyCode.Alpha1)) anim.SetTrigger("Water");
        if (Input.GetKeyDown(KeyCode.Alpha2)) anim.SetTrigger("Cut");
        if (Input.GetKeyDown(KeyCode.Alpha3)) anim.SetTrigger("Gather");

        // simple flip for left/right sprites if you use one right-facing sheet
        if (moveInput.x > 0.1f) sr.flipX = false;
        else if (moveInput.x < -0.1f) sr.flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
