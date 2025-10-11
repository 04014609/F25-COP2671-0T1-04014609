using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // adjustable in Inspector

    private Rigidbody2D rb;     // reference to Rigidbody2D
    private Vector2 moveInput;  // stores movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal (A/D or Left/Right) and vertical (W/S or Up/Down) input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create a movement vector and normalize it
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Move player smoothly based on speed
        rb.linearVelocity = moveInput * speed;
    }
}
