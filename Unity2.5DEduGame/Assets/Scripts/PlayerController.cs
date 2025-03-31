using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    [Header("2.5D Settings")]
    public float zOffset = 0.1f; // Depth offset for 2.5D effect
    public Transform visualTransform; // Reference to visual child object

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, groundLayer);

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement
        movement = new Vector3(horizontal, 0, vertical) * moveSpeed;

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // 2.5D visual effect - tilt based on movement
        if (visualTransform != null)
        {
            float tiltAmount = Mathf.Clamp(horizontal * 15f, -25f, 25f);
            visualTransform.localRotation = Quaternion.Euler(0, 0, -tiltAmount);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;
        
        // Add depth offset for 2.5D effect
        newPosition.z = newPosition.y * zOffset;
        
        rb.MovePosition(newPosition);
    }

    public void FreezePlayer()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        this.enabled = false;
    }

    public void UnfreezePlayer()
    {
        rb.isKinematic = false;
        this.enabled = true;
    }
}