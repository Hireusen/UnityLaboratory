using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataRegistrySO_", menuName = "2D3D/Data/Game Data Registry (SO)")]
public class GameDataRegistrySO : ScriptableObject
{
    // 데이터 묶음
    // 프로젝트에서 사용하는 데이터 묶음을 한 곳에서 처리한다.
    // 보통 SO쓰면 흐름이 다음과 같다.
    // 1단계 : 해당 파일에 대한 SO를 만든다. (하나의 곡)
    // 2단계 : 프로젝트에서 사용하는 데이터 묶음을 한 곳에서 처리한다. (CD나 레코드 판)
    // 3단계 
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("기본 설정")]
    [SerializeField, Min(0f)] private float _minInterval = 0.8f; // 랜덤 간격 뽑기
    [SerializeField, Min(0f)] private float _maxInterval = 1.6f;
    [SerializeField] private SpawnTableSO _spawnTable = null;
    [SerializeField] private List<ItemDataSO> _items = new List<ItemDataSO>();
    [SerializeField] private List<EnemyDataSO> _enemies = new List<EnemyDataSO>();

    [SerializeField] private int _score = 10;
    [SerializeField] private int _heal = 0;
    #endregion

    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public SpawnTableSO SpawnTable => _spawnTable;
    public IReadOnlyList<ItemDataSO> Items => _items;
    public IReadOnlyList<EnemyDataSO> Enemies => _enemies;
    #endregion
}
