using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 정사각형을 그리며 이동합니다.
/// </summary>
public class UseTransformDirection : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const float MOVE_DELAY = 0.5f;
    private const float MOVE_SPEED = 10f;
    private float _nextTurn;
    private Vector3 _dir;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _dir = Vector3.forward;
        _nextTurn = Time.time;
    }

    private void Moving()
    {
        Vector3 movement = _dir * MOVE_SPEED * Time.deltaTime;
        transform.position += movement;
    }

    private void ChangeDir()
    {
        if (_dir == Vector3.forward) {
            _dir = Vector3.right;

        } else if (_dir == Vector3.right) {
            _dir = Vector3.back;

        } else if (_dir == Vector3.back) {
            _dir = Vector3.left;

        } else if (_dir == Vector3.left) {
            _dir = Vector3.up;

        } else if (_dir == Vector3.up) {
            _dir = Vector3.down;

        } else if (_dir == Vector3.down) {
            _dir = Vector3.forward;
        }
    }

    private void Update()
    {
        // 방향 변경
        if(_nextTurn <= Time.time) {
            _nextTurn += MOVE_DELAY;
            ChangeDir();
        }
        // 이동
        Moving();
    }
    #endregion
}
