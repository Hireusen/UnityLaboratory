using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class FlappyPipeCleanUp2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _cameraTr;

    [Header("사용자 정의 설정")]
    [SerializeField] private float _destoryBehind = 2f;
    #endregion

    private bool HasCamera => _cameraTr != null;

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Awake()
    {
        if (_cameraTr == null && Camera.main != null) {
            _cameraTr = Camera.main.transform;
        }
    }

    private void Update()
    {
        float camX = HasCamera ? _cameraTr.position.x : 0f;
        if(transform.position.x < camX - _destoryBehind) {
            De.Print($"파이프를 삭제했습니다. {camX} - {_destoryBehind}");
            Destroy(gameObject);
        }
    }
    #endregion
}
