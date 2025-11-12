using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down; // default facing front

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("MoveX", moveInput.x);
            anim.SetFloat("MoveY", moveInput.y);

            lastMoveDir = moveInput; // << IMPORTANT FIX

            anim.SetFloat("LastX", lastMoveDir.x);
            anim.SetFloat("LastY", lastMoveDir.y);
        }

        // Flip sprite visually
        if (lastMoveDir.x > 0.1f) sr.flipX = false;
        else if (lastMoveDir.x < -0.1f) sr.flipX = true;

        // Tool / Action triggers
        if (Input.GetKeyDown(KeyCode.Alpha1))
            anim.SetTrigger("Water");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            anim.SetTrigger("Cut");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            anim.SetTrigger("Gather");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
