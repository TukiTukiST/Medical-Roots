using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 1. Lectura de entrada horizontal
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Detección de suelo
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // 3. Captura del pulso de salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        // 4. Inversión de escala según dirección
        Flip();

        // 5. Parámetros del Animator
        if (anim != null)
        {
            anim.SetFloat("velocityX", Mathf.Abs(horizontalInput));
            anim.SetFloat("velocityY", rb.linearVelocity.y);
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    private void FixedUpdate()
    {
        // Aplicar movimiento horizontal por físicas
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        // Aplicar salto seguro en FixedUpdate
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private void Flip()
    {
        if ((horizontalInput > 0 && !facingRight) || (horizontalInput < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}