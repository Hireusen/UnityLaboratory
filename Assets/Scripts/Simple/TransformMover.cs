using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 오브젝트에 부착하는 C# 스크립트입니다.
/// 이동 키로 트랜스폼 기반으로 움직입니다.
/// </summary>
public class TransformMover : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _player;

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _enable = true;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private bool _rotateBody = true;
    [SerializeField] private float _rotateSpeed = 360f;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────
    public bool Enable
    {
        get { return _enable; }
        set { _enable = value; }
    }
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // GetAxis 기반으로 상하좌우 이동량 반환
    private bool TryGetMovement(out float v, out float h)
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        if (v == 0f && h == 0f) {
            return false;
        }
        return true;
    }

    // 키 입력으로 WASD 이동 시도
    private void TryMove()
    {
        float v, h;
        if (TryGetMovement(out v, out h)) {
            Vector3 axis = new Vector3(h, 0f, v);
            Vector3 movement = axis * _moveSpeed * Time.deltaTime;
            _player.position += movement;
        }
    }

    // 키 입력으로 바라보는 방향 전환
    private void RotateBody()
    {
        float v, h;
        if (TryGetMovement(out v, out h)) {
            Vector3 axis = new Vector3(h, 0f, v);
            Quaternion desired = Quaternion.LookRotation(axis);
            _player.rotation = Quaternion.RotateTowards(_player.rotation, desired, _rotateSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (_player == null) {
            _player = transform;
        }
    }

    private void Update()
    {
        if (_player == null)
            return;
        if (_enable) {
            TryMove();
        }
        if (_rotateBody) {
            RotateBody();
        }
    }

    private void Reset()
    {
        if (_player == null) {
            _player = transform;
        }
    }
    #endregion
}
