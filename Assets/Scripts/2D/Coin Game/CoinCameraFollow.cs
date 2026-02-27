using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라에 부착하는 C# 스크립트입니다.
/// 카메라가 인스펙터에 등록된 트랜스폼을 따라갑니다.
/// </summary>
public class CoinCameraFollow : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _target;

    [Header("사용자 정의 설정")]
    // 카메라 특성
    [SerializeField] private EFollowType _followType = EFollowType.SmoothDamp;
    [SerializeField] private EEffectType _effectType = EEffectType.Normal;
    // 카메라 위치
    [SerializeField] private Vector3 _offset = new Vector3(0f, 4f, -10f);
    [SerializeField] private float _smoothTime = 0.15f;
    // 카메라 축 잠금
    [SerializeField] private bool _lookX = false;
    [SerializeField] private bool _lookY = false;
    // 카메라 위치 클램프
    [SerializeField] private Vector2 _minXY = new Vector2(-100, 2f);
    [SerializeField] private Vector2 _maxXY = new Vector2(100, 100f);
    [SerializeField] private bool _useClamp = false;
    // 디버그
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Vector3 _originPos;
    private Vector3 _curPos;
    private Vector3 _velocity;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    private enum EFollowType
    {
        Lerp,
        SmoothDamp,
        Immediately,
    }
    private enum EEffectType
    {
        Normal,
        Vibration,
    }
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void FollowCamera(bool snap = false)
    {
        // 변수 준비
        Vector3 camPos = transform.position;
        Vector3 desiredPos = _target.position + _offset;
        // 축 잠금
        if (_lookX) desiredPos.x = camPos.x;
        if (_lookY) desiredPos.y = camPos.y;
        // 클램프
        if (_useClamp) {
            desiredPos.x = Mathf.Clamp(desiredPos.x, _minXY.x, _maxXY.x);
            desiredPos.y = Mathf.Clamp(desiredPos.y, _minXY.y, _maxXY.y);
        }
        // 적용
        switch (_followType) {
            case EFollowType.Lerp:
                if (!snap) {
                    transform.position = Vector3.Lerp(camPos, desiredPos, Time.deltaTime * _smoothTime);
                    return;
                }
                break;
            case EFollowType.SmoothDamp:
                if (!snap) {
                    transform.position = Vector3.SmoothDamp(camPos, desiredPos, ref _velocity, _smoothTime);
                    return;
                }
                break;
        }
        camPos = desiredPos;
    }

    private void EffectCamera()
    {

    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Start()
    {
        _originPos = transform.position;
        _velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;
        FollowCamera();
    }
    #endregion
}
