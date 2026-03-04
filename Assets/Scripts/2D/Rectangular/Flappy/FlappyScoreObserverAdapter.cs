using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어댑터 해보자
/// </summary>
public class FlappyScoreObserverAdapter : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private FlappyScoreManager2D _scoreManager;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private readonly List<IScoreAdapterObserver> _observers = new List<IScoreAdapterObserver>();
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // 이벤트를 받아서 옵저버들에게 재전파한다.
    private void HandleScoreChanged(int newScore)
    {
        // 여기에서 반복 돌린다.
        int length = _observers.Count;
        for (int i = 0; i < length; ++i) {

        }
    }

    private void Awake()
    {
        // 완전 방어
        if (_scoreManager == null) {
            _scoreManager = FlappyScoreManager2D.Ins;
            if (_scoreManager == null) {
                _scoreManager = FindFirstObjectByType<FlappyScoreManager2D>();
                if (_scoreManager == null) {
                    De.Print("?", LogType.Assert);
                }
            }
        }
    }

    private void OnEnable()
    {
        if (_scoreManager == null)
            return;
        _scoreManager.OnScoreChanged += HandleScoreChanged;
    }

    private void OnDisable()
    {
        if (_scoreManager == null)
            return;
        _scoreManager.OnScoreChanged -= HandleScoreChanged;
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    // 기존 방식도 쓸 수 있도록 해준다. → 병합할 때 터지지 않음
    public void Attach(IScoreAdapterObserver obs)
    {
        if (obs == null)
            return;
        if (_observers.Contains(obs))
            return;
        _observers.Add(obs);
    }

    public void Detach(IScoreAdapterObserver obs)
    {
        if (obs == null)
            return;
        if (!_observers.Contains(obs))
            return;
        _observers.Remove(obs);
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────

    #endregion
}
