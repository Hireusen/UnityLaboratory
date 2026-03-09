using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// ~ 오브젝트에 부착하는 C# 스크립트입니다.
/// ~ 합니다.
/// </summary>
public class AsyncSequenceRunner : MonoBehaviour
{
    // 유니테스크 → 연출 / 로딩 / 상태 전환 같은 것들을 한 곳에서 실행 / 취소 처리를 해보고 싶다.
    // ㄴ 순서가 있는 작업들 처리를 할때 유용할 수 있다.
    #region ─────────────────────────▶ 공개 멤버 ◀─────────────────────────
    public void Cancel()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    // 요청이 연속으로 들어오면 이전 작업을 취소하고 가장 마지막 요청만 실행한다.
    public void RunLatest(Func<CancellationToken, UniTask> sequence)
    {
        // Func는 함수를 담는 델리게이트 타입 (콜백)
        // Func<입력들 ... , 반환 ...> 형태로 함수를 받는다라는 선언이다.
        // 딕셔너리같죠? 이 메서드 매개변수에서는 토큰을 키로 사용하고, UniTask를 뱉는다.

        // 이전 작업 취소
        Cancel();
        // 이번 실행에 대한 전용 토큰 발급
        _cts = new CancellationTokenSource();
        // 실행한다.
        RunInternal(sequence, _cts.Token, this.GetCancellationTokenOnDestroy()).Forget();
    }
    #endregion

    #region ─────────────────────────▶ 내부 변수 ◀─────────────────────────
    // 현재 실행중인 시퀀스를 취소하기 위한 토큰 소스
    CancellationTokenSource _cts;
    #endregion

    #region ─────────────────────────▶ 내부 메서드 ◀─────────────────────────
    private async UniTaskVoid RunInternal // 호출자는 기다리지 않는다. → 예외에 대한 처리가 필요하니 내부에서 해줘야 한다.
        (System.Func<CancellationToken, UniTask> sequence, CancellationToken local, CancellationToken destroy)
    {
        CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(local, destroy).Token;
        try
        {
            // 아무것도 안하겠다. → 외부에서 잘못 넘겨도 터지지 않도록
            if (sequence != null)
            {
                // 시퀀스 내부에서 토큰을 받아서, 외부에서 넘긴 시퀀스를 실행한다.
                await sequence(token);
            }
        }
        // 항상 가장 최신 것을 실행하려 하기 때문에 취소는 정상 흐름
        // 비동기 작업할 때 많이 쓰는 예외다.
        // (작업 자체가 개발자에 의해서) 정상적으로 취소가 되었음을 알리는 예외
        catch (OperationCanceledException)
        { 
            

        }
        catch (Exception e)
        {
            // 이건 진짜로 에러
            De.Print($"시퀀스 에러 발생! → {e.Message}", LogType.Assert);
        }
    }
    #endregion

    #region ─────────────────────────▶ 메시지 함수 ◀─────────────────────────
    private void OnDisable()
    {
        Cancel();
    }
    #endregion
}
