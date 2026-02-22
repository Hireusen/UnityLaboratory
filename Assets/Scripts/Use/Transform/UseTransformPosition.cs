using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 진동합니다.
/// </summary>
public class UseTransformPosition : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const float JITTER_MIN = -0.05f;
    private const float JITTER_MAX = 0.05f;
    private Vector3 _originPos;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _originPos = transform.position;
    }

    private void Update()
    {
        Vector3 pos = _originPos;
        pos.x += Random.Range(JITTER_MIN, JITTER_MAX);
        pos.y += Random.Range(JITTER_MIN, JITTER_MAX);
        pos.z += Random.Range(JITTER_MIN, JITTER_MAX);
        transform.position = pos;
    }
    #endregion
}
