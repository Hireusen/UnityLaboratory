using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class ScoreTriggerBetweenPipes : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private bool _scored;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 일단은 만들어두는데 이걸 쓸 일이 있을까
    public void ResetScoreTrigger() {
        _scored = false;
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어만 점수 대상
        if (!other.CompareTag(_playerTag))
            return;
        // 중복 실행 방지
        if (_onlyOnce && _scored)
            return;
        _scored = true;
        // 정상 작동
        if(FlappyScoreManager2D.Ins != null) {
            // 그냥 매직넘버 넣었는데, 나중에 제대로 만들땐 당연히 변수여야 함
            FlappyScoreManager2D.Ins.AddScore(1);
        }
        // 점수 매니저 어디감?
        else {
            De.Print("매니저님 어디가셨어요", LogType.Assert);
        }
    }
    #endregion
}
