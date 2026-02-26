using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// 인덱서, 스테이트 패턴이 적용된 2D 플레이어 무브 스크립트입니다.
/// </summary>
public class PlayerMove2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private LayerMask _groundLayer = default;
    [SerializeField] private float _groundRayLength = 0.2f;
    [SerializeField] private bool _showDebugRay = true;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    private enum EMoveState : byte { Idle, Move, Air }
    private enum EAxis : byte { Horizontal, Vertical }
    private enum EButton : byte { JumpDown }

    // 네이티브 언어의 전형적인 키 구현 방식
    private class InputMap
    {
        public float this[EAxis axis]
        {
            get {
                switch (axis) {
                    case EAxis.Horizontal:
                        return Input.GetAxisRaw("Horizontal");
                    case EAxis.Vertical:
                        return Input.GetAxisRaw("Vertical");
                }
                return 0;
            }
        }

        public bool this[EButton btn]
        {
            get {
                switch (btn) {
                    case EButton.JumpDown:
                        return Input.GetKeyDown(KeyCode.Space);
                }
                return false;
            }
        }
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Rigidbody2D _rb;
    private Collider2D _col;
    private float _moveX;
    private bool _jumpPressed;
    private bool _isGrounded;
    private EMoveState _state = EMoveState.Idle;
    private readonly InputMap _input = new InputMap();
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void UpdateInput()
    {
        _moveX = _input[EAxis.Horizontal];
        if (_input[EButton.JumpDown]) {
            _jumpPressed = true;
        }
    }

    // 2D에서 바닥 체크는 언제나 레이를 활용하자.
    // 무조건은 아니고, 
    private void UpdateIsGround()
    {
        // 지면과 겹쳐 인식 못하는 버그 방지 : 유니티 2D 관례값 : 0.02f
        Vector2 origin = new Vector2(_col.bounds.center.x, _col.bounds.min.y + 0.02f);
        // 마스크가 0이면 모든 레이어가 대상
        int mask = (_groundLayer.value == 0) ? Physics2D.AllLayers : _groundLayer.value;
        // 레이 아래로 쏘기
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _groundRayLength, mask);
        _isGrounded = (hit.collider != null);
        // 디버그 레이
        if (_showDebugRay) {
            Debug.DrawRay(origin, Vector2.down * _groundRayLength, _isGrounded ? Color.green : Color.red);
        }
    }

    private void ApplyMove()
    {
        // 속도 결정
        Vector2 v = _rb.velocity;
        v.x = _moveX * _moveSpeed;
        _rb.velocity = v;
    }

    private void TryJump()
    {
        // 방어 코드
        if (!_jumpPressed)
            return;
        _jumpPressed = false;
        if (_state == EMoveState.Air) {
            return;
        }
        // 점프 직전 y속도 초기화 → 캐릭터가 빠릿하게 뛰어오르지 않고 씹히는 느낌 방지
        Vector2 v = _rb.velocity;
        v.y = 0f;
        _rb.velocity = v;
        // 점프
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        De.Print("점프했다!");
    }

    // 상황에 따라 현재 상태 결정
    private EMoveState UpdateState()
    {
        if (_isGrounded) {
            float h = _input[EAxis.Horizontal];
            if (h != 0) {
                if (!(_state == EMoveState.Move)) {
                    ChangeState(EMoveState.Move);
                }
            } else {
                if (!(_state == EMoveState.Idle)) {
                    ChangeState(EMoveState.Idle);
                }
            }
        } else {
            if (!(_state == EMoveState.Air)) {
                ChangeState(EMoveState.Air);
            }
        }
        return _state;
    }

    private void ChangeState(EMoveState state)
    {
        if (_state == state) {
            return;
        }
        HandleExitState(_state);
        HandleEnterState(state);
        _state = state;
    }

    // 상태에 들어가는 순간 1회만 실행되는 처리
    private void HandleEnterState(EMoveState state)
    {
        switch (state) {
            case EMoveState.Idle:
                De.Print("대기 상태로 전환합니다.");
                break;
            case EMoveState.Move:
                De.Print("이동 상태로 전환합니다.");
                break;
            case EMoveState.Air:
                De.Print("공중 상태로 전환합니다.");
                break;
        }
    }

    // 상태에 들어가는 순간 1회만 실행되는 처리
    private void HandleExitState(EMoveState state)
    {
        switch (state) {
            case EMoveState.Idle:
                De.Print("대기 상태에서 벗어납니다.");
                break;
            case EMoveState.Move:
                De.Print("이동 상태에서 벗어납니다.");
                break;
            case EMoveState.Air:
                De.Print("공중 상태에서 벗어납니다.");
                break;
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) {
            enabled = false;
            De.Print("2D 플레이어에 리지드 바디가 없습니다.");
            return;
        }
        _col = GetComponent<Collider2D>();
        if (_col == null) {
            enabled = false;
            De.Print("2D 플레이어에 콜라이더가 없습니다.");
            return;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (De.IsNull(_rb) || De.IsNull(_col)) {
            enabled = false;
        }
        // 상태 업데이트
        UpdateInput();
        UpdateIsGround();
        UpdateState();
    }

    private void FixedUpdate()
    {
        if (De.IsNull(_rb) || De.IsNull(_col)) {
            enabled = false;
        }
        // 내부 변수에 따라 이동 및 점프
        ApplyMove();
        TryJump();
    }
    #endregion
}
