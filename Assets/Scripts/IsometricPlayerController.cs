using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    Rigidbody rb;
    // Isometric movement is rotated 45° so WASD feels diagonal on screen.
    static readonly Quaternion IsoRotation = Quaternion.Euler(0f, 45f, 0f);

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;
        Vector3 isoDirection = IsoRotation * input;

        Vector3 newPosition = rb.position + isoDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (isoDirection.sqrMagnitude > 0.01f)
            transform.forward = isoDirection;
    }
}