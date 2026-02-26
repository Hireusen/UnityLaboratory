using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// 플레이어 사망 사건을 전달하기 위해 StageFlow2D의 메서드를 호출합니다.
/// </summary>
public class DeathTrigger2D : MonoBehaviour
{
    // 사망 조건은 여러개가 있을 것이기 때문에 배열로 잡는 것이 좋다.
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private int _width = 0;
    [SerializeField] private int _height = 0;
    [SerializeField] private float _cellSize = 1f;

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private bool _dead = false;
    private ETileFlags[,] _flags;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    [System.Flags]
    public enum ETileFlags : uint
    {
        None = 0,
        // 기본 지형
        Ground = 1u << 0,
        OneWay = 1u << 1,
        Wall = 1u << 1,
        // 위험
        Spike = 1u << 1,
        Killzone = 1u << 1,
        Slow = 1u << 1,

        Solid = Ground | Wall,
        Hazard = Spike | Killzone
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 디버깅 메서드
    [ContextMenu("죽음 상태 리셋")]
    public void ResetDeath()
    {
        
    }

    public void SetFlags(int x, int y, ETileFlags flags)
    {
        _flags[x, y] |= flags;
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Reset()
    {
        _flags = new ETileFlags[_width, _height];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 방어 코드
        if (_onlyOnce && StageFlow2D.Ins.IsCleared)
            return;
        if (!other.CompareTag(_playerTag)) {
            De.Print($"플레이어가 아닌 엔티티가 목표 지점에 도달했습니다. ({other.name})");
            return;
        }
        // 활성화
        StageFlow2D.Ins.SetState(StageFlow2D.EStageState.Cleared);
        De.Print($"플레이어가 목표를 달성했습니다. ({other.name})");
        // 클리어 사건 전달
        if (StageFlow2D.Ins != null) {
            StageFlow2D.Ins.NotifyGoalReached();
        }
        // 왜 없지?
        else {
            De.Print("StageFlow2D 어디갔어요?");
        }
    }
    #endregion
}
