using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class DoorInteractable : ExInteractableBase
{
    // 토글 느낌을 내는 것
    // DoorInteractable : 토글
    // SignInteractable : 계속 (지속성) != 단발성
    // 인벤 + 아이템 : 둘이 세트 한쪽에서 줏어먹으면 다른쪽에도 영향을 주는 느낌
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private GameObject _doorVisual;    // 스프라이트
    [SerializeField] private Collider2D _doorCollider;  // 막는 콜라이더

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _isOpen = false;
    #endregion

    protected override void OnInteract(ExInteractor interactor)
    {
        SetOpen(!_isOpen);
    }

    public void SetOpen(bool open)
    {
        _isOpen = open;
        if(_doorVisual != null) {
            _doorVisual.SetActive(!_isOpen); // 그냥 꺼버리네
        }
        if(_doorCollider != null) {
            _doorCollider.enabled = !_isOpen;
        }
        // 로그 찍기
        De.Print($"[Door] 오픈 {_isOpen}");
    }

    public bool IsOpen() => _isOpen;
}
