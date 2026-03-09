using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class PauseFlowSequence : MonoBehaviour
{
    // 유니테스크
    // 이벤트 (핸들러 / 디스패처)로 상태가 바뀌는 순간을 받아서 다음 작업을 수행한다.
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private PauseInputHandler _pause;
    [SerializeField] private AsyncSequenceRunner _runner;

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _applyTimeScale = true;
    [SerializeField] private float _pauseTimeScale = 0f; // timeScale에 대입할 값
    [SerializeField] private float _resumeDelay = 0f; // 다시 재생할 때 살짝 연출용
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────

    private void Awake()
    {
        // 자동 탐색 편하죠? 인스펙터 연결이 베스트입니다. Reset에 쓰던가
        if (_pause == null)
        {
            _pause = FindFirstObjectByType<PauseInputHandler>();
        }
        if (_runner == null)
        {
            _runner = GetComponent<AsyncSequenceRunner>();
            if (_runner == null)
            {
                // 중요하므로..
                _runner = gameObject.AddComponent<AsyncSequenceRunner>();
            }
        }

    }

    private void OnEnable()
    {
        if (_pause == null)
        {
            return;
        }
        // 구독
        _pause.OnPausedChanged += HandlePausedChanged;
        // 동기화 반영
        HandlePausedChanged(PauseInputHandler.IsPaused);
    }

    private void OnDisable()
    {
        if(_pause != null)
        {
            _pause.OnPausedChanged -= HandlePausedChanged;
        }
    }

    private void HandlePausedChanged(bool paused)
    {
        // 람다를 사용하기 좋은 이유
        // Pause 자체가 On일때 할 일과 Off일때 할 일을 한번에 정의할 수 있기 때문에 람다가 유리하다.
        _runner.RunLatest(async ct =>
        {
            if (paused)
            {
                if (_applyTimeScale) // 일시정지
                {
                    Time.timeScale = _pauseTimeScale;
                }
                De.Print($"일시정지 플로우 : On (후처리 완료)");
            } else // 
            {
                if(_resumeDelay > 0.0001f)
                {
                    await UniTask.Delay(
                        // 초를 시간 길이 객체로 바꿔주는 함수
                        // 즉 몇 초라는 숫자를 컴퓨터가 다루기 좋은 시간 간격으로 바꿔준다.
                        // Delay는 밀리세컨 말고도 TimeSpan을 받도록 오버로딩되어 있다. (성능을 더 위한다면 써라)
                        System.TimeSpan.FromSeconds(_resumeDelay),
                        DelayType.UnscaledDeltaTime,
                        PlayerLoopTiming.Update,
                        ct);
                }
                if (_applyTimeScale)
                {
                    Time.timeScale = 1f;
                }
                De.Print("일시정지 플로우 : Off (후처리 완료)");
            }

        });
    }
    #endregion
}
