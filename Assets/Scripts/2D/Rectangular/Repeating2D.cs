using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class Repeating2D : MonoBehaviour
{
    #region ─────────────────────────▶ 인스펙터 ◀─────────────────────────
    [Header("필수 요소 등록")]
    // 의존성이 있는 방식 → 없으면 자체 스피드로 돌아가게 하기 위해 아래에서 new
    [SerializeField] private ScrollSpeed2D _speedProvider;
    [SerializeField] private Transform _cameraTr;

    [Header("사용자 정의 설정")]
    [SerializeField, Range(0f, 1.5f)] private float _ratio = 0.5f; // 레이어 속도 비율
    [SerializeField] private float _fallBackSpeed = 2f;
    [SerializeField] private bool _autoGatherChildren = true; // 자동으로 자식을 세그먼트로 수집
    [SerializeField] private List<Transform> _segments = new List<Transform>(); // 런타임에 이미지 추가까지 고려하여 리스트
    [SerializeField] private float _extraBuffer = 1f; // 여유 값 (혹시 하얀 화면 보일까봐)
    [SerializeField] private float _overlap = 0.1f; // 배경 경계선을 숨기는 용도
    #endregion

    #region ─────────────────────────▶ 접근자 ◀─────────────────────────

    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    private float _segmentWidth = 0f; // 이미지 폭
    private float _halfWidth = 0f; // 가운데에서 오른쪽 끝까지의 거리
    private Camera _cam;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    // 세그먼트 폭 측정
    private float MeasureSegmentWidth(Transform seg)
    {
        SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
        if(sr == null) {
            return 0f;
        }
        // bounds : 월드 단위 크기
        return sr.bounds.size.x;
    }

    // 세그먼트 높이 측정
    private float MeasureSegmentHeight(Transform seg)
    {
        SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
        if(sr == null) {
            return 0f;
        }
        // bounds : 월드 단위 크기
        return sr.bounds.size.y;
    }

    // 카메라 왼쪽 끝 계산
    private float GetCameraLeftEdgeX()
    {
        // 세그먼트가 화면 밖으로 나갔는지 판단하기 위한 화면 경계 (왼쪽 끝)
        // 카메라가 없어? 대충이라도 구해보기
        if (_cam == null) {
            // 카메라가 없으면 정확한 값을 못 구함. 최소 동작만을 보장
            float camX = (_cameraTr != null) ? _cameraTr.position.x : 0f;
            return camX - _halfWidth;
        }
        // 카메라와 평면의 거리
        float z = Mathf.Abs(_cam.transform.position.z - transform.position.z);
        // 뷰포트 좌표(0 ~ 1)를 월드 좌표로 변환
        // x = 0 : 화면의 왼쪽 끝
        // y - 0.5 : 화면의 높이 중앙
        // z : 깊이감 (2D는 z -10, 배경 + 오브젝트는 z = 0 근처에 있도록)
        Vector3 worldPos = _cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, z));
        // 필요한 것은 화면 왼쪽 경계선의 X좌표
        return worldPos.x;
    }

    // 카메라 오른쪽 끝 계산
    private float GetRightMostRightEdgeX()
    {
        // 음의 무한대를 표현
        // 어떤 값이 오든 무조건 크기 때문에 첫 비교를 안전하게 성립
        float rightMost = float.NegativeInfinity;
        int length = _segments.Count;
        // 순회 → 오른쪽 끝 계산
        for (int i = 0; i < length; ++i) {
            float rightEdge = _segments[i].position.x + _halfWidth;
            // 가장 큰 rightEdge를 rightMost로 유지
            if(rightEdge > rightMost) {
                rightMost = rightEdge;
            }
        }
        return rightMost;
    }

    private void TickLoop()
    {
        // 보통 이런 로직을 작성할 때는 카메라를 기준점으로 해야 정확하다.
        // 카메라 왼쪽 화면 끝 X를 기준으로 → 완전히 화면 밖
        float camLeftX = GetCameraLeftEdgeX();
        // 실제 판정 기준선 (왼쪽 경계 + 여유)
        float leftLimit = camLeftX - _extraBuffer;
        //화면 밖으로 나간 세그먼트는 재배치
        int length = _segments.Count;
        float rightMostRightEdgeX = GetRightMostRightEdgeX();
        for (int i = 0; i < length; ++i) {
            // 검사 대상 세그먼트
            Transform seg = _segments[i];
            // segRightEdgeX → 이 세그먼트의 오른쪽 끝 X
            float segRightEdgeX = seg.position.x + _halfWidth;
            // 화면 밖 판정 조건 설정
            if(segRightEdgeX < leftLimit) {
                Vector3 pos = seg.position;
                // p.x = 가장 오른쪽 끝 + 가로 절반 - 오버랩
                pos.x = rightMostRightEdgeX +_halfWidth - _overlap;
                // 재배치
                seg.position = pos;
                rightMostRightEdgeX = pos.x + _halfWidth;
            }
        }
    }
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

    private float GetBaseSpeed()
    {
        if(_speedProvider != null) {
            return _speedProvider.Speed;
        }
        return _fallBackSpeed;
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void Reset()
    {
        if(_cameraTr == null && Camera.main != null) {
            _cameraTr = Camera.main.transform;
        }
    }

    private void Awake()
    {
        if(_cameraTr == null && Camera.main != null) {
            _cameraTr = Camera.main.transform;
        }
        // 캐싱
        _cam = _cameraTr.GetComponent<Camera>();
        if (_cam == null && Camera.main != null) {
            _cam = Camera.main;
        }
        // 이미지 자동 수집
        if (_autoGatherChildren) {
            int length = _segments.Count;
            _segments.Clear();
            for (int i = 0; i < length; ++i) {
                _segments.Add(transform.GetChild(i));
            }
        }
        // 없으면 안되지
        if(_segments == null || _segments.Count < 2) {
            De.Print("세그먼트가 null이거나 2개 미만입니다!", LogType.Assert);
            enabled = false;
            return;
        }
        // 폭 측정
        _segmentWidth = MeasureSegmentWidth(_segments[0]);
        _halfWidth = _segmentWidth * 0.5f;
        // 폭이 너무 작으면 루프 계산이 사실상 불가능
        if (_segmentWidth <= 0.0001f) {
            enabled = false;
            return;
        }
        // 오버랩 안전 보정
        // (음수일 경우 간격이 오히려 벌어짐)
        // (과하게 벌어져도 티가 많이남)
        _overlap = Mathf.Clamp(_overlap, 0f, _segmentWidth * 0.25f);
    }

    private void Update()
    {
        // 기존 속도를 가져오고 레이어 비율을 곱한다. → 실제 속도 반영
        float baseSpeed = GetBaseSpeed();
        float speed = baseSpeed * _ratio;
        float dx = speed * Time.deltaTime;
        for (int i = 0; i < _segments.Count; ++i) {
            {
                Vector3 p = _segments[i].position;
                p.x -= dx;
                _segments[i].position = p;
            }
        }
        TickLoop();
    }
    #endregion
}
