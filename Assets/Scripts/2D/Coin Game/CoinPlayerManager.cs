using UnityEngine;

/// <summary>
/// 플레이어 오브젝트에 부착하는 C# 스크립트입니다.
/// 플레이어 데이터를 저장하고 관리합니다.
/// </summary>
public class CoinPlayerManager : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D _col;
    [SerializeField] private CoinPlayerInput _input;
    [SerializeField] private CoinPlayerMover _mover;
    [SerializeField] private CoinPlayerRespawn _respawn;
    [SerializeField] private CoinPlayerTrigger _trigger;
    //[SerializeField] private CoinPlayerAnimator _animator;
    #endregion

    private CoinPlayerInfo _info;
    public void ReturnPlayer() => _respawn.ReturnPlayer();

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // 인스펙터 유효성 검사
    private bool Verification()
    {
        if (De.IsNull(_rb)
            || De.IsNull(_col)
            || De.IsNull(_input)
            || De.IsNull(_mover)
            || De.IsNull(_respawn)
            || De.IsNull(_trigger)
            //|| De.IsNull(_animator)
        ) {
            enabled = false;
            return false;
        }
        return true;
    }

    // 연결된 클래스를 포함하여 변수 초기화
    private void Initialized()
    {
        _info = new CoinPlayerInfo();
        _mover.Initialize(_info, transform, _col, _rb);
        _input.Initialize(_info);
        _respawn.Initialize(transform);
        //_animator.Initialize();
    }


    private void Awake()
    {
        if (!Verification())
            return;
        Initialized();
    }
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void UpdateState()
    {
        ECoinPlayerState next;
        // 상태 결정
        if (_info.isGrounded) {
            next = (_info.moveX != 0f) ? ECoinPlayerState.Move : ECoinPlayerState.Idle;
        } else {
            if(0f < _rb.velocity.y) {
                next = ECoinPlayerState.Jump;
            } else {
                next = ECoinPlayerState.Fall;
            }
        }
        // 상태 변경
        if (next != _info.state) {
            ChangeState(next);
        }
    }

    private void ChangeState(ECoinPlayerState next)
    {
        HandleExitState(_info.state);
        HandleEnterState(next);
        _info.state = next;
    }

    private void HandleEnterState(ECoinPlayerState state)
    {
        switch (state) {
            case ECoinPlayerState.Idle:
                Debug.Log("대기 상태로 전환합니다.");
                break;
            case ECoinPlayerState.Move:
                Debug.Log("이동 상태로 전환합니다.");
                break;
            case ECoinPlayerState.Jump:
                Debug.Log("점프 상태로 전환합니다.");
                break;
            case ECoinPlayerState.Fall:
                Debug.Log("낙하 상태로 전환합니다.");
                break;
        }
    }

    private void HandleExitState(ECoinPlayerState state)
    {
        switch (state) {
            case ECoinPlayerState.Idle:
                Debug.Log("대기 상태에서 벗어납니다.");
                break;
            case ECoinPlayerState.Move:
                Debug.Log("이동 상태에서 벗어납니다.");
                break;
            case ECoinPlayerState.Jump:
                Debug.Log("점프 상태에서 벗어납니다.");
                break;
            case ECoinPlayerState.Fall:
                Debug.Log("낙하 상태에서 벗어납니다.");
                break;
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Update()
    {
        _respawn.TryReturnPlayer();
        _input.UpdateMoveX();
        _mover.UpdateGrounded();
        _mover.UpdateRotation();
        UpdateState();
    }

    private void FixedUpdate()
    {
        // 물리 이동
        _mover.ApplyMoveX();
        if (_input.PressedJump()) {
            _mover.TryJump();
        }
        // 애니메이션
        //_animator.UpdateAnimation();
    }
    #endregion
}
