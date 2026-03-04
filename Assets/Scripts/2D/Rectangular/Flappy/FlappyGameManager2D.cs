using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyGameManager2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private EFlappyState _startState = EFlappyState.Playing; // 레디 구현 예정이 없어서..
    [SerializeField] private FlappyPlayerController2D _controller; // 플레이어 의존도가 심한 스크립트
    [SerializeField] private FlappyScoreManager2D _score;
    [SerializeField] private Behaviour[] _disableOnGameOver; // 게임의 각종 스크립트들을 배열로 한번에 관리하겠다.
    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public static FlappyGameManager2D Ins { get; private set; }
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    public enum EFlappyState
    {
        Ready = 0, Playing = 1, GameOver = 2
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private EFlappyState _state = EFlappyState.Ready;
    private bool IsPlaying => _state == EFlappyState.Playing;
    private bool IsReady => _state == EFlappyState.Ready;
    private bool IsGameover => _state == EFlappyState.GameOver;
    public event System.Action<EFlappyState, EFlappyState> StateChanged;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    public bool IsPlayerDead()
    {
        if(_controller == null) {
            return false;
        }
        return _controller.IsDead;
    }

    public void NotifyPlayerDead()
    {
        
    }

    public void RestartScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    private void SetState(EFlappyState next)
    {
        if(_state == next) {
            return;
        }
        // 상태 설정
        EFlappyState prev = _state;
        _state = next;
        De.Print($"{prev} → {next}");
        //
        switch (_state) {
            case EFlappyState.Ready:
                // 입력 대기, UI, 로드 준비 등
                break;
            case EFlappyState.Playing:
                if(_score != null) {
                    _score.ResetScore();
                }
                EnableAll(true);
                break;
            case EFlappyState.GameOver:
                EnableAll(false);
                break;
        }
    }

    private void EnableAll(bool enable)
    {
        if(_disableOnGameOver == null) {
            return;
        }
        for (int i = 0; i < _disableOnGameOver.Length; ++i) {
            if(_disableOnGameOver[i] == null) {
                continue;
            }
            _disableOnGameOver[i].enabled = enable;
        }
    }

    // 단일 진입점으론 나를 쓸 수 밖에 없는 구조를 만들어 놨다.
    //

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (Ins != null && Ins != this) {
            Destroy(gameObject);
        } else {
            Ins = this;
        }
    }

    private void OnDestroy()
    {
        if (Ins == this) {
            Ins = null;
        }
    }

    private void Start()
    {
        if (_controller == null) {
            _controller = FindFirstObjectByType<FlappyPlayerController2D>();
        }
        if (_score == null) {
            _score = FindFirstObjectByType<FlappyScoreManager2D>();
        }
        // 상태 세팅
        SetState(_startState);
    }

    private void Update()
    {
        if (IsPlaying) {
            if (IsPlayerDead()) {
                SetState(EFlappyState.GameOver);
            }
        }
        else if (IsGameover) {
            if (Input.GetKeyDown(_restartKey)) {
                RestartScene();
            }
        }
    }
    #endregion
}
