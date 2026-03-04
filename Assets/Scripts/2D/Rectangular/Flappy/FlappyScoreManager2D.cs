using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빈 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyScoreManager2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private int _startScore = 0; // 동작 안정성을 위해서
    [SerializeField] private bool _enableLog = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public event Action<int> OnScoreChanged;
    public static FlappyScoreManager2D Ins {  get; private set; }

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private int _score;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // silent는 무슨 역할? 그냥 이벤트 발행여부 결정하는거였네
    private void SetScore(int next, bool silent = false)
    {
        if(_score == next) {
            return;
        }
        _score = next;
        if (_enableLog) {
            De.Print($"점수 갱신 ({_score})");
        }
        if (!silent) {
            OnScoreChanged?.Invoke(_score);
        }
    }

    // silent는 무슨 역할? 그냥 이벤트 발행여부 결정하는거였네
    public void AddScore(int amount)
    {
        // 의미 없음. 이벤트 발행도 하니까 의미 없으면 방어해주자
        if(amount == 0) {
            return;
        }
        // 실행
        int next = _score + amount;
        if (next < 0) {
            next = 0;
        }
        SetScore(next, true);
    }

    public int GetScore() => _score;
    public int ResetScore() => _score = _startScore;
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 인스펙터 유효성 검사
    public void Verification() {

    }

    // 스크립트 내부 변수 초기화
    public void Initialize() {

    }

    // 외부에 전달할 데이터 생성
    public void DataBuilder() {

    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        // 주인이 아님
        if(Ins != null && Ins != this) {
            Destroy(this);
            De.Print("플래피 스코어 매니저를 삭제했습니다.");
            return;
        }
        // 새로 생성
        Ins = this;
    }

    private void OnDestroy()
    {
        if(Ins == this) {
            Ins = null;
        }
    }
    #endregion
}
