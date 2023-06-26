using System.Collections;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; // Oyuncunun Rigidbody bileþeni
    public Rigidbody2D Rb { get { return rb; } } // Oyuncunun Rigidbody bileþenine eriþim
    private Animator anim; // Oyuncunun Animator bileþeni
    private SpriteRenderer sr; // Oyuncunun SpriteRenderer bileþeni
    private BoxCollider2D bc; // Oyuncunun BoxCollider2D bileþeni

    private float dirx; // Yatay giriþ deðeri
    private float raycastExtentY; // Yatay raycast
    private float raycastExtentX; // Dikey raycast
    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket hýzý
    [SerializeField] private float playerJump = 14f; // Oyuncunun zýplama gücü
    [SerializeField] private float wallSlideSpeed = 2f; // Duvara kayma hýzý
    [SerializeField] private float wallJumpForce = 10f; // Duvara zýplama gücü

    private int isWall; // Duvara çarpýp çarpmadýðýný kontrol etmek için kullanýlan int

    private bool isGround; // Yerde olup olmadýðýný kontrol etmek için kullanýlan bool
    private bool isWallSliding; // Duvara kayýp kaymadýðýný kontrol etmek için kullanýlan bool
    private bool doubleJump; // Çift zýplama
    private bool iswallJump;

    public bool DoubleJump { get { return doubleJump; } set { doubleJump = value; } } // doubleJump deðiþkenine eriþim ve deðiþtirme

    [SerializeField] private LayerMask groundLayer; // Yerde olup olmadýðýný kontrol etmek için kullanýlan layer maskesi

    private enum MovementState { idle, running, jumping, falling, doubleJump, wallSlide } // Oyuncunun hareket durumlarý

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bileþenine eriþim
        anim = GetComponent<Animator>(); // Animator bileþenine eriþim
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer bileþenine eriþim
        bc = GetComponent<BoxCollider2D>(); // BoxCollider2D bileþenine eriþim

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yarýsý
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yarýsý 

        StartCoroutine(LevelStart()); // Leveli Baþlat
    }

    private void Update()
    {
        isGround = IsGroundedCheck(); // Oyuncunun yerde olup olmadýðýný kontrol et
        isWall = IsWallCheck(); // Oyuncunun duvara çarpýp çarpmadýðýný kontrol et

        dirx = Input.GetAxisRaw("Horizontal"); // Yatay giriþ deðerini al

        if (Input.GetKeyDown(KeyCode.W))
        {
            // Burada kaldýn. walljump yaparken doublejump yapmamasýný saðlaman lazým.
            if (isWallSliding)
            {
                iswallJump = true;
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                rb.AddForce(new Vector2(-isWall * wallJumpForce, 0), ForceMode2D.Impulse);
            }
            else if (isGround)
            {
                iswallJump = false;
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                doubleJump = true;
            }
            else if (doubleJump && !iswallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerJump * 0.75f);
                doubleJump = false;
            }
        }

        WallSlide(); // Duvara kayma

        UpdateAnimationState(); // Animasyon durumunu güncelle
    }

    private void FixedUpdate()
    {
        if (isGround || dirx != 0)
        {
            rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y); // Hareket etmek için hýzý güncelle
        }
        
    }
    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsayýlan olarak idle olarak ayarla

        if (isWallSliding) // Duvara kayýyorsa (duvara kayma animasyonu)
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
        else if (rb.velocity.y > .1f && doubleJump) // Yükselme hýzý pozitif ise (zýplama animasyonu)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && !doubleJump)
        {
            state = MovementState.doubleJump;
        }
        else if (rb.velocity.y < -.1f) // Yükselme hýzý negatif ise (düþme animasyonu)
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirx) > 0f) // Yatay giriþ deðeri 0'dan büyük ise (koþma animasyonu ve yüzünü çevirme)
        {
            state = MovementState.running;
            sr.flipX = (dirx < 0f); // Yatay giriþ deðerine göre yüzünü çevirme
        }

        // Karakter havadaysa ve yatay giriþ deðeri 0'dan büyük ise yüzünü çevirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f); // Yatay giriþ deðerine göre yüzünü çevirme
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

        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast baþlangýç noktasý
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sað raycast baþlangýç noktasý

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir þey var mý kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sað raycast'i at ve yerde bir þey var mý kontrol et

        // Debug çizgilerini çiz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sað raycast'ten herhangi biri yerde bir þey algýladýysa true dön, aksi halde false dön
    }

    private int IsWallCheck()
    {
        Vector2 raycastOrigin = CalculateRaycastOrigin();


        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOrigin, Vector2.left, raycastExtentX + 0.1f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOrigin, Vector2.right, raycastExtentX + 0.1f, groundLayer);

        // Debug çizgilerini çiz
        Debug.DrawRay(raycastOrigin, Vector2.left * (raycastExtentX + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOrigin, Vector2.right * (raycastExtentX + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        if (hitLeft.collider != null)
        {
            Debug.Log("Hit Left");
            return -1;
        }
        else if (hitRight.collider != null)
        {
            Debug.Log("Hit Right");
            return 1;
        }
        else
        {
            Debug.Log("Hit None");
            return 0;
        }
    }


    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'ýný Static olarak ayarla
        anim.Play("Player_start"); // Baþlangýç animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'ýný Dynamic olarak ayarla
    }
}
