using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 아이소메트릭 타일맵 전용 플레이어 컨트롤러.
/// 
/// ★ 핵심: 물리 중력을 사용하지 않습니다.
/// - 이동: 상하좌우 키로 타일맵 위를 자유롭게 이동
/// - 지면 판정: Tilemap.HasTile()로 현재 위치에 타일이 있는지 확인
/// - 점프: 스프라이트를 시각적으로 위로 올렸다 내림 (포물선)
/// - 낙하: 타일 밖으로 나가면 아래로 떨어짐
/// 
/// [설정 방법]
/// 1. 플레이어 빈 GameObject 생성
/// 2. 자식으로 스프라이트 오브젝트 생성 (SpriteRenderer에 player-idle-1 할당)
/// 3. 이 스크립트를 플레이어 루트에 추가
/// 4. Inspector에서:
///    - Ground Tilemap: Hierarchy의 Sample Tilemap 드래그
///    - Sprite Object: 자식 스프라이트 오브젝트 드래그
/// 5. Rigidbody2D, Collider 등 물리 컴포넌트는 필요 없음!
/// </summary>
public class IsometricPlayerController : MonoBehaviour
{
    [Header("=== 필수 할당 ===")]
    [Tooltip("타일이 깔려있는 Tilemap (Sample Tilemap)")]
    [SerializeField] private Tilemap groundTilemap;

    [Tooltip("플레이어 스프라이트가 있는 자식 오브젝트")]
    [SerializeField] private Transform spriteObject;

    [Header("이동")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("점프")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDuration = 0.4f;

    [Header("낙하")]
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float fallThresholdY = -10f;

    // 내부 상태
    private SpriteRenderer spriteRenderer;
    private Vector3 respawnPosition;
    private bool isJumping = false;
    private bool isFalling = false;
    private float spriteBaseY;

    private enum PlayerState { Idle, Move, Jump, Fall }
    private PlayerState currentState = PlayerState.Idle;

    private void Start()
    {
        // 스프라이트 자동 탐색
        if (spriteObject != null)
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteObject == null && spriteRenderer != null)
            spriteObject = spriteRenderer.transform;

        spriteBaseY = spriteObject.localPosition.y;
        respawnPosition = transform.position;

        // Tilemap 자동 탐색
        if (groundTilemap == null)
            groundTilemap = FindObjectOfType<Tilemap>();

        Debug.Log("[Player] 초기화 완료. 타일맵: " + groundTilemap?.name);
    }

    private void Update()
    {
        // ===== 낙하 중 =====
        if (isFalling) {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            if (transform.position.y < fallThresholdY)
                Respawn();
            return;
        }

        // ===== 이동 =====
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;

        if (input.sqrMagnitude > 0.01f) {
            Vector3 movement = new Vector3(input.x, input.y, 0f) * moveSpeed * Time.deltaTime;
            transform.position += movement;

            if (input.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (input.x < -0.01f)
                spriteRenderer.flipX = true;

            SetState(PlayerState.Move);
        } else if (!isJumping) {
            SetState(PlayerState.Idle);
        }

        // ===== 지면 체크 (점프 중 아닐 때) =====
        if (!isJumping && !IsOnGround()) {
            isFalling = true;
            SetState(PlayerState.Fall);
            Debug.Log("[Player] 타일 밖 → 낙하!");
            return;
        }

        // ===== 점프 =====
        if (Input.GetButtonDown("Jump") && !isJumping && !isFalling) {
            StartCoroutine(JumpCoroutine());
        }
    }

    /// <summary>
    /// 현재 위치 아래에 타일이 있는지 확인
    /// </summary>
    private bool IsOnGround()
    {
        if (groundTilemap == null) return true;
        Vector3Int cell = groundTilemap.WorldToCell(transform.position);
        return groundTilemap.HasTile(cell);
    }

    /// <summary>
    /// 시각적 점프: 스프라이트만 포물선으로 올렸다 내림.
    /// 점프 중에도 이동 가능. 착지 후 지면 없으면 낙하.
    /// </summary>
    private IEnumerator JumpCoroutine()
    {
        isJumping = true;
        SetState(PlayerState.Jump);

        float elapsed = 0f;
        while (elapsed < jumpDuration) {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;
            float yOffset = jumpHeight * 4f * t * (1f - t); // 포물선
            spriteObject.localPosition = new Vector3(
                spriteObject.localPosition.x, spriteBaseY + yOffset, spriteObject.localPosition.z);
            yield return null;
        }

        // 착지
        spriteObject.localPosition = new Vector3(
            spriteObject.localPosition.x, spriteBaseY, spriteObject.localPosition.z);
        isJumping = false;

        if (!IsOnGround()) {
            isFalling = true;
            SetState(PlayerState.Fall);
            Debug.Log("[Player] 착지 실패 → 낙하!");
        }
    }

    private void Respawn()
    {
        isFalling = false;
        isJumping = false;
        transform.position = respawnPosition;
        spriteObject.localPosition = new Vector3(
            spriteObject.localPosition.x, spriteBaseY, spriteObject.localPosition.z);
        SetState(PlayerState.Idle);
        Debug.Log("[Player] 리스폰!");
    }

    private void SetState(PlayerState s)
    {
        if (s != currentState) {
            currentState = s;
            Debug.Log($"[Player] 상태: {currentState}");
        }
    }
}
