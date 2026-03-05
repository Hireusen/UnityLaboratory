using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyCameraShake2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _cameraTr;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.2f;
    [SerializeField] private float _frequency = 40f; // 갱신 속도 (값이 높을수록 더 많이 흔들린다.)
    [SerializeField] private bool _useUnscaledtime = false;
    [SerializeField] private bool _useLog = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    public static FlappyCameraShake2D Ins {get; private set;}
    private Vector3 _originPos; // 기준점
    private bool _isShaking = false; // 외부에서 진동 중인지 참고 용도
    private float _remain = 0f; // 남은 시간
    private float _strength = 0f; // 흔들림 강도
    private float _nextSampleTime = 0f; // 진동 하나하나의 간격 (Time.time 기준)
    private Vector3 _currentOffset = Vector3.zero; // 현재 적용 중인 오프셋
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public static void PlayGameOverShake()
    {
        if(Ins == null) {
            return;
        }
        Ins.Play(Ins._shakeDuration, Ins._shakeStrength);
    }

    public void Play(float duration, float strength)
    {
        if(_cameraTr == null) {
            De.Print("플래피 카메라 게임 쉐이크 2D에서 카메라가 없습니다.");
            return;
        }
        _isShaking = true;
        _remain = Mathf.Max(_remain, Mathf.Max(0.01f, duration));
        _strength = Mathf.Max(_remain, Mathf.Max(0f, strength));

        float now = _useUnscaledtime ? Time.unscaledTime : Time.time;
        _nextSampleTime = now;
        _originPos = _cameraTr.position;
    }

    public void StopShake()
    {
        if(_cameraTr != null) {
            _cameraTr.position = _originPos;
        }
        _isShaking = false;
        _remain = 0f;
        _strength = 0f;
        _currentOffset = Vector3.zero;
        // 변수들 로그 찍는 성실함이 있으면 좋겠죠
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void LateUpdate()
    {
        if (_cameraTr == null) {
            return;
        }
        float dt = _useUnscaledtime ? Time.unscaledDeltaTime : Time.deltaTime;
        float now = _useUnscaledtime ? Time.unscaledTime : Time.time;
        // 현재 쉐이킹 안하는 중 → 기준점 계속 갱신
        if (!_isShaking) {
            _originPos = _cameraTr.position;
            return;
        }
        // 쿨타임 처리 및 진동시켜서 나온 오프셋 갱신
        _remain -= dt;
        if(now >= _nextSampleTime) {
            // 이건 0 초과 1 이하로 유지하겠다는 것
            // 0으로 나누면 예외 발생합니다. 이게 효율적이라고 하신 듯
            float interval = 1f / Mathf.Max(1f, _frequency);
            _nextSampleTime = now + interval;
            // 랜덤 구하기
            Vector2 rand = Random.insideUnitCircle * _strength;
            _currentOffset = new Vector3(rand.x, rand.y, 0f);
        }
        // 적용
        _cameraTr.position = _originPos + _currentOffset;
        if(_remain <= 0f) {
            StopShake();
        }
    }

    private void Awake()
    {
        // 싱글턴
        if(Ins != null || Ins != this) {
            Destroy(this);
            return;
        }
        Ins = this;
        // 카메라 참조 연결
        if(_cameraTr == null) {
            if(Camera.main != null) {
                _cameraTr = Camera.main.transform;
            } else {
                _cameraTr = transform; // 자기 자신
            }
        }
        // 
        _originPos = _cameraTr.position;
    }
    private void OnDestroy()
    {
        if(Ins == this) {
            Ins = null;
        }
    }
    #endregion
}
