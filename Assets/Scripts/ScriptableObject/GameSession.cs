using UnityEngine;

/// <summary>
/// 씬 안에서만 쓰는 게임 상태를 관리하는 관리자 느낌으로 쓸 수 있는 클래스
/// </summary>
public class GameSession : MonoBehaviour
{
    // 점수 / HP / 획득 카운트 등을 관리하는 씬 세션
    // 점수 / HP / 아이템 획득수 등을 이걸 각각의 오브젝트가 들고 있으면 값이 어긋나기 쉽고 책임 처리가 애매한 경우가 많다.
    // 보통 중앙 객체가 상태를 보관하고 외부에서 변경 요청을 받으면 처리하는 구조가 관리 측면에선 좋다.
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private int _startHP = 10;

    [Header("디버그")]
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public static GameSession Ins { get; private set; }
    public int Score => _score;
    public int HP => _hp;
    public int ItemsCollected => _itemsCollected;
    public void Heal(int add)
    {
        _hp += Mathf.Max(0, add);
        if (_log)
        {
            De.Print($"현재 체력 = {_hp}");
        }
    }
    public void OnItemCollected(ItemDataSO data)
    {
        _itemsCollected++;
        if (data != null)
        {
            AddScore(data.Score);
            Heal(data.Heal);
            if (_log)
            {
                De.Print($"데이터 등록 완료 → {data}");
            }
        }
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private int _score;
    private int _hp;
    private int _itemsCollected;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void AddScore(int add)
    {
        _score += Mathf.Max(0, add);
        if (_log)
        {
            De.Print($"현재 점수 = {_score}");
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if(Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }
        Ins = this;
        // 초기화
        _hp = Mathf.Max(1, _startHP);
        _score = 0;
        _itemsCollected = 0;
    }
    private void OnDestroy()
    {
        if(Ins == this)
        {
            Ins = null;
        }
    }
    #endregion
}
