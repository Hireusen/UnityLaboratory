using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 랜덤 값을 반환하는 유틸리티 클래스입니다.
/// </summary>
public class URand
{
    private static readonly Vector3[] _directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left, Vector3.up, Vector3.down };
    /// <summary>
    /// 여섯 방향 중 랜덤 3D 방향 벡터를 반환합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 GetAxis()
    {
        return _directions[Random.Range(0, 6)];
    }

    /// <summary>
    /// 씬에 존재하는 모든 트랜스폼 중 하나를 랜덤으로 반환합니다.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Transform GetTransform()
    {
        Transform[] allTransforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
        int length = allTransforms.Length;
        if (length > 0) {
            return allTransforms[Random.Range(0, length)];
        }
        return null;
    }
}
