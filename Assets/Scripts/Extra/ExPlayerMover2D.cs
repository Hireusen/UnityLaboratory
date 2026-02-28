using UnityEngine;

/// <summary>
/// 플레이어 오브젝트에 부착하는 C# 스크립트입니다.
/// 단순 양옆 이동합니다.
/// </summary>
public class ExPlayerMover2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody2D _rb;
    
    [Header("사용자 정의 설정")]
    [SerializeField] private float _moveSpeed = 20f;
    #endregion

    float _moveX;
    
    private void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 v = _rb.velocity;
        v.x = _moveSpeed * _moveX * Time.fixedDeltaTime;
        _rb.velocity = v;
    }
}
