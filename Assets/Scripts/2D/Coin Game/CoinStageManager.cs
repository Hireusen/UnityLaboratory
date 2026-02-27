using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// 스테이지의 상태를 관리합니다.
/// </summary>
public class CoinStageManager : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private CoinPlayerManager _player;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private RawImage _blackImage;

    [Header("사용자 정의 설정")]
    [SerializeField] private KeyCode _restartKey = KeyCode.R;
    [SerializeField] private int _targetCoin = 7;
    [SerializeField] private float _endAlpha = 0.84f;
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public static CoinStageManager Ins => _ins;
    public bool IsPlaying => _state == EStageState.Playing;
    public bool IsCleared => _state == EStageState.Cleared;
    public bool IsGameOver => _state == EStageState.GameOver;
    public int GetCoin() => _coin;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    private enum EStageState { None, Playing, Cleared, GameOver }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private static CoinStageManager _ins;
    private EStageState _state = EStageState.None;
    private int _coin = 0;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void UpdateCoinUI()
    {
        if (_coinText != null) {
            _coinText.text = $"Coins : {_coin} / {_targetCoin}";
        }
    }

    // 조건 알아서 검사하여 스테이지 재시작
    private void TryRestartStage()
    {
        if (!Input.GetKeyDown(_restartKey))
            return;
        if (IsGameOver || IsCleared) {
            SetGameStart();
            De.Print("게임을 재시작합니다.");
        }
        De.Print("리스타트 키를 눌렀지만 유효한 게임 상태가 아닙니다.", LogType.Warning);
        return;
    }

    // 게임 상태를 변경
    private void SetState(EStageState next)
    {
        if (_state == next) {
            return;
        }

        EStageState prev = _state;
        _state = next;
        if (_logEnable) {
            De.Print($"스테이지 상태를 {prev}에서 {next}로 변경합니다.");
        }
    }

    private void HandleGameStart()
    {
        SetState(EStageState.Playing);
        if (_logEnable) {
            De.Print("게임을 시작합니다.");
        }
        // 시작 상태 처리
        _player.gameObject.SetActive(true);
        _player.ReturnPlayer();
        Color color = _blackImage.color;
        color.a = 0f;
        _blackImage.color = color;
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void GameRestart() => HandleGameStart();
    public bool CheckVictory() => _coin >= _targetCoin;

    // 상태 설정 : 게임 시작
    public void SetGameStart()
    {
        if (IsPlaying) {
            De.Print($"게임 시작 이벤트가 발생했지만 이미 플레이 상태입니다.", LogType.Warning);
            return;
        }
        HandleGameStart();
    }

    // 상태 설정 : 게임 오버
    public void SetGameOver()
    {
        if (IsGameOver) {
            De.Print($"스테이지 패배 이벤트가 발생했지만 현재 상태가 {_state}입니다.", LogType.Warning);
            return;
        }
        SetState(EStageState.GameOver);
        if (_logEnable) {
            De.Print("게임을 실패했습니다.");
        }
        // 실패 상태 처리
        _player.gameObject.SetActive(false);
        Color color = _blackImage.color;
        color.a = _endAlpha;
        _blackImage.color = color;
    }

    // 상태 설정 : 게임 클리어
    public void SetGameClear()
    {
        if (IsPlaying) {
            De.Print($"스테이지 클리어 이벤트가 발생했지만 현재 상태가 {_state}입니다.", LogType.Warning);
            return;
        }
        SetState(EStageState.Cleared);
        if (_logEnable) {
            De.Print("스테이지를 클리어했습니다.");
        }
        // 성공 상태 처리
        _player.gameObject.SetActive(false);
        Color color = _blackImage.color;
        color.a = _endAlpha;
        _blackImage.color = color;
    }

    public void AddCoin()
    {
        _coin++;
        UpdateCoinUI();
        if(_coin >= _targetCoin) {
            SetGameClear();
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (_ins == null) {
            _ins = this;
            DontDestroyOnLoad(_ins);
            De.Print("CoinStageManager 싱글턴 인스턴스를 생성했습니다.");
        } else {
            Destroy(gameObject);
            De.Print("CoinStageManager 중복 인스턴스를 제거했습니다.", LogType.Warning);
        }

        if (De.IsNull(_player)
            || De.IsNull(_coinText)
            || De.IsNull(_blackImage)
        ) {
            De.Print("스테이지 매니저에 필수 요소가 등록되어있지 않습니다.", LogType.Assert);
            enabled = false;
        }
    }

    private void Update()
    {
        TryRestartStage();
    }

    private void OnDestroy()
    {
        if (_ins == this) {
            _ins = null;
        }
    }
    #endregion
}
