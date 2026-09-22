using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float touchDeadZone = 15f;

    Rigidbody rb;
    static readonly Quaternion IsoRotation = Quaternion.Euler(0f, 45f, 0f);

    int activeTouchId = -1;
    Vector2 touchOrigin;
    Vector2 currentInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Prevents the physics engine from letting the capsule tunnel through the ground.
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && activeTouchId == -1)
            {
                activeTouchId = touch.fingerId;
                touchOrigin = touch.position;
            }
            else if (touch.fingerId == activeTouchId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector2 delta = touch.position - touchOrigin;
                    currentInput = delta.magnitude > touchDeadZone ? delta.normalized : Vector2.zero;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    activeTouchId = -1;
                    currentInput = Vector2.zero;
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 isoDirection = IsoRotation * new Vector3(currentInput.x, 0f, currentInput.y);
        Vector3 desiredHorizontal = isoDirection * moveSpeed;

        // Only drive X/Z with our input; leave Y velocity untouched so gravity behaves normally.
        rb.velocity = new Vector3(desiredHorizontal.x, rb.velocity.y, desiredHorizontal.z);

        if (isoDirection.sqrMagnitude > 0.01f)
            transform.forward = isoDirection;
    }
}