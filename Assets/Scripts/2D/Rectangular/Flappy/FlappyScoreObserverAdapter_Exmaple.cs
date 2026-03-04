using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyScoreObserverAdapter_Exmaple : MonoBehaviour, IScoreAdapterObserver
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private FlappyScoreObserverAdapter _adapter;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void OnScoreChanged(int newScore)
    {
        // 처리 로직 작성
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if(_adapter == null) {
            _adapter = FindFirstObjectByType<FlappyScoreObserverAdapter>();
        }
    }

    private void OnEnable()
    {
        if (_adapter == null)
            return;
        _adapter.Attach(this);
    }

    private void OnDisable()
    {
        if (_adapter == null)
            return;
        _adapter.Detach(this);
    }
    #endregion
}
