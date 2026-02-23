using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 진동합니다.
/// </summary>
public class UseDebugRay : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Color _color;
    private float _distance;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _color = URand.GetColor();
        _distance = UTool.GetAverageScale(transform) * 0.8f;
        if(_distance < 1f) {
            _distance = 1f;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _distance, _color);
    }
    #endregion
}
