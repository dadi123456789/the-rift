using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 6f;

    Rigidbody rb;
    Collider col;
    bool jumpRequested;
    static readonly Quaternion IsoRotation = Quaternion.Euler(0f, 45f, 0f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void RequestJump() => jumpRequested = true;

    bool IsGrounded() => rb.position.y <= 1.05f && rb.velocity.y <= 0.1f;

    void FixedUpdate()
    {
        Vector2 input = VirtualJoystick.Instance != null
            ? VirtualJoystick.Instance.InputVector
            : Vector2.zero;

        Vector3 isoDirection = IsoRotation * new Vector3(input.x, 0f, input.y);
        Vector3 desiredHorizontal = isoDirection * moveSpeed;

        desiredHorizontal = PhysicsUtil.BlockMovementIntoObstacles(rb.position, desiredHorizontal, rb.rotation, col);

        rb.velocity = new Vector3(desiredHorizontal.x, rb.velocity.y, desiredHorizontal.z);

        if (isoDirection.sqrMagnitude > 0.01f)
            transform.forward = isoDirection;

        if (jumpRequested)
        {
            if (IsGrounded())
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpRequested = false;
        }

        if (rb.position.y < 1f)
        {
            rb.position = new Vector3(rb.position.x, 1f, rb.position.z);
            if (rb.velocity.y < 0f)
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}