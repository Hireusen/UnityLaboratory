using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트의 정면에 산탄처럼 디버그 레이를 발사합니다.
/// </summary>
public class UseTransformVector : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const int LINE_COUNT = 2;
    private const float MIN_LINE_DISTANCE = 2f;
    private const float MAX_LINE_DISTANCE = 5f;
    private const float SPREAD_ANGLE = 45f;
    private Color _color;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _color = URand.GetColor();
    }

    private void Update()
    {
        for (int i = 0; i < LINE_COUNT; ++i) {
            float x = Random.Range(-SPREAD_ANGLE, SPREAD_ANGLE);
            float y = Random.Range(-SPREAD_ANGLE, SPREAD_ANGLE);
            Quaternion rotation = Quaternion.Euler(x, y, 0f);
            float distance = Random.Range(MIN_LINE_DISTANCE, MAX_LINE_DISTANCE);
            Vector3 offset = rotation * Vector3.forward * distance;
            Debug.DrawRay(transform.position, transform.TransformVector(offset), _color);
        }
    }
    #endregion
}
