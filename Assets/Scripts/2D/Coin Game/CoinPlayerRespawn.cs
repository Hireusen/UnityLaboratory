using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 오브젝트에 부착하는 C# 스크립트입니다.
/// 조건을 만족하면 자동으로 복귀시킵니다.
/// </summary>
public class CoinPlayerRespawn : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("사용자 정의 설정")]
    [SerializeField] private Vector2 _respawnPos = new Vector2(0f, 1f);
    [SerializeField] private float _voidY = -5f;
    #endregion

    private Transform _tr;

    #region ─────────────────────────▶ 외부 메서드 ◀─────────────────────────
    public void Initialize(Transform tr)
    {
        _tr = tr;
    }

    public bool PlayerInVoid() => (_tr.position.y < _voidY);

    public void ReturnPlayer()
    {
        Vector3 pos = _tr.transform.position;
        pos.x = _respawnPos.x;
        pos.y = _respawnPos.y;
        _tr.transform.position = pos;
    }

    public void TryReturnPlayer()
    {
        if (PlayerInVoid()) {
            ReturnPlayer();
        }
    }
    #endregion
}
