using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class ExItemInteractable : ExInteractableBase
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private string _itemID = "Key_A";
    [SerializeField] private bool _destroyOnPickUp = true;

    // 인벤토리 → 단일로는 의미 없음
    // 아이템 → 단일로는 의미 없음
    // 둘은 하나라고 생각하고 접근해서 만들어야 한다.
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    protected override void OnInteract(ExInteractor interactor)
    {
        // 데이터로 확인을 해야 한다.
        ExInventoryStub inventory = interactor.GetComponent<ExInventoryStub>();
        inventory.Add(_itemID);
        // 필요하면 후처리할 공간 ↓

        // 일단 삭제로 해놨는데 오브젝트 풀링으로 바꿔주는게 좋음
        if (_destroyOnPickUp) {
            Destroy(gameObject);
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
