using UnityEngine;

/// <summary>
/// 사실상 플레이어다.
/// 플레이어는 앞에 있는 상호작용 가능한 것이 있다면 특정 키를 눌러 상호작용한다.
/// </summary>
public class ExInteractor : MonoBehaviour
{
    // 필요한 것은 '누른다'와 '탐지'
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private Transform _origin;                 // 탐지 기준점
    [SerializeField] private float _radius = 3f;                // 상호작용 거리
    [SerializeField] private LayerMask _interactableLayer = 0;  // 상호작용 레이어 (0 = 전체)
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private bool _logEnable = true;
    [SerializeField] private bool _showGizmo = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private ExInteractableBase _current;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // 플레이어 근처에 상호작용 가능한 대상
    private void FindInteractable()
    {
        // 플레이어 근처에 상호작용 가능한 대상이 무엇이 있나?
        // 여러개가 잡힘녀 가장 가까운 것 하나를 선택핢
        // 상호작용 가능한 것을 current로 저장
        int mask = _interactableLayer.value;
        if(mask == 0) {
            mask = Physics2D.AllLayers;
        }
        // 콜라이더 + 배열 = 성능 부담 있음, 상대에 콜라이더가 없으면 탐지가 안됨.
        // 오버랩 : 트리거 여부 상관없이 다 잡는다. 단 객체가 많아질수록 반드시 레이어로 분리해서 관리해야 부담이 적음
        //         또한 아무런 처리를 하지 않는 타일맵이랑 궁합이 좋지 않다.
        Collider2D[] hits = Physics2D.OverlapCircleAll(_origin.position, _radius, mask);
        // 원 탐지는 프로그래머 기준에서 봤을 때 죄악과 같은 행위임 사실.. 연산량이 너무 높음
        // 일반적으로 원 탐지는 퍼포먼스에서 가장 좋지 않은 탐색 방법이다.
        if(hits == null) {
            return;
        }
        int length = hits.Length;
        if(length <= 0) {
            return;
        }
        ExInteractableBase best = null;
        float bestDist = float.MaxValue;
        for (int i = 0; i < length; ++i) {
            // 왜 Children? → 연결 즉 상속을 전제하는 경우가 많기 때문에 확인한다.
            // 콜라이더는 보통 자식에 붙어있는 편이고, 스크립트는 부모에 붙는 경우가 많기 때문이다.
            // 그래서 충돌체를 가진 오브젝트의 부모까지 올라가서 Interactable을 찾는다.
            ExInteractableBase target = hits[i].GetComponentInChildren<ExInteractableBase>();
            if(target == null) {
                continue;
            }
        }
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 인스펙터 유효성 검사
    public void Verification() {

    }

    // 스크립트 내부 변수 초기화
    public void Initialize() {

    }

    // 외부에 전달할 데이터 생성
    public void DataBuilder() {

    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if(_origin == null) {
            _origin = transform;
        }
    }

    private void OnEnable()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void LateUpdate()
    {

    }

    private void OnDisable()
    {

    }

    private void OnDestroy()
    {
        
    }

    private void Reset()
    {
        
    }

    private void OnValidate()
    {

    }
    #endregion
}
