using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class CoinPlayerTrigger : MonoBehaviour
{
    [Header("사용자 정의 설정")]
    [SerializeField] private string _itemTag = "Item";

    private CoinStageManager _stage;

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (String.IsNullOrEmpty(_itemTag)) {
            De.Print("태그에 아무것도 입력하지 않았습니다.", LogType.Assert);
            enabled = false;
            return;
        }
    }
    private void Start()
    {
        _stage = CoinStageManager.Ins;
        if (_stage == null) {
            De.Print("왜인지 스테이지 매니저가 존재하지 않습니다.", LogType.Assert);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.CompareTag(_itemTag)) {
            go.SetActive(false);
            _stage.AddCoin();
            De.Print($"코인을 획득했습니다! (현재 {_stage.GetCoin()}개)");
        }
    }
    #endregion
}
