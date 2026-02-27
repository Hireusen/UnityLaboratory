using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 규칙에 따라 움직이는 오브젝트에 부착하는 C# 스크립트입니다.
/// 자식 오브젝트의 위치를 순회하거나 원형 이동합니다.
/// </summary>
public class CoinMoveToRule : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private EntityInfo[] _entitys;
    #endregion

    #region ─────────────────────────▶ 중첩 타입 ◀─────────────────────────
    [Serializable]
    private class EntityInfo
    {
        public Transform tr;
        public EType type = EType.Point;
        public float moveSpeed = 5f;
        // 포인트
        public Transform[] points;
        public int nextPoint = 0;
        // 원
        public float radius = 0f;
        public float radian = 0f;
    }

    private enum EType : byte { Point, Circle }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    const float ARRIVE_DISTANCE = 0.002f;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void Verification()
    {
        // 유효성 검사
        int length = _entitys.Length;
        for (int i = 0; i < length; ++i) {
            var entity = _entitys[i];
            if (De.IsNull(entity)) {
                enabled = false;
                return;
            }
            if (De.IsNull(entity.tr)) {
                enabled = false;
                return;
            }
        }
    }

    // 포인트에 도착했는가?
    private bool IsArrivePoint(Vector2 curPos, Vector2 targetPos)
    {
        // 도착 판정
        if ((curPos - targetPos).sqrMagnitude < ARRIVE_DISTANCE) {
            return true;
        }
        return false;
    }

    // 엔티티이 포인트를 순회하며 이동합니다.
    private void PlatformPointMover(EntityInfo entity, Vector2 curPos)
    {
        Vector2 targetPos = entity.points[entity.nextPoint].position;
        // 이동
        entity.tr.position = Vector2.MoveTowards(curPos, targetPos, entity.moveSpeed * Time.deltaTime);
        // 도착했을 경우
        if (!IsArrivePoint(curPos, targetPos))
            return;
        // 다음 인덱스
        entity.nextPoint++;
        if (entity.points.Length <= entity.nextPoint) {
            entity.nextPoint = 0;
        }
    }

    // 엔티티를 원 형태로 이동시킵니다.
    private void PlatformCircleMover(EntityInfo entity)
    {
        entity.radian += entity.moveSpeed * Time.deltaTime;
        // 라디안으로 좌표 계산
        float newX = Mathf.Cos(entity.radian) * entity.radius;
        float newY = Mathf.Sin(entity.radian) * entity.radius;
        // 좌표 적용
        Vector2 newPos = entity.points[0].position;
        newPos.x += newX;
        newPos.y += newY;
        entity.tr.position = newPos;
    }

    // 모든 엔티티을 순환하며 이동시킵니다.
    private void PlatformMover()
    {
        int length = _entitys.Length;
        for (int i = 0; i < length; ++i) {
            var entity = _entitys[i];
            Vector2 curPos = entity.tr.position;
            switch (entity.type) {
                case EType.Point:
                    PlatformPointMover(entity, curPos);
                    break;
                case EType.Circle:
                    PlatformCircleMover(entity);
                    break;
            }
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        Verification();
    }

    private void Update()
    {
        PlatformMover();
    }

    // 데이터 유효성 검사
    private void OnValidate()
    {
        int length = _entitys.Length;
        for (int i = 0; i < length; ++i) {
            var entity = _entitys[i];
            // Null 체크
            if (entity == null)
                return;
            if (entity.tr == null)
                return;
            // 데이터 체크
            if(entity.moveSpeed <= 0f) {
                entity.moveSpeed = 1f;
            }
            switch (entity.type) {
                case EType.Point:
                    if (UArray.InBounds(entity.points, entity.nextPoint)) {
                        entity.nextPoint = 0;
                    }
                    break;
                case EType.Circle:
                    if(entity.radius <= 0f) {
                        entity.radius = 1f;
                    }
                    if(entity.points.Length != 1) {
                        entity.points = new Transform[1];
                    }
                    break;
            }
        }
        
    }
    #endregion
}
