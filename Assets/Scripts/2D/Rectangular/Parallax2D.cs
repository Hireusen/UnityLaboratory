using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// 패럴렉스 합시다
/// </summary>
public class Parallax2D : MonoBehaviour
{
    [System.Serializable]
    private struct ParallaxLayer
    {
        public Transform tsf;
        [Range(-2f, 2f)] public float factor; // 이동 비율
    }

    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    [SerializeField] private Transform _cameraTsf;
    [SerializeField] private ParallaxLayer[] _layers;

    [Header("사용자 정의 설정")]
    [SerializeField] private bool _useX = true;
    [SerializeField] private bool _useY = true;
    [SerializeField] private bool _logEnable = true;
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private Vector3 _prevCamPos; // 이동량 구하기 위해서
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
        if(_cameraTsf == null) {
            _cameraTsf = Camera.main.transform;
            if(_cameraTsf == null) {
                De.Print("메인 카메라조차 없습니다 !");
                enabled = false;
                return;
            }
        }
        _prevCamPos = _cameraTsf.position;
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        if (!_logEnable)
            return;
        if(_layers != null) {
            int length = _layers.Length;
            for (int i = 0; i < length; ++i) {
                string name = (_layers[i].tsf != null) ? _layers[i].tsf.name : "NULL";
                De.Print($"레이어[{i}] = {name} / 팩터 : {_layers[i].factor:0.00}");
            }
        }
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        // 카메라 이동량
        Vector3 camPos = _cameraTsf.position;
        Vector3 delta = camPos - _prevCamPos;
        // 레이어 이동
        if (_layers != null) {
            int length = _layers.Length;
            for (int i = 0; i < length; ++i) {
                // 방어 코드
                var layer = _layers[i];
                if (layer.tsf == null)
                    return;
                // 변수 준비
                Vector3 movement = Vector3.zero;
                float factor = layer.factor;
                // 축 별 적용
                if (_useX) {
                    movement.x = delta.x * factor;
                }
                if (_useY) {
                    movement.y = delta.y * factor;
                }
                layer.tsf.position += movement;
            }
            _prevCamPos = _cameraTsf.position;
        }
    }

    private void OnDisable()
    {

    }

    private void OnDestroy()
    {
        
    }

    private void Reset()
    {
        
    }

    private void OnValidate()
    {

    }
    #endregion
}
