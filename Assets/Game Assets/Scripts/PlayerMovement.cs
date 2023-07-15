using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } } // Oyuncunun Rigidbody bile�enine eri�im
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private AudioSource auso;

    private float dirX; // Yatay giri� de�eri
    private float raycastExtentY; // Yatay raycast
    private float raycastExtentX; // Dikey raycast
    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket h�z�
    [SerializeField] private float playerJump = 14f; // Oyuncunun z�plama g�c�
    [SerializeField] private float wallSlideSpeed = 2f; // Duvara kayma h�z�
    [SerializeField] private float wallJumpForce = 10f; // Duvara z�plama g�c�

    private int isWall; // Duvara �arp�p �arpmad���n� kontrol etmek i�in kullan�lan int

    private bool isGround; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan bool
    private bool isWallSliding; // Duvara kay�p kaymad���n� kontrol etmek i�in kullan�lan bool
    private bool doubleJump; // �ift z�plama
    private bool iswallJump; // Duvara z�plama
    private bool isLevelStart = true; // Level ba�lang�c�
    private PlayerLife pl;

    public bool DoubleJump { get { return doubleJump; } set { doubleJump = value; } } // doubleJump de�i�kenine eri�im ve de�i�tirme

    [SerializeField] private LayerMask groundLayer; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan layer maskesi
    [SerializeField] private List<AudioClip> clipList;

    private enum MovementState { idle, running, jumping, falling, doubleJump, wallSlide } // Oyuncunun hareket durumlar�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bile�enine eri�im
        anim = GetComponent<Animator>(); // Animator bile�enine eri�im
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer bile�enine eri�im
        bc = GetComponent<BoxCollider2D>(); // BoxCollider2D bile�enine eri�im
        auso = GetComponent<AudioSource>(); // AudioSource bile�enine eri�im
        pl = GameObject.FindObjectOfType<PlayerLife>();

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yar�s�
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yar�s� 

        StartCoroutine(LevelStart()); // Leveli Ba�lat
    }

    private void Update()
    {
        isGround = IsGroundedCheck(); // Oyuncunun yerde olup olmad���n� kontrol et
        isWall = IsWallCheck(); // Oyuncunun duvara �arp�p �arpmad���n� kontrol et

        dirX = Input.GetAxisRaw("Horizontal"); // Yatay giri� de�erini al

        if (Input.GetKeyDown(KeyCode.W) && !pl.IsDead && !isLevelStart)
        {
            // Burada kald�n. walljump yaparken doublejump yapmamas�n� sa�laman laz�m.
            if (isWallSliding)
            {
                auso.PlayOneShot(clipList[2]);
                iswallJump = true;
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                rb.AddForce(new Vector2(-isWall * wallJumpForce, 0), ForceMode2D.Impulse);
            }
            else if (isGround)
            {
                auso.PlayOneShot(clipList[2]);
                iswallJump = false;
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                doubleJump = true;
            }
            else if (doubleJump && !iswallJump)
            {
                auso.PlayOneShot(clipList[2]);
                rb.velocity = new Vector2(rb.velocity.x, playerJump * 0.75f);
                doubleJump = false;
            }
        }

        WallSlide(); // Duvara kayma

        UpdateAnimationState(); // Animasyon durumunu g�ncelle
    }

    private void FixedUpdate()
    {
        if ((isGround || dirX != 0) && !pl.IsDead && !isLevelStart)
        {
            rb.velocity = new Vector2(dirX * playerSpeed, rb.velocity.y); // Hareket etmek i�in h�z� g�ncelle
            if (rb.velocity.x != 0)
            {
                if (!auso.isPlaying && isGround)
                {
                    auso.PlayOneShot(clipList[1]);
                }
            }
            else
            {
                auso.Stop();
            }
        }
        
    }
    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsay�lan olarak idle olarak ayarla

        if (isWallSliding) // Duvara kay�yorsa (duvara kayma animasyonu)
        {
            if (isWall == 1)
            {
                sr.flipX = false;
                state = MovementState.wallSlide;
                doubleJump = true;
            }
            else if (isWall == -1)
            {
                sr.flipX = true;
                state = MovementState.wallSlide;
                doubleJump = true;
            }
        }
        else if (rb.velocity.y > .1f && doubleJump) // Y�kselme h�z� pozitif ise (z�plama animasyonu)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && !doubleJump)
        {
            state = MovementState.doubleJump;
        }
        else if (rb.velocity.y < -.1f) // Y�kselme h�z� negatif ise (d��me animasyonu)
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirX) > 0f) // Yatay giri� de�eri 0'dan b�y�k ise (ko�ma animasyonu ve y�z�n� �evirme)
        {
            state = MovementState.running;
            sr.flipX = (dirX < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        // Karakter havadaysa ve yatay giri� de�eri 0'dan b�y�k ise y�z�n� �evirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirX) > 0f)
        {
            sr.flipX = (dirX < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        anim.SetInteger("state", (int)state); // Animator'a hareket durumunu ileti
    }

    private void WallSlide()
    {
        int wallCheckResult = isWall;
        if ((wallCheckResult == 1 || wallCheckResult == -1) && !isGround && rb.velocity.y < 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private Vector2 CalculateRaycastOrigin()
    {
        return bc.bounds.center;
    }

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = CalculateRaycastOrigin();

        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast ba�lang�� noktas�
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sa� raycast ba�lang�� noktas�

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir �ey var m� kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sa� raycast'i at ve yerde bir �ey var m� kontrol et

        //// Debug �izgilerini �iz
        //Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        //Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sa� raycast'ten herhangi biri yerde bir �ey alg�lad�ysa true d�n, aksi halde false d�n
    }

    private int IsWallCheck()
    {
        Vector2 raycastOrigin = CalculateRaycastOrigin();


        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOrigin, Vector2.left, raycastExtentX + 0.1f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOrigin, Vector2.right, raycastExtentX + 0.1f, groundLayer);

        //// Debug �izgilerini �iz
        //Debug.DrawRay(raycastOrigin, Vector2.left * (raycastExtentX + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        //Debug.DrawRay(raycastOrigin, Vector2.right * (raycastExtentX + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        if (hitLeft.collider != null)
        {
            return -1;
        }
        else if (hitRight.collider != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'�n� Static olarak ayarla
        auso.PlayOneShot(clipList[0]);
        anim.Play("Player_start"); // Ba�lang�� animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        isLevelStart = false; // Level ba�lad�
        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'�n� Dynamic olarak ayarla

    }
}
