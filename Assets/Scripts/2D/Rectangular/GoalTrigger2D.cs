using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포탈 역할을 하는 오브젝트에 부착하는 C# 스크립트입니다.
/// 클리어 사건을 전달하기 위해 StageFlow2D의 메서드를 호출합니다.
/// </summary>
public class GoalTrigger2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 디버깅 메서드
    [ContextMenu("게임 클리어 상태 리셋")]
    public void ResetGoal()
    {
        
        //De.Print($"게임 클리어 상태를 {prev}에서 {_gameClear}로 변경합니다.");
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if(col != null) {
            // 이거 깜박하는 경우가 많아서 코드에서 잡아줬음
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 방어 코드
        if(_onlyOnce && StageFlow2D.Ins.IsCleared)
            return;
        if (!other.CompareTag(_playerTag)) {
            De.Print($"플레이어가 아닌 엔티티가 목표 지점에 도달했습니다. ({other.name})");
            return;
        }
        De.Print($"플레이어가 목표를 달성했습니다. ({other.name})");
        // 클리어 사건 전달
        if(StageFlow2D.Ins != null) {
            StageFlow2D.Ins.NotifyGoalReached();
        }
        // 왜 없지?
        else {
            De.Print("StageFlow2D 어디갔어요?");
        }
    }
    #endregion
}
