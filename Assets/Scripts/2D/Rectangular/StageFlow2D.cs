using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class StageFlow2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public static StageFlow2D Ins => _ins;
    public bool IsPlaying => _state == EStageState.Playing;
    public bool IsCleared => _state == EStageState.Cleared;
    public bool IsGameOver => _state == EStageState.GameOver;
    public EStageState GetState() => _state;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    // 스테이지가 가질 수 있는 상태
    public enum EStageState { None, Playing, Cleared, GameOver }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private static StageFlow2D _ins;
    private EStageState _state = EStageState.None;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void RestartStage()
    {
        SetState(EStageState.Playing);
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void SetState(EStageState next)
    {
        if (_state == next) {
            return;
        }

        EStageState prev = _state;
        _state = next;
        if (_logEnable) {
            De.Print($"스테이지 상태를 {prev}에서 {next}로 변경합니다.");
        }

        switch (_state) {
            case EStageState.Playing:
                break;
            case EStageState.Cleared:
                break;
            case EStageState.GameOver:
                break;
        }
    }

    public void NotifyGoalReached()
    {
        if (IsPlaying) {
            De.Print($"스테이지 클리어 이벤트가 발생했지만 현재 상태가 {_state}입니다.");
            return;
        }
        SetState(EStageState.Cleared);
        if (_logEnable) {
            De.Print("스테이지를 클리어했습니다.");
        }
    }

    public void NotifyPlayerDead()
    {
        if (IsPlaying) {
            De.Print($"플레이어 사망 이벤트가 발생했지만 현재 상태가 {_state}입니다.");
            return;
        }
        SetState(EStageState.GameOver);
        if (_logEnable) {
            De.Print("플레이어가 사망했습니다.");
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (_ins == null) {
            _ins = this;
            DontDestroyOnLoad(_ins);
            De.Print("StageFlow 싱글턴 인스턴스를 생성했습니다.");
        } else {
            Destroy(gameObject);
            De.Print("StageFlow 중복 인스턴스를 제거했습니다.");
        }
    }

    private void Update()
    {
        if (IsCleared || IsGameOver) {
            if (Input.GetKeyDown(_restartKey)) {
                RestartStage();
            }
        }
    }

    private void OnDestroy()
    {
        if (_ins == this) {
            _ins = null;
        }
    }
    #endregion
}
