using UnityEngine;
using static FlappyGameManager2D;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyPipeSpawner : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private FlappyPipeFactory _factory;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private float _startDelay = 2f;
    [SerializeField] private float _minY = -1.5f;
    [SerializeField] private float _maxY = 1.5f;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private FlappyGameManager2D _gm;
    private float _nextSpawnTime;
    private bool _canSpawn = true;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void SpawnOnce()
    {
        if(_factory == null) {
            return;
        }

        float y = Random.Range(_minY, _maxY);
        Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
        _factory.CreatePipePair(pos);
        De.Print($"좌표({pos})에 파이프를 생성했습니다.");
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if(_factory == null) {
            _factory = FlappyPipeFactory.Ins;
        }
        if(_factory == null) {
            _factory = FindFirstObjectByType<FlappyPipeFactory>();
            De.Print("static인데도 없으면 찾는다고 이게 찾아질 수가 있나?");
        }
    }

    private void Start()
    {
        _nextSpawnTime = Time.time + _startDelay;
    }

    private void Update()
    {
        if (!_canSpawn) {
            return;
        }
        if(Time.time < _nextSpawnTime) {
            return;
        }
        SpawnOnce();
        _nextSpawnTime = Time.time + _spawnInterval;
    }
    #endregion
}
