using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스포너에 붙이세요.
/// </summary>
public class DataSpawner : MonoBehaviour
{

    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("참조 연결")]
    [SerializeField] private GameDataRegistrySO _registry = null;
    [SerializeField] private List<Transform> _spawnPoint = new List<Transform>();

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _spawnOnStart = true; // 게임 시작 시 바로 하나 스폰?
    [SerializeField] private int _maxAlive = 20;
    [SerializeField] private float _spawnCooldown = 2.5f;

    [Header("디버그")]
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private readonly List<GameObject> _alive = new List<GameObject>();
    private float _nextSpawnTime; // 다음 스폰 예정 시간
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private bool TrySpawnOne()
    {
        // 랜덤 드롭할 아이템 가져오기
        var table = _registry.SpawnTable;
        if (!table.TryPick(out var picked) || picked == null || picked.prefab == null)
        {
            return false;
        }
        // 스폰 장소 결정
        Vector3 pos = GetSpawnPosition();
        Quaternion rot = Quaternion.identity;
        // 생성
        GameObject go = Instantiate(picked.prefab, pos, rot);
        _alive.Add(go);
        // 데이터 주입
        // 연결고리가 되는 코드이기 때문에 어찌보면 오늘 수업에서 가장 중요한 부분
        if (picked.type == SpawnTableSO.EEntryType.Item)
        {
            var pickup = go.GetComponent<ItemPickup>();
            if (pickup != null && picked.itemData != null)
            {
                pickup.SetData(picked.itemData);
            }
        }
        return true;
    }

    private Vector3 GetBaseSpawnPosition()
    {
        Vector3 basePos = transform.position;
        basePos.x += Random.Range(-4f, 4f);
        basePos.y += Random.Range(-4f, 4f);
        return basePos;
    }
    // 스폰 포인트가 있다면 그 위치 / 없다면 스포너 주변으로 랜덤 스폰
    private Vector3 GetSpawnPosition()
    {
        // 스폰 포인트 없음
        int length = _spawnPoint.Count;
        if (_spawnPoint == null || length <= 0)
        {
            return GetBaseSpawnPosition();
        }
        // 스폰 포인트 있음
        int rand = Random.Range(0, length);
        Transform tr = _spawnPoint[rand];
        if(tr == null)
        {
            return GetBaseSpawnPosition();
        }
        return tr.position;
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        if (_registry == null || _registry.SpawnTable == null)
        {
            enabled = false;
            return;
        }
        if (_spawnOnStart)
        {
            // 빠르게 가져오겠다.
            _nextSpawnTime = Time.time + 0.2f;
        }
        else
        {
            // 테이블 간격 기반 스폰
            _nextSpawnTime = Time.time + _registry.SpawnTable.GetNextInterval();
        }
    }

    private void Update()
    {
        // 누수 방지 → 유니티 세계는 오브젝트 destroy 되면 주소가 알아서 null이 된다.
        int length = _alive.Count;
        for (int i = length - 1; i >= 0; --i)
        {
            var alive = _alive[i];
            if (alive == null)
            {
                _alive.RemoveAt(i);
            }
        }
        // 개체 수 제한
        if (length >= _maxAlive)
        {
            return;
        }
        // 쿨타임 중
        if (Time.time < _nextSpawnTime)
        {
            return;
        }
        if (TrySpawnOne())
        {
            _nextSpawnTime = Time.time + _spawnCooldown;
        }
    }
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────

    #endregion
}
