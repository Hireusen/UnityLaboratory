using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파이프 프리펩에 부착하는 C# 스크립트입니다.
/// </summary>
public class FlappyPipePair2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _topPipe;
    [SerializeField] private Transform _bottomPipe;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _gapSize = 3f;
    #endregion

    private float _halfGapSize;


    [ContextMenu("Gap 즉시 적용")]
    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private void ApplyGap()
    {
        if(_topPipe != null || _bottomPipe) {
            return;
        }

        Vector3 top = _topPipe.localPosition;
        top.y += _halfGapSize;
        _topPipe.localPosition = top;

        Vector3 bottom = _bottomPipe.localPosition;
        bottom.y += _halfGapSize;
        _topPipe.localPosition = bottom;
    }
    #endregion

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    private void OnValidate()
    {
        _halfGapSize = _gapSize * 0.5f;
#if UNITY_EDITOR
        ApplyGap();
#endif
    }
    #endregion
}
