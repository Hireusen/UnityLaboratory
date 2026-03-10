using UnityEngine;

// fileName = 생성할 때 기본적으로 붙는 접두사
[CreateAssetMenu(fileName = "EnemyDataSO_", menuName = "2D3D/Data/Enemy Data (SO)")]
public class EnemyDataSO : ScriptableObject
{
    // 단일 데이터 처리인가? 여러 데이터 처리인가?
    // ㄴ 최초에 데이터를 만들어줄 때 배열 / 리스트를 고려할것인가? → 원본품으로 나온다.
    // ㄴ 또는 원본 데이터를 만들고 그 원본을 배열 / 리스트로 올릴 것인가? → 원본 자체를 배열로 → 더이상 확장의 여지가 없을 때
    //    ㄴ 연결되는 클래스를 만들어야 하기 때문에 좀 귀찮다. → 만약 내부 코드 복잡해지면 흐름 문제가 생겨 SO의 장점 퇴색
    // 하나의 프로젝트에서 두 가지 중 하나로 통일할 필요는 없다.

    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("기본 설정")]
    [SerializeField] private string _enemyName = "Enemy";
    [SerializeField] private string _enemyId = "enemy_01";

    [SerializeField] private int _hp = 10;
    [SerializeField] private float _moveSpeed = 2f;

    [SerializeField] private GameObject _prefab = null;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public string ItemName => _enemyName;
    public string ItemId => _enemyId;
    public int HP => _hp;
    public float MoveSpeed => _moveSpeed;
    public GameObject Prefab => _prefab;

    /// <summary>
    /// SO 데이터 유효성 검사
    /// </summary>
    public bool IsValid(out string reason)
    {
        if (string.IsNullOrEmpty(_enemyName))
        {
            reason = "적 이름이 공백입니다.";
            return false;
        }
        if (string.IsNullOrEmpty(_enemyId))
        {
            reason = "적 ID가 공백입니다.";
            return false;
        }
        reason = "";
        return true;
    }
    #endregion
}
