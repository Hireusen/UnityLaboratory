using UnityEngine;
using System.Collections;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class PauseInputHandler : MonoBehaviour
{
    [Header("사용자 정의 설정")]
    [SerializeField] private bool _pauseByTimeScale = true; // 업데이트 / 물리 / 애니메이션
    [SerializeField] private float _pausedTimeScale = 0f;   // 값에 따라 연출 효과 낼 수 있음
    [SerializeField] private bool _controlCursor = true;    // 일시정지일 때 제어 용도

    // 쉽게 확인하기 위해 넣어둔 것 (씬 시작 시 false로 초기화 필요)
    public static bool IsPaused { get; private set; } = false;
    public event System.Action<bool> OnPausedChanged;
    private Coroutine _bindCo;
    private bool _subscribed = false;

    private void HandlePause()
    {
        SetPaused(!IsPaused);
    }

    private IEnumerator CoBind()
    {
        while (InputDisfactcher.Ins == null) {
            yield return null;
        }
        if (_subscribed) {
            yield break;
        }
        InputDisfactcher.Ins.OnPause += HandlePause;
    }

    public void SetPaused(bool paused)
    {
        // 같다면 거르기
        if (IsPaused == paused) {
            return;
        }
        // 일시정지 적용
        IsPaused = paused;
        if (_pauseByTimeScale) {
            Time.timeScale = IsPaused ? _pausedTimeScale : 1f;
        }
        if (_controlCursor) {
            // 조작을 위해 커서를 살린다.
            if (IsPaused) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // 커서를 숨긴다.
            else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        // 알림
        OnPausedChanged?.Invoke(IsPaused);
        De.Print($"일시정지 상태를 변경합니다. ({IsPaused})");
    }

    private void OnEnable()
    {
        _bindCo = StartCoroutine(CoBind());
    }

    private void OnDisable()
    {
        if (_bindCo != null) {
            StopCoroutine(_bindCo);
            _bindCo = null;
        }
        // 해제
        var input = InputDisfactcher.Ins;
        if (_subscribed && input != null) {
            input.OnPause -= HandlePause;
        }
        _subscribed = false;
    }
}
