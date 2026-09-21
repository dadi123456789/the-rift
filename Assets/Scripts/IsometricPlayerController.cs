using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float touchDeadZone = 20f;

    Rigidbody rb;
    static readonly Quaternion IsoRotation = Quaternion.Euler(0f, 45f, 0f);

    int activeTouchId = -1;
    Vector2 touchOrigin;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width * 0.5f && activeTouchId == -1)
            {
                activeTouchId = touch.fingerId;
                touchOrigin = touch.position;
            }
            if (touch.fingerId == activeTouchId &&
                (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
            {
                activeTouchId = -1;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        if (activeTouchId != -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.fingerId == activeTouchId)
                {
                    Vector2 delta = touch.position - touchOrigin;
                    if (delta.magnitude > touchDeadZone)
                        inputVector = delta.normalized;
                }
            }
        }

        Vector3 isoDirection = IsoRotation * new Vector3(inputVector.x, 0f, inputVector.y);
        rb.MovePosition(rb.position + isoDirection * moveSpeed * Time.fixedDeltaTime);

        if (isoDirection.sqrMagnitude > 0.01f)
            transform.forward = isoDirection;
    }
}