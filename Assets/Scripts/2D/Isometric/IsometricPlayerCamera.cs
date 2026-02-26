using UnityEngine;

/// <summary>
/// 플레이어를 부드럽게 추적하는 카메라.
/// Main Camera에 추가하고 Target에 플레이어를 할당.
/// </summary>
public class IsometricPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform t) => target = t;
}
