using UnityEngine;

// fileName = 생성할 때 기본적으로 붙는 접두사
[CreateAssetMenu(fileName = "ItemDataSO_", menuName = "2D3D/Data/Item Data (SO)")]
public class ItemDataSO : ScriptableObject
{

    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("기본 설정")]
    [SerializeField] private string _itemName = "Coin";
    [SerializeField] private string _itemId = "coin_01";

    [SerializeField] private int _score = 10;
    [SerializeField] private int _heal = 0;

    [SerializeField] private Sprite _icon = null;
    [SerializeField] private GameObject _prefab = null; // 확장할 때 유용하고 단일일 때는 일반적으로 내부로 내리는 게 더 편하다.
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public string ItemName => _itemName;
    public string ItemId => _itemId;
    public int Score => _score;
    public int Heal => _heal;
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;

    /// <summary>
    /// SO 데이터 유효성 검사
    /// </summary>
    public bool IsValid(out string reason)
    {
        if (string.IsNullOrEmpty(_itemName))
        {
            reason = "아이템 이름이 공백입니다.";
            return false;
        }
        if (string.IsNullOrEmpty(_itemId))
        {
            reason = "아이템 ID가 공백입니다.";
            return false;
        }
        reason = "";
        return true;
    }
    #endregion
}
