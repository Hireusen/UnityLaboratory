using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 회전 값이 떨립니다.
/// </summary>
public class UseQuaternionIdentity : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private const float MIN_JITTER_ANGLE = -9.9f;
    private const float MAX_JITTER_ANGLE = 9.9f;
    private Quaternion _originRotation;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _originRotation = transform.rotation;
    }

    // Translate = position + (rotation * translate)
    // 결론 : 자신의 방향을 고려해서 이동함.
    private void Update()
    {
        float randX = Random.Range(MIN_JITTER_ANGLE, MAX_JITTER_ANGLE);
        float randY = Random.Range(MIN_JITTER_ANGLE, MAX_JITTER_ANGLE);
        float randZ = Random.Range(MIN_JITTER_ANGLE, MAX_JITTER_ANGLE);
        Quaternion randRot = Quaternion.Euler(randX, randY, randZ);
        transform.rotation = _originRotation * randRot;
    }
    #endregion
}
