using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] float distance = 14f;
    [SerializeField] float smoothTime = 0.15f;

    static readonly Quaternion IsoRotation = Quaternion.Euler(30f, 45f, 0f);
    Vector3 velocity;

    void Start() => transform.rotation = IsoRotation;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 offset = IsoRotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}