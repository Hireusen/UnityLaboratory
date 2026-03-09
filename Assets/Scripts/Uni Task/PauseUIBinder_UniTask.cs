using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class PauseUIBinder_UniTask : MonoBehaviour
{
    [SerializeField] private PauseInputHandler _pause;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeTime = 0.15f;
    [SerializeField] private Button _resumeButton;

    #region 내부 변수
    //private CancellationToken _fadeCt; // 현재 진행중인 페이드 작업을 취소하기 위한 토큰 소스
    // ㄴ 애는 할 수 있는게 좀 제한적이어서 매개 변수로 던져서 타입만 지정해준다.
    // ㄴ 내가 커스텀으로 만들어 쓴다. 이때 애를 자료형으로 가져온다.

    private CancellationTokenSource _fadeCts;
    // ↑이걸 더 많이 쓴다.
    // CancellationToken : 생성을 할때 편리하지만 결국 해제가 필요하기 때문에 토큰소스가 필요하다.
    // CnacellationTokenSource 해제를 담당한다고 보면 되는데 비동기니까 해제는 당연하고, 해제만 담당하니 생성이 필요하다.
    // 세트네? 그런데 state를 하나만 받을 수 있도록 되어있기 때문에 state를 여러개 줄 상황이 되면 답답하다.
    #endregion

    private void Awake()
    {
        // 탐색
        if (_pause == null)
        {
            _pause = FindFirstObjectByType<PauseInputHandler>();
        }
        // 시작할 때 꺼주고
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(false);
        }
        // 버튼 이벤트 연결
        if (_resumeButton != null)
        {
            _resumeButton.onClick.AddListener(OnClickResume);
        }
        // 캔버스 알파값
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    // 버튼 콜백
    // 버튼 클릭을 요청하면 OFF 요청을 하겠다.
    // 요청만 하고 처리는 핸들러쪽에서 할 수 있도록
    private void OnClickResume()
    {
        if (_pause == null)
        {
            return;
        }
        _pause.SetPaused(false);
    }

    private void StopFadeTask()
    {
        if (_fadeCts == null)
        {
            return;
        }

        // 페이드를 비동기 없이 즉시 중단
        _fadeCts.Cancel();
        // 자원 회수
        _fadeCts.Dispose();
        // 다음 페이드 준비해야 하니 null
        _fadeCts = null;
    }

    private void HandlePauseChanged(bool paused)
    {
        if (_pausePanel == null)
        {
            return;
        }
        if(_canvasGroup == null)
        {
            _pausePanel.SetActive(paused);
            return;
        }
        // 페이드 작업 리셋
        // 상태가 바뀔때마다 이전 페이드들 아웃시키고 새로 시작하기 위함이다.
        StopFadeTask();
        _fadeCts = new CancellationTokenSource();

        // 시동을 시켜야 한다. (유니테스크 시작)
        // 유니 테스크 포인트 → 유지 시킬것인지 / 아닌지? → 속성을 설정해주어야 한다. 이걸 Forget으로 찍어준다.
        FadeRoutine(paused, _fadeCts.Token, this.GetCancellationTokenOnDestroy()).Forget();
        // fire-and-forget : 간단하게 말해서 호출하면 잊는다.
        // 1. 그냥 던지고 끝나는 것 / 2. 안전하게 던지고 끝나는 것
        // 비동기 시퀀스를 설계하는 경우에는 2번을 해줘야 하고
        // 비동기 처리를 수행할 때는 1번으로 처리하는 경우가 많다. (괜찮은 경우가 있는거지 보통 위험하다.)
        //
        // ※ 예외가 밖으로 잘 안나오기 때문에 위험할 수 있다.
        // 안에서 await을 했는데 문제가 생겨 기다리지 않는다면?
        // 그러면 이 오류를 잡아낼 수가 없다. 호출자가 잡아야 하는데 밖으로 안나오기 때문에 잡을수가 없다.
        // 그래서 내부에서 일반적으로 try catch로 처리를 하는 경우가 많다.
        //
        // 취소 설계가 중요하다.
    }

    // localCts : 이번 페이드에 대한 전용 취소 토큰 발행
    // destroyToken : 상태가 바뀌면 (오브젝트가 파괴되면) 즉시 취소 → null 안전 코드
    // show → true : 패널 켜고 알파값 0에서 1로 페이드 인
    // show → false : 패널 켜고 알파값 1에서 0로 페이드 아웃
    private async UniTaskVoid FadeRoutine(bool show, CancellationToken localCts, CancellationToken destroyToken)
    {
        // UniTaskVoid : 기다리지 않는 비동기 작업에 많이 사용한다.
        // 즉 다른 유니테스크 호출해서 실행만 시키고 끝내고 싶을 때 사용한다. → 이벤트 핸들러들이 많이 들고 있다.
        
        // async / await은 서로 세트 → 이 함수 안에서 wait을 사용한다는 선언이다.
        // 반환형 자체는 비동기 타입이어야 한다.
        // await : 여기서 잠깐 멈추고 끝나면 작업 끝나면 이어서 한다. → 표현식 / 키워드 (실행 시점 제어)
        // await : 스레드를 멈추는 것은 아니고 함수의 진행만 멈춘다.

        // 토큰 링크 작업 → 토큰 두 개를 연결해서 하나의 변수에 담는다. 둘 중 하나라도 취소되면 토큰도 취소된다.
        // localCts, destroyToken 둘중 하나라도 취소되면 페이드는 즉시 중단되야 안전하다.
        // 유니티에서 흔히 볼 수 있는 패턴
        CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(localCts, destroyToken).Token;

        if (show)
        {
            _pausePanel.SetActive(true); // 패널 키고
            _canvasGroup.interactable = true; // 기능 활성화 시키고
            _canvasGroup.blocksRaycasts = true;

            // 페이드 처리 0 → 1
            await FadeAlpha(0f, 1f, _fadeTime, token);
        }

        else
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await FadeAlpha(1f, 0f, _fadeTime, token);
            // 페이드를 끝낸 후 비활성화 (완전 OFF)
            if(_pausePanel != null)
            {
                _pausePanel.SetActive(false);
            }
        }
    }

    // time동안 aplha을 from → to로 이동
    private async UniTask FadeAlpha(float from, float to, float time, CancellationToken token)
    {
        // UniTask : 는 기다릴 수 있는 작업(결과 / 예외 전달이 가능)에 많이 사용한다.
        // 끝날때까지 기다려야 하는 흐름에 적합하다.
        if(_canvasGroup == null)
        {
            return;
        }
        if(time < 0.0001f)
        {
            _canvasGroup.alpha = to;
            return;
        }
        float t = 0f;
        _canvasGroup.alpha = from;
        while (t < 1f)
        {
            // 중단 요청 확인
            // 취소 신호가 오면 즉시 중단하겠다.
            token.ThrowIfCancellationRequested();
            // 기반 진행률 (0 → 1)
            t += Time.unscaledDeltaTime / time;
            _canvasGroup.alpha = Mathf.Lerp(from, to, t);
            // 다음 프레임까지 대기 → 업데이트 타이밍에서 매 프레임 이어서 실행된다.
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        // 마지막 값 보정 (소수점 오차로 0.9999 방지)
        _canvasGroup.alpha = to;
    }

    private void OnEnable()
    {
        if (_pause == null)
        {
            return;
        }
        _pause.OnPausedChanged += HandlePauseChanged;
        HandlePauseChanged(PauseInputHandler.IsPaused);
    }

    private void OnDisable()
    {
        if (_pause != null)
        {
            _pause.OnPausedChanged -= HandlePauseChanged;
        }
        // 비동기 처리이기 때문에 해제는 당연하다.
        // Disable이 될때 테스크가 돌고 있으면 없는 오브젝트를 만질 예정이기 때문에 주의
        StopFadeTask();
    }
}
