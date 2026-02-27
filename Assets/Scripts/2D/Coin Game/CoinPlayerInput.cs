using UnityEngine;

/// <summary>
/// 플레이어 오브젝트에 부착하는 C# 스크립트입니다.
/// 키 입력을 받습니다.
/// </summary>
public class CoinPlayerInput : MonoBehaviour
{
    [Header("사용자 정의 설정")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    private CoinPlayerInfo _info;

    public void Initialize(CoinPlayerInfo info)
    {
        _info = info;
    }

    public void UpdateMoveX()
    {
        _info.moveX = Input.GetAxisRaw("Horizontal");
    }

    public bool PressedJump() => Input.GetKey(_jumpKey);
}
