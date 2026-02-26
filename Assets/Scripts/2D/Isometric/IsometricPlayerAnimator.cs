using UnityEngine;

/// <summary>
/// 플레이어 애니메이션 세팅 도우미.
/// 이 스크립트는 Animator Controller가 없을 때 기본 스프라이트 애니메이션을 처리합니다.
/// Animator Controller를 직접 만들기 어려울 때 이 스크립트를 대신 사용하세요.
/// 
/// [사용법]
/// 1. 플레이어에 이 스크립트 추가 (Animator 컴포넌트 대신)
/// 2. Inspector에서 각 상태의 스프라이트 배열 할당
///    - idleSprites: player-idle-1, player-idle-2, ...
///    - runSprites:  player-run-1, player-run-2, ...
///    - jumpSprites: player-jump-1, ...
/// </summary>
public class IsometricPlayerAnimator : MonoBehaviour
{
    [Header("스프라이트 시퀀스")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite[] jumpSprites;

    [Header("애니메이션 속도")]
    [SerializeField] private float frameRate = 8f;

    private SpriteRenderer spriteRenderer;
    private Sprite[] currentSprites;
    private float frameTimer;
    private int currentFrame;

    public enum AnimState { Idle, Run, Jump, Fall }
    private AnimState currentAnim = AnimState.Idle;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (currentSprites == null || currentSprites.Length == 0) return;

        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate) {
            frameTimer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }

    /// <summary>
    /// 외부에서 애니메이션 상태를 전환할 때 호출합니다.
    /// </summary>
    public void SetState(AnimState newState)
    {
        if (newState == currentAnim) return;

        currentAnim = newState;
        currentFrame = 0;
        frameTimer = 0f;

        switch (newState) {
            case AnimState.Idle:
                currentSprites = idleSprites;
                break;
            case AnimState.Run:
                currentSprites = runSprites;
                break;
            case AnimState.Jump:
            case AnimState.Fall:
                currentSprites = jumpSprites;
                break;
        }
    }
}
