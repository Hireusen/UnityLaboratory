using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyPipeFactory : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private FlappyPipePair2D _pipePairPrefab;
    [SerializeField] private Transform _parent; // 관리 측면 → 생성된 파이프로 인해 하이어라키 정리 용도
    #endregion

    #region ─────────────────────────▶ 메서드 ◀─────────────────────────
    public static FlappyPipeFactory Ins { get; private set; }

    private void Awake()
    {
        if(Ins != null && Ins != this) {
            Destroy(this);
            return;
        }
        Ins = this;
    }

    private void OnDestroy()
    {
        if(Ins == this) {
            Ins = null;
        }
    }

    public FlappyPipePair2D CreatePipePair(Vector2 pos, Quaternion rot)
    {
        if(_pipePairPrefab == null) {
            De.Print("파이프 페어 프리펩 어디갔어!");
            return null;
        }
        // null이면 루트에 생성
        // 부모가 있다면 그 아래로 정리
        Transform parent = _parent != null ? _parent : null;
        FlappyPipePair2D instance = Instantiate(_pipePairPrefab, pos, rot, parent);
        return instance;
    }

    public FlappyPipePair2D CreatePipePair(Vector2 pos)
    {
        return CreatePipePair(pos, Quaternion.identity);
    }
    #endregion
}
