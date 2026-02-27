using UnityEngine;

/// <summary>
/// 플레이어 오브젝트에 부착하는 C# 스크립트입니다.
/// 이동과 점프를 적용합니다.
/// </summary>
public class CoinPlayerMover : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    // 이동 설정
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;
    // 바닥 판정
    [SerializeField] private LayerMask _groundLayer = default;
    [SerializeField] private float _groundUpper = 0.02f;
    [SerializeField] private float _groundStick = 0.2f;
    [SerializeField] private bool _showDebugRay = true;
    #endregion

    private CoinPlayerInfo _info;
    private Transform _tr;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _col;

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void Initialize(CoinPlayerInfo info, Transform tr, CapsuleCollider2D col, Rigidbody2D rb)
    {
        _info = info;
        _tr = tr;
        _col = col;
        _rb = rb;
    }

    /// <summary>
    /// 바닥 판정 여부를 업데이트합니다.
    /// </summary>
    public void UpdateGrounded()
    {
        // 변수 준비
        Vector2 origin = new Vector2(_col.bounds.center.x, _col.bounds.min.y + _groundUpper);
        int mask = _groundLayer.value;
        // 지상 여부 판정
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _groundStick, mask);
        _info.isGrounded = (hit.collider != null);
        // 발판 인식
        if (_info.isGrounded) {
            Transform tr = hit.transform;
            if (tr.CompareTag("MovingBlock")) {
                var foothold = _info.foothold;
                // 새로운 발판
                if (tr != _info.foothold) {
                    _info.foothold = tr;
                    _info.prevFootholdPos = _info.foothold.position;
                }
                // 발판의 이동과 동기화
                else {
                    // 이동량 계산
                    Vector2 curPos = foothold.position;
                    Vector2 delta = curPos - _info.prevFootholdPos;
                    // 적용 및 갱신
                    _rb.position += delta;
                    _info.prevFootholdPos = curPos;
                    //De.Print($"발판을 밟고 있습니다! (Delta = {delta})");
                }
            // 발판이 변화함
            } else {
                _info.foothold = null;
            }
        } else {
            _info.foothold = null;
        }
        // 디버그 레이
        if (_showDebugRay) {
            Debug.DrawRay(origin, Vector2.down * _groundStick, _info.isGrounded ? Color.green : Color.red);
        }
    }

    /// <summary>
    /// 물리 이동량을 적용하고 상태를 업데이트합니다.
    /// </summary>
    public void ApplyMoveX()
    {
        // 변수 준비
        Vector2 v = _rb.velocity;
        float moveX = _info.moveX;
        // 이동 적용
        v.x = moveX * _moveSpeed;
        _rb.velocity = v;
        // 방향 업데이트
        if (0f < moveX) {
            _info.lookToRight = true;
        } else if (0f > moveX) {
            _info.lookToRight = false;
        }
    }

    /// <summary>
    /// 바라보는 방향에 따라 Y축 회전을 변경합니다.
    /// </summary>
    public void UpdateRotation()
    {
        float yRot = _info.lookToRight ? 0f : 180f;
        _tr.rotation = Quaternion.Euler(0f, yRot, 0f);
    }

    /// <summary>
    /// 물리 점프량을 적용합니다.
    /// </summary>
    public void TryJump()
    {
        if(_info.IsAir)
            return;
        // y 리셋
        Vector2 v = _rb.velocity;
        v.y = 0f;
        _rb.velocity = v;
        // 점프
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
    #endregion
}
