using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class ExInventoryStub : MonoBehaviour
{
    /* 인벤토리 뼈대
     * 아이템 보유 여부를 검증한다. → 자료구조
     * 상호작용이 되는지? → 상속, 다형성
     * 적합하게 나눠서 동작하는가? (인벤토리, 아이템) → 알고리즘
     * ↑  /  ↓
     * 아이템 렌더링 작업 → 리소스 필요
     * 후처리 (이펙트 / 사운드) → 리소스 필요
     * 
     * 위 내용은 인벤토리라면 고정이지만 인벤토리를 구현하는 방법은 기상천외하게 많다.
     * 옛날엔 인벤토리가 역량 증명의 상징이었는데 요즘엔 라이트해진 느낌이 좀 있어서 퇴색된 듯.
     * 인벤토리는 포폴에 왠만하면 넣으세요. 페이크 인벤토리는 곤란
     */

    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _log = false;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    // 아이템은 고유 ID를 갖는다는 특성이 있다. → 문자열로 처리할 준비중
    // List : 이도저도 아닌 애매한 인벤토리 구조라면 일반적으로 선택한다.
    // HashSet : 속도면에서 리스트나 딕셔너리 자료구조보다 굉장히 유리하다. 단 확장성은 떨어짐
    // Dictionary : 수량 인벤토리에 유리한 구조
    // 참고 : HashSet 쓰다가 나중에 List로 변경하는 건 굉장히 쉽다. 다만 Dictionary로 바꾸는 건 고민 필요
    // 참고 : 리스트와 딕셔너리 간 변환은 단순 리팩토링보다 아예 다시 만드는 게 유리한 경우가 많음
    private readonly HashSet<string> _items = new HashSet<string>();

    // 데이터가 바뀔 때 사실만 외부에서 확인할 수 있도록 자리 마련
    public event Action<string> OnItemAdded;
    public event Action<string> OnItemRemoved;

    public IReadOnlyCollection<string> Items => _items;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 쿼리 기반으로 빠르게 탐색이 가능하다.
    // 아이템을 가지고 있는지 확인
    public bool Has(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) {
            return false;
        }
        // 우리가 편한 값 / 컴퓨터가 확인해야 하는 아이템 키 값 (ID)
        return _items.Contains(itemID);
    }

    public void Add(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) {
            return;
        }
        bool added = _items.Add(itemID);
        // ↓ 함수 호출 자리들
        // 정렬 , 앞 데이터 확인하고 들어올지 등
        // ↓ 로그
        if (_log) {
            if (added) {
                De.Print($"인벤토리에 아이템{itemID}을 추가했습니다.");
            } else {
                De.Print($"인벤토리에 아이템{itemID}을 추가하지 못했습니다.");
            }
        }
    }

    // 위 프로토타입에서 점점 살이 붙었다.
    public bool TryAdd(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) {
            return false;
        }
        bool added = _items.Add(itemID);
        if (added) {
            // 만약 실제 게임이라면?
            // 데이터 날리고 쪼개서 검증 →
            OnItemAdded?.Invoke(itemID);
        }
        return added;
    }

    public void Remove(string itemID)
    {
        TryRemove(itemID);
    }

    public bool TryRemove(string itemID)
    {
        if (string.IsNullOrEmpty(itemID)) {
            return false;
        }
        bool removed = _items.Remove(itemID);
        if (removed) {
            OnItemRemoved?.Invoke(itemID);
        }
        if (_log) {
            if (removed) {
                De.Print($"인벤토리에서 아이템{itemID}을 제거했습니다.");
            } else {
                De.Print($"인벤토리에서 아이템{itemID}을 제거하지 못했습니다.");
            }
        }
        return removed;
    }

    public void Clear()
    {
        // 이런 메서드는 주의해야한다. 어떤 나비효과를 받을지 알 수 없음

        // 직접 가서 확인해야 하고 인스턴스 삭제
        // 참조가 있거나 물려있는 객체라면 참조 끊어주고 물려 있는 관계 돌려야 한다.
        // ↓ 아래처럼 만들면 위처럼 참조 관계를 만들지를 않는다. ↑

        // 인벤토리는 아이템을 구축할 떄 전역 처리로 관리하고, 인벤토리는 왠만하면 의존성을 두지 않는다.
        // 아이템은 전역 처리로 관리하고 구조체로 나가는 경우가 많음
        // ㄴ 그냥 던져 놓으면 아무나 쓰거나 바꿀 수 있기 때문에 -> 사용을 하려고 하는 쪽에서 생성하고 쓸 수 있게 처리를 해야한다.
        // ㄴ new
        // ㄴ 여기까지 생각이 도달했다면 ItemInfo 클래스를 만들어서 구조체를 만들려고 한다. → 던지면 오케이
        // ㄴ 여기에서 더 효율적으로 사용하겠다면 메타 데이터를 분리한다. (SO)
        // ㄴ 이름, 속성, 방어력, 공격력, 판매 가격 등
        // ㄴ (고정) 저장 / 로드 구현해서 관리 (제이슨?) + 트랙잭션
        // 뭐야 이거 Clear가 아니라 설계 구조인 것 같은데
        if (_items == null) {
            return;
        }
        _items.Clear();
        if (_log) {
            De.Print("아이템을 모두 청소했습니다.");
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────

    #endregion
}
