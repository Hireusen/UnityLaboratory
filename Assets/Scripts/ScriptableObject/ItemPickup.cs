using UnityEngine;

/// <summary>
/// 아이템에 붙이는 컴포넌트
/// </summary>
[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("참조 연결")]
    [SerializeField] private ItemDataSO _data = null;

    [Header("사용자 정의 설정")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;

    [Header("디버그")]
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public void SetData(ItemDataSO data)
    {
        _data = data;
    }
    #endregion

    private bool _picked = false;

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Reset()
    {
        var col = GetComponent<Collider>();
        if(col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 일회성 획득
        if (_onlyOnce && _picked)
        {
            return;
        }
        // 태그 유효성 검사
        if (string.IsNullOrEmpty(_playerTag) || !other.CompareTag(_playerTag))
        {
            return;
        }
        // 아이템 주웠다
        _picked = true;
        if(GameSession.Ins != null)
        {
            GameSession.Ins.OnItemCollected(_data);
        }
        // 어째서 여기에 왔지?
        else if(_log)
        {
            De.Print("게임 세션 어디갔어!");
        }
        if (_log)
        {
            string name = (_data != null) ? _data.ItemName : "Null Data";
            De.Print(name);
        }
        Destroy(gameObject);
        De.Print("삭제 완료");
    }
    #endregion
}
