using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 자신의 정면을 왕복합니다.
/// </summary>
public class UseTransformTranslate : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const float TURN_DELAY = 1.5f;
    private const float MOVE_SPEED = 4.2f;
    private float _nextTurn;
    private Vector3 _dir;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _dir = Vector3.forward;
        _nextTurn = Time.time;
    }

    // Translate = position + (rotation * translate)
    // 결론 : 자신의 방향을 고려해서 이동함.
    private void Update()
    {
        // 방향 전환
        if(_nextTurn < Time.time) {
            _nextTurn += TURN_DELAY;
            _dir = _dir != Vector3.forward ? Vector3.forward : Vector3.back;
        }
        // 이동
        transform.Translate(_dir * MOVE_SPEED * Time.deltaTime, Space.Self);
    }
    #endregion
}
