using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라에 부착하는 C# 스크립트입니다.
/// 카메라가 플레이어를 따라다니며 상황에 따라 연출이 적용됩니다.
/// </summary>
public class CameraFollow2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _target;
    [SerializeField] private string _targetTag = "Player";

    [Header("사용자 정의 설정")]
    // 카메라 위치
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -10f);
    [SerializeField] private bool _useSmooth = true;
    [SerializeField] private float _smoothTime = 0.15f;
    // 카메라 축 잠금
    [SerializeField] private bool _lookX = false;
    [SerializeField] private bool _lookY = true;
    // 카메라 위치 클램프
    [SerializeField] private Vector2 _minXY = new Vector2(-2, 2f);
    [SerializeField] private Vector2 _maxXY = new Vector2(10, -10f);
    [SerializeField] private bool _useClamp = true;
    // 디버그
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    public enum ECameraType
    {
        
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Vector3 _velocity;
    private Vector3 _startPos;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────

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
        _startPos = transform.position;
        if (string.IsNullOrEmpty(_targetTag)) {
            enabled = false;
            De.Print($"태그({_target})가 존재하지 않습니다.");
            return;
        }
        if (_target == null) {
            GameObject go = GameObject.FindGameObjectWithTag(_targetTag);
            if (go == null) {
                enabled = false;
                De.Print($"카메라가 존재하지 않습니다.");
                return;
            }
        }
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        if (!_logEnable)
            return;
        De.Print("");
    }
    private void LateUpdate()
    {
        if (_target == null)
            return;
        Vector3 camPos = transform.position;
        // 목표 위치 계산
        Vector3 desiredPos = _target.position + _offset;
        // 축 잠금
        if (_lookX) desiredPos.x = camPos.x;
        if (_lookY) desiredPos.y = camPos.y;
        // 클램프
        if (_useClamp) {
            //desiredPos.x = Mathf.Clamp(desiredPos.x, _minXY.x, _maxXY.x);
            //desiredPos.x = Mathf.Clamp(desiredPos.y, _minXY.y, _maxXY.y);
        }
        // 적용
        if (_useSmooth) {
            Vector3.Lerp(camPos, desiredPos, Time.deltaTime * _smoothTime);
            //transform.position = Vector3.SmoothDamp(camPos, desiredPos, ref _velocity, Mathf.Max(0.0001f, _smoothTime));
        } else {
            camPos = desiredPos;
        }
    }
    #endregion
}
