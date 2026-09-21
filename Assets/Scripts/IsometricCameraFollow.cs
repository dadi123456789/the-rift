using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] Vector3 offset = new Vector3(0f, 10f, -10f);
    [SerializeField] float smoothTime = 0.15f;

    Vector3 velocity;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}