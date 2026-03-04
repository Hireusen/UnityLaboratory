using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 움직여야 하는 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyObjectMover2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private ScrollSpeed2D _speedProvider;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _multiplier = 1f;
    [SerializeField] private float _fallbackSpeed = 2f;
    [SerializeField] private bool _useUnscaledTime = false;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private float GetBaseSpeed()
    {
        if(_speedProvider != null) {
            return _speedProvider.Speed;
        }
        return _fallbackSpeed;
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Reset()
    {
        if(_speedProvider == null) {
            _speedProvider = FindFirstObjectByType<ScrollSpeed2D>();
        }
    }

    private void Update()
    {
        float baseSpeed = GetBaseSpeed();
        float speed = baseSpeed * _multiplier;
        float dt = _useUnscaledTime ? Time.unscaledTime : Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x -= speed * dt;
        transform.position = pos;
    }
    #endregion
}
