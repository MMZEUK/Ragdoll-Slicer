using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;

    Rigidbody rb;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 10f;
    public float groundDrag = 6f;

    [Header("Jump")]
    public float jumpForce = 6f;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.5f;
    bool isGrounded;
    float coyoteTime = 0.15f;
    float coyoteCounter;
    bool jumpRequest;

    [Header("Slide")]
    public float slideForce = 10f;
    public float slideStopSpeed = 2f;
    bool isSliding;
    bool slideRequest;

    float horizontalInput;
    float verticalInput;
    bool sprintInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        GetInput();
        CheckGround();

        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequest = true;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            slideRequest = true;
    }

    void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
        ApplyDrag();

        if (jumpRequest)
            Jump();

        if (slideRequest)
            StartSlide();

        if (isSliding)
            CheckSlideStop();

        jumpRequest = false;
        slideRequest = false;
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        sprintInput = Input.GetKey(KeyCode.LeftShift);
    }

    void RotatePlayer()
    {
        Quaternion targetRot = Quaternion.Euler(0f, orientation.eulerAngles.y, 0f);
        rb.MoveRotation(targetRot);
    }

    void MovePlayer()
    {
        if (isSliding) return;

        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDir.Normalize();

        float targetSpeed = sprintInput ? sprintSpeed : walkSpeed;

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 targetVel = moveDir * targetSpeed;
        Vector3 velocityChange = targetVel - flatVel;

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
    }

    void ApplyDrag()
    {
        rb.linearDamping = (isGrounded && !isSliding) ? groundDrag : 0f;
    }

    void Jump()
    {
        if (coyoteCounter <= 0f) return;

        coyoteCounter = 0f;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartSlide()
    {
        if (!isGrounded || isSliding) return;

        isSliding = true;
        rb.AddForce(orientation.forward * slideForce, ForceMode.Impulse);
    }

    void CheckSlideStop()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude < slideStopSpeed)
            isSliding = false;
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundMask);
    }
}