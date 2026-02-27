using UnityEngine;

/// <summary>
/// 플래피 오브젝트에 부착하는 C# 스크립트입니다.
/// 입력은 자동으로 받도록 할까
/// </summary>
public class FlappyPlayerController2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _flappy;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _flapForce = 12f;
    [SerializeField] private float _maxUpVel = 8.5f;
    [SerializeField] private float _maxDownVel = -8.5f;
    // 회전 보여주기
    [SerializeField] private float _upAngle = 25f;
    [SerializeField] private float _downAngle = -80f;
    [SerializeField] private float _sharpness = 12f;
    // 태그
    [SerializeField] private string _pipeTag = "Pipe";
    [SerializeField] private string _groundTag = "Ground";
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public bool IsDead => _isDead;
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Rigidbody2D _rb;
    private bool _isDead;
    private float _desiredAngle;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void ClampVelocity()
    {
        Vector2 v = _rb.velocity;
        v.y = Mathf.Clamp(v.y, _maxDownVel, _maxUpVel);
        _rb.velocity = v;
    }
    private void CalcDesiredAngle()
    {
        float vel = _rb.velocity.y;
        // 위로
        if (vel > 0.01) {
            _desiredAngle = _upAngle;
            
        }
        // 아래로
        else if (vel < 0) {
            // value가 a~b 사이에서 어디쯤인지 0~1로 정규화한다.
            // vel = 0, t = 0 (거의 수평)
            // vel = -10, t = 1 (거의 최대 하강 각도)
            float t = Mathf.InverseLerp(0f, -10f, vel);
            _desiredAngle = Mathf.Lerp(0f, _downAngle, t);
        }
    }

    private void TickRotate()
    {
        float curZ = transform.eulerAngles.z;
        // 누가 -180 ~ 180으로 받더라
        if(curZ > 180f) {
            curZ -= 360f;
        }
        float t = Mathf.Exp(-_sharpness * Time.deltaTime);
        float nextZ = Mathf.Lerp(curZ, _desiredAngle, t);
        // 2D에서 오일러인가? 쿼터니언인가?
        // 트랜스폼 쓸거면 어차피 쿼터니언으로 저장은 해야하잖아
        // 3D는 당연히 쿼터니언이고, 2D여도 엔진 내부에서 어차피 쿼터니언쓰는데 의미 있을까요?
        // 의미 있습니다. 변환 과정과 속도를 고려합시다.
        transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
    }

    // 플래피 장르의 점프는 순간 힘이 가해지는 느낌이 강함
    private void Flap()
    {
        // 초기화
        Vector2 v = _rb.velocity;
        v.y = 0f;
        _rb.velocity = v;
        // 힘 가하기
        _rb.AddForce(Vector2.up * _flapForce, ForceMode2D.Impulse);
        // 회전 적용
        _desiredAngle = _upAngle;
    }

    // 플레이어 사망 확정 → 입력 / 충돌 / 중복 사망 잠금
    private void Die()
    {
        if (_isDead)
            return;
        // 이벤트
        _isDead = true;
        // FlappyGameManager2D.Ins.NotifyPlayerDead();
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) {
            De.Print("리지드 바디가 없음..");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (_rb == null)
            return;
        if (_isDead)
            return;
        // 점프
        if (Input.GetKeyDown(KeyCode.Space)) {
            Flap();
        }
        // 회전
        CalcDesiredAngle();
        TickRotate();
        // 속도 제한
        ClampVelocity();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
    #endregion
}
