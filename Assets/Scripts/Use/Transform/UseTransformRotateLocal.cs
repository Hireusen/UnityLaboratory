using UnityEngine;

/// <summary>
/// 오브젝트에 부착하는 C# 스크립트입니다.
/// 부착된 오브젝트는 랜덤 축으로 회전합니다.
/// </summary>
public class UseTransformRotateLocal : MonoBehaviour
{
    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    [SerializeField] private Vector3 _originAxis;
    private const float ROTATION_SPEED = 135f;
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        int rand = Random.Range(0, 6);
        switch (rand) {
            case 0: _originAxis = Vector3.forward; break;
            case 1: _originAxis = Vector3.right; break;
            case 2: _originAxis = Vector3.back; break;
            case 3: _originAxis = Vector3.left; break;
            case 4: _originAxis = Vector3.up; break;
            case 5: _originAxis = Vector3.down; break;
        }
    }

    private void Update()
    {
        transform.Rotate(_originAxis, ROTATION_SPEED * Time.deltaTime, Space.Self);
    }
    #endregion
}
