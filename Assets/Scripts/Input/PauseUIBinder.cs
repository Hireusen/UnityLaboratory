using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class PauseUIBinder : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    /*[Header("필수 요소 등록")]
    [SerializeField] private PauseInputHandler _pause;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    // 쉽게 확인하기 위해 넣어둔 것 (씬 시작 시 false로 초기화 필요)
    public static bool IsPaused { get; private set; } = false;
    public event System.Action<bool> OnPausedChanged;
    private Coroutine _bindCo;
    private bool _subscribed = false;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private IEnumerator CoBind()
    {

    }
    private void OnClickResume()
    {

    }

    private void HandlePausedChanged(bool paused)
    {
        if(_pausePanel == null) {
            return;
        }
        _pausePanel.SetActive(paused);
    }

    private void OnClickResume()
    {
        if(_pause == null) {
            return;
        }
        _pausePanel.SetActive(false);
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (_pause == null) {
            _pause = FindAnyObjectByType<PauseInputHandler>();
        }
        if (_pausePanel != null) {
            _pausePanel.SetActive(false);
        }
        if (_resumeButton != null) {
            _resumeButton.onClick.AddListener(OnClickResume);
        }
    }

    private void OnEnable()
    {
        if (_pause == null) {
            return;
        }
        _bindCo = StartCoroutine(CoBind());

        // 상태가 바뀔 때마다 이벤트가 호출된다.
        _pause.OnPausedChanged += HandlePausedChanged;
        // 현재 상태를 한번 강제로 반영해서 동기화시킨다.
        HandlePausedChanged(PauseInputHandler.IsPaused);
    }

    private void OnDisable()
    {
        if(_pause == null) {
            return;
        }
        _pause.OnPausedChanged -= HandlePausedChanged;

        // 해제
        var input = InputDisfactcher.Ins;
        if (_subscribed && input != null) {
            
        }
        _subscribed = false;
    }*/
    #endregion
}
