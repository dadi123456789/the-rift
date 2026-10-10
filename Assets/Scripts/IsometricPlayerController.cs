using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricPlayerController : MonoBehaviour
{
    public const int GroundLayer = 9;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 6f;

    Rigidbody rb;
    bool jumpRequested;
    static readonly Quaternion IsoRotation = Quaternion.Euler(0f, 45f, 0f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void RequestJump() => jumpRequested = true;

    // Native raycast against a dedicated Ground layer only — reliable even
    // when enemies are standing directly underneath the player.
    bool IsGrounded()
    {
        int mask = 1 << GroundLayer;
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, mask);
    }

    void FixedUpdate()
    {
        Vector2 input = VirtualJoystick.Instance != null
            ? VirtualJoystick.Instance.InputVector
            : Vector2.zero;

        Vector3 isoDirection = IsoRotation * new Vector3(input.x, 0f, input.y);
        Vector3 desiredHorizontal = isoDirection * moveSpeed;

        // Let PhysX itself resolve collisions with walls/obstacles — no manual check.
        rb.velocity = new Vector3(desiredHorizontal.x, rb.velocity.y, desiredHorizontal.z);

        if (isoDirection.sqrMagnitude > 0.01f)
            transform.forward = isoDirection;

        if (jumpRequested)
        {
            if (IsGrounded())
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpRequested = false;
        }
    }
}