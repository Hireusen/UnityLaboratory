using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 타겟을 바라보며 쫓아갑니다.
/// </summary>
public class UseMoveTowards : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    [SerializeField] private Transform _target;
    private const float MOVE_SPEED = 26.6f;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _target = URand.GetTransform();
    }

    private void Update()
    {
        Vector3 myPos = transform.position;
        Vector3 targetPos = _target.position;
        // 이동
        transform.position = Vector3.MoveTowards(myPos, _target.position, MOVE_SPEED * Time.deltaTime);
        transform.LookAt(targetPos);
        // 도착
        float sqrDistance = (targetPos - myPos).sqrMagnitude;
        if (sqrDistance < 0.1f) {
            _target = URand.GetTransform();
        }
    }
    #endregion
}
