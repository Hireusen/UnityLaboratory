using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// 스크롤하기 위한 값을 제공합니다.
/// </summary>
public class ScrollSpeed2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private float _baseSpeed = 8.5f;
    [SerializeField] private float _speedMultiplier = 1f;
    [SerializeField] private bool _isPaused = false;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public float Speed
    {
        get {
            if (_isPaused) {
                return 0f;
            } else {
                return _baseSpeed * _speedMultiplier;
            }
        }
    }
    public bool IsPaused => _isPaused;
    public void Pause() => SetPaused(true);
    public void Resume() => SetPaused(false);
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void SetPaused(bool paused) => _isPaused = paused;
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void SetBaseSpeed(float baseSpeed)
    {
        _baseSpeed = Mathf.Max(0f, baseSpeed);
        De.Print($"2D 스크롤의 기본 속도를 {_baseSpeed}로 설정합니다.");
    }
    public void SetMultiplier(float multiplier)
    {
        _speedMultiplier = Mathf.Max(0f, multiplier);
        De.Print($"2D 스크롤의 속도 배율을 {_speedMultiplier}로 설정합니다.");
    }
    #endregion
}
