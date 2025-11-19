using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;
    public Vector2 LastMoveDirection => lastMoveDir;


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
            lastMoveDir = moveInput;
            anim.SetFloat("MoveX", moveInput.x);
            anim.SetFloat("MoveY", moveInput.y);
            anim.SetFloat("LastX", lastMoveDir.x);
            anim.SetFloat("LastY", lastMoveDir.y);

            // Farming hotkeys
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                FarmingController.OnHoe.Invoke();
                Debug.Log("HOE pressed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FarmingController.OnWater.Invoke();
                Debug.Log("WATER pressed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                FarmingController.OnSeed.Invoke();
                Debug.Log("SEED pressed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                FarmingController.OnHarvest.Invoke();
                Debug.Log("HARVEST pressed");
            }
        }

        // Flip sprite
        if (lastMoveDir.x > 0.1f) sr.flipX = false;
        else if (lastMoveDir.x < -0.1f) sr.flipX = true;

        // ===============================
        //   ACTION HOTKEYS (REQUIRED)
        // ===============================

        if (Input.GetKeyDown(KeyCode.Alpha1))
            FarmingController.OnHoe.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            FarmingController.OnWater.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            FarmingController.OnSeed.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            FarmingController.OnHarvest.Invoke();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
