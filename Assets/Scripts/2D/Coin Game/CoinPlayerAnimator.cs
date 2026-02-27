using UnityEngine;

/// <summary>
/// Player 오브젝트의 자식에 부착하는 애니메이션 컴포넌트입니다.
/// 현재 상태에 따라 애니메이터 파라미터를 변경하고, 방향을 전환합니다.
/// 애니메이션 컨트롤러는 이미 만들어져 있다고 가정합니다.
/// </summary>
public class CoinPlayerAnimator : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Animator _animator;

    /// <summary>
    /// 현재 상태에 따라 애니메이터 파라미터를 갱신합니다.
    /// </summary>
    public void UpdateAnimation(ECoinPlayerState state)
    {
        if (_animator == null)
            return;
        
    }
    #endregion
}
