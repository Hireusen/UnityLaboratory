using UnityEngine;

/// <summary>
/// 플레이어의 데이터를 저장합니다.
/// </summary>
public class CoinPlayerInfo
{
    public ECoinPlayerState state = ECoinPlayerState.Idle;
    public bool isGrounded = true;
    public bool lookToRight = true; // 마지막으로 오른쪽을 바라봄
    public float moveX; // 이번 프레임에 이동할 X 값
    public Transform foothold; // 현재 밟고 있는 타일의 트랜스폼
    public Vector2 prevFootholdPos; // 이전 프레임

    public bool IsAir => ECoinPlayerState.Jump == state || ECoinPlayerState.Fall == state;

    public CoinPlayerInfo()
    {
        this.state = ECoinPlayerState.Idle;
        this.moveX = 0f;
        this.isGrounded = true;
        this.lookToRight = true;
    }
}
