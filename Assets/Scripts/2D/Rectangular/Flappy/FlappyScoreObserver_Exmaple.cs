using UnityEngine;

// 옵저버 인터페이스 써보자~
public class FlappyScoreObserver_Exmaple : MonoBehaviour, IScoreObserver
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private ScoreSubject_Classic _subject;

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _autoFind = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void OnScoreChanged(ScoreSubject_Classic subject, int newScore)
    {

    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        // 흐름을 가지고 있는 구조이기 때문에 중간에 1개라도 누락시키면 바로 에러
        if(_subject == null && _autoFind) {
            _subject = FindFirstObjectByType<ScoreSubject_Classic>();
        }
    }

    private void OnEnable()
    {
        if (_subject == null)
            return;
        _subject.Attach(this);
        OnScoreChanged(_subject, _subject.GetScore());
    }

    private void OnDisable()
    {
        if (_subject == null)
            return;
        _subject.Detach(this);
    }
    #endregion
}
