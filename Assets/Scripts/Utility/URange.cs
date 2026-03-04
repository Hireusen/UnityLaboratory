using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// 범위 및 거리를 계산하는 유틸리티입니다.
/// </summary>
public class URange
{
    /// <summary>
    /// 두 좌표 사이의 거리가 일정 거리 이하인지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InCircle(Vector2 pos1, Vector2 pos2, float distance)
    {
        float sqrtDistance = distance * distance;
        float sqrtBetween = UMath.GetDistanceSquare(pos1, pos2);
        if (sqrtBetween < sqrtDistance) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 두 좌표 사이의 거리가 일정 거리 이하인지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InCircle(float x1, float y1, float x2, float y2, float distance)
    {
        float sqrtDistance = distance * distance;
        float sqrtBetween = UMath.GetDistanceSquare(x1, y1, x2, y2);
        if (sqrtBetween < sqrtDistance) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 좌표가 특정 좌표를 중심으로 한 사각형 범위 안에 있는지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InRect(Vector2 targetPos, Vector2 rectPos, float diameter)
    {
        float left = rectPos.x - diameter;
        float right = rectPos.x + diameter;
        float down = rectPos.y - diameter;
        float up = rectPos.y + diameter;
        if (targetPos.x < left) return false;
        if (targetPos.x > right) return false;
        if (targetPos.y < down) return false;
        if (targetPos.y > up) return false;
        return true;
    }

    /// <summary>
    /// 좌표가 특정 좌표를 중심으로 한 사각형 범위 안에 있는지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InRect(float targetX, float targetY, float rectX, float rectY, float diameter)
    {
        float left = rectX - diameter;
        float right = rectX + diameter;
        float up = rectY + diameter;
        float down = rectY - diameter;
        if (targetX < left) return false;
        if (targetX > right) return false;
        if (targetY < down) return false;
        if (targetY > up) return false;
        return true;
    }

    /// <summary>
    /// 좌표가 사각형 범위 안에 있는지 검사합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InRect(Vector2 targetPos, Vector2 minPos, Vector2 maxPos)
    {
        if (targetPos.x < minPos.x) return false;
        if (targetPos.x > maxPos.x) return false;
        if (targetPos.y < minPos.y) return false;
        if (targetPos.y > maxPos.y) return false;
        return true;
    }

    /// <summary>
    /// 직교 카메라의 화면에 보이는 최소, 최대 월드 좌표를 반환합니다.
    /// </summary>
    public static (Vector2 min, Vector2 max) GetCameraBounds2D(Camera camera)
    {
        // 직교 카메라가 아닐 경우
        if (!camera.orthographic) {
            De.Print($"직교 카메라 전용 메서드에 {camera.name}이 들어왔습니다.", LogType.Assert);
            return (Vector2.zero, Vector2.zero);
        }
        // 변수 준비
        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;
        Vector2 center = camera.transform.position;
        // 좌하단, 우상단
        Vector2 minBounds = new Vector2(center.x - halfWidth, center.y - halfHeight);
        Vector2 maxBounds = new Vector2(center.x + halfWidth, center.y + halfHeight);
        return (minBounds, maxBounds);
    }
}
