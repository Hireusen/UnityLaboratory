using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 점수를 관리하고 옵저버 역할을 한다.
/// </summary>
public class ScoreSubject_Classic : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private int _startScore = 0;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private readonly List<IScoreObserver> _observes = new List<IScoreObserver>();
    private int _score;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    public void Attach(IScoreObserver obs)
    {
        if (obs == null) {
            De.Print("Null이 들어왔습니다.", LogType.Warning);
            return;
        }
        if (_observes.Contains(obs)) {
            De.Print("obs가 이미 존재합니다.", LogType.Warning);
            return;
        }
        _observes.Add(obs);
    }

    public void Detach(IScoreObserver obs)
    {
        if (obs == null) {
            De.Print("Null이 들어왔습니다.", LogType.Warning);
            return;
        }
        if (!_observes.Contains(obs)) {
            De.Print("obs가 존재하지 않습니다.", LogType.Warning);
            return;
        }
        _observes.Remove(obs);
    }

    private void Notify()
    {

    }

    public int GetScore() => _score;
    public void ResetScore()
    {
        _score = _startScore;
    }

    public void AddScore(int amount)
    {
        if (amount == 0)
            return;
        _score += amount;
        if(_score < 0) {
            _score = 0;
        }
        // 알림
        Notify();
    }
    #endregion
}
