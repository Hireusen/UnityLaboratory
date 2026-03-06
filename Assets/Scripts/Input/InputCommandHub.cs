using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// 구독자를 모으고 Execute 하겠다.
/// </summary>
public class InputCommandHub : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private PauseInputHandler _pauseSystem;
    #endregion

    public void Execute(string key)
    {
        // 이것이 값이 있는지 확인함과 동시에 null까지 확인하는 방어 코드
        if (_commands.TryGetValue(key, out IInputCommand command) == false || command == null) {
            return;
        }
        // 실제 실행은 커맨드에게 위임
        command.Execute();
    }

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    // 흐름만 봐봅시다.
    private class CPauseToggleCommand : IInputCommand
    {
        private readonly PauseInputHandler _pause;

        public CPauseToggleCommand(PauseInputHandler pause)
        {
            // 리시버 저장
            _pause = pause;
        }

        public void Execute()
        {
            if(_pause == null) {
                return;
            }
            // 상태 토글을 진행 → 처리 책임은 핸들러에게 있다.
            _pause.SetPaused(PauseInputHandler.IsPaused);
        }
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    // 키 문자열 + 커맨드 객체를 매핑하는 테이블로 쓰려고 만든 변수
    // 입력이 늘어나면 여기만 늘어나는 구조 -> 확장이 되도 한 곳으로 모이는
    private readonly Dictionary<string, IInputCommand> _commands = new Dictionary<string, IInputCommand>();
    private bool _subscribed = false; // 중복 구독 확인
    private bool _needRetryBind = false; // 실패했을 때 폴링해서 시도해볼 변수
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void RegisterDefaultCommands()
    {
        // 키 → 명확하게 인지할 수 있는 이름으로 만들어주는 게 중요하다.
        // 여기에선 등록만 해서 사용하겠다.
        _commands["Pause.Toggle"] = new CPauseToggleCommand(_pauseSystem);
        // _commands["Player.Jump"] = new PlayerInputCommand() 이런식으로 확장. 다만 애는 값 타입이니까 별도 처리 필요
    }

    private bool BindDispatcherOnce()
    {
        if (_subscribed) {
            return true;
        }
        var ins = InputDisfactcher.Ins;
        if (ins == null) {
            return false;
        }
        BindInputEvents(ins);
        _subscribed = true;
        return true;
    }

    private bool UnBindDispatcher()
    {
        if (!_subscribed) {
            return true;
        }
        var ins = InputDisfactcher.Ins;
        if (ins == null) {
            return false;
        }
        UnBindInputEvents(ins);
        _subscribed = false;
        return true;
    }

    // 입력 이벤트에 대한 커맨드 키 매핑 슬롯
    private void BindInputEvents(InputDisfactcher disfactcher)
    {
        disfactcher.OnPause += HandlePause;
    }
    private void UnBindInputEvents(InputDisfactcher disfactcher)
    {
        disfactcher.OnPause -= HandlePause;
    }

    private void HandlePause()
    {
        Execute("Pause.Toggle");
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if(_pauseSystem == null) {
            _pauseSystem = FindFirstObjectByType<PauseInputHandler>();
        }
        RegisterDefaultCommands();
    }

    private void Update()
    {
        // 구독이 끝나서 업데이트 필요 없음
        if (!_needRetryBind) {
            return;
        }
        if (BindDispatcherOnce()) {
            _needRetryBind = false;
        }
    }

    private void OnEnable()
    {
        _needRetryBind = !BindDispatcherOnce();
        if (_needRetryBind) {
            enabled = true;
        }
    }

    private void OnDisable()
    {
        UnBindDispatcher();
    }

    private void OnDestroy()
    {
        UnBindDispatcher();
    }
    #endregion
}
