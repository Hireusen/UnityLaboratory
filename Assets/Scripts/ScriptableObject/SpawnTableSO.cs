using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnTableSO_", menuName = "2D3D/Data/Spawn Table (SO)")]
public class SpawnTableSO : ScriptableObject
{
    // 스폰 규칙을 코드가 아니라 에셋으로 관리한다.
    // 스포너는 테이블에서 뽑고 스폰한다. (단순화)
    // 실제 개발에서 원래 스폰 규칙이나 게임 룰 등은 대부분 외부에서 들어오는 경우가 많다.
    // 기획자 → 파일 → IO → SO와 IO가 유니티에서 궁합이 그다지 좋지 않다. (SO를 쓸 때는 왠만하면 IO를 붙이지 않을 것)
    // 만약 부득이하게 SO와 IO를 같이 쓰게 된다면 풀어버릴 것을 권장 (SO는 SO만, IO는 IO만)
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("기본 설정")]
    [SerializeField, Min(0f)] private float _minInterval = 0.8f; // 랜덤 간격 뽑기
    [SerializeField, Min(0f)] private float _maxInterval = 1.6f;
    [SerializeField] private List<SpawnEntry> _entries = new List<SpawnEntry>(); // 후보 목록

    [SerializeField] private int _score = 10;
    [SerializeField] private int _heal = 0;

    [SerializeField] private Sprite _icon = null;
    [SerializeField] private GameObject _prefab = null;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public float MaxInterval => _maxInterval;
    public float MinInterval => _minInterval;
    public IReadOnlyList<SpawnEntry> Entries => _entries;

    public float GetNextInterval()
    {
        float min = Mathf.Max(0.01f, _minInterval);
        float max = Mathf.Max(min, _maxInterval);
        return Random.Range(min, max);
    }

    public bool TryPick(out SpawnEntry picked)
    {
        picked = null;
        int length = _entries.Count;
        // 방어 코드
        if (_entries == null || length == 0)
        {
            return false;
        }
        // 가중치 합산
        float total = 0f;
        for (int i = 0; i < length; ++i)
        {
            var e = _entries[i];
            if (e == null) continue;
            if (e.prefab == null) continue;
            if (e.weight <= 0f) continue;
            total += e.weight;
        }
        if (total <= 0f)
        {
            return false;
        }
        // 랜덤 픽
        float rand = Random.Range(0f, total);
        float acc = 0f; // 누적합 변수
        for (int i = 0; i < length; ++i)
        {
            var e = _entries[i];
            if (e == null) continue;
            if (e.prefab == null) continue;
            if (e.weight <= 0f) continue;
            acc += e.weight;
            if (rand <= acc)
            {
                picked = e;
                return true;
            }
        }
        // 과연 여기에 올까?
        picked = _entries[Random.Range(0, length)];
        return true;
    }
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    public enum EEntryType
    {
        Item,
        Enemy
    }
    [System.Serializable]
    public class SpawnEntry
    {
        [Header("종류")]
        public EEntryType type = EEntryType.Item; // 종류
        public ItemDataSO itemData = null; // 연결 데이터
        public EnemyDataSO enemyData = null;
        public GameObject prefab = null; // 실제 생성될 오브젝트
        [Min(0f)] public float weight = 1f; // 가중치 (스폰 비율) (A : B = 1 : 2 라면 B가 2배 더 많이 스폰)
    }
    #endregion
}
