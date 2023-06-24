/*
// New

using System.Collections;
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

    private bool isGrounded; // Yerde olup olmadýðýný kontrol etmek için kullanýlan bool deðiþkeni
    private bool canJump; // Zýplama durumunu kontrol etmek için kullanýlan bool deðiþkeni
    private bool canDoubleJump; // Çift zýplama durumunu kontrol etmek için kullanýlan bool deðiþkeni

    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket hýzý
    [SerializeField] private float playerJump = 14f; // Oyuncunun zýplama gücü
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmadýðýný kontrol etmek için kullanýlan layer maskesi

    private enum MovementState { idle, running, jumping, falling, doubleJumping } // Oyuncunun hareket durumlarý

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yarýsý
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yarýsý 

        StartCoroutine(LevelStart());
    }

    private void Update()
    {
        isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmadýðýný kontrol et


        // Zýplama iþlemi
        if (isGrounded)
        {
            canJump = true;
            canDoubleJump = false;
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.W) && canJump)
        {
            Debug.Log("Jump");
            Jump();

        }
        else if (!isGrounded && Input.GetKeyDown(KeyCode.W) && canDoubleJump)
        {
            Debug.Log("Double Jump");
            Jump();
        }

        UpdateAnimationState(); // Animasyon durumunu güncelle
    }

    private void FixedUpdate()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y);
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;

        if (rb.velocity.y > .1f)
        {
            if (!canJump;, canDoubleJump;)
            {
                Debug.Log("canjump");
                state = MovementState.jumping;
            }
            else if (canDoubleJump)
            {
                Debug.Log("candoublejump");
                state = MovementState.doubleJumping;
            }

        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirx) > 0f)
        {
            state = MovementState.running;
            sr.flipX = (dirx < 0f);
        }

        // Karakter havadaysa ve yatay giriþ deðeri 0'dan büyük ise yüzünü çevirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f);
        }

        anim.SetInteger("state", (int)state); // Animator'a hareket durumunu ileti
    }

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in baþlangýç noktasý
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast baþlangýç noktasý
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sað raycast baþlangýç noktasý

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir þey var mý kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sað raycast'i at ve yerde bir þey var mý kontrol et

        // Debug çizgilerini çiz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null || rb.velocity.y == 0f;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerJump);
        if (canJump)
        {
            canJump = false;
            canDoubleJump = true;
        }
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.Play("Player_start");

        yield return new WaitForSeconds(0.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
*/

/*
// Old

using System.Collections;
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
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmadýðýný kontrol etmek için kullanýlan layer maskesi

    private enum MovementState { idle, running, jumping, falling } // Oyuncunun hareket durumlarý

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
        bool isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmadýðýný kontrol et

        // Zýplama iþlemi
        if (isGrounded && Input.GetKeyDown(KeyCode.W)) // Oyuncu yerde ve W tuþuna basýldýysa
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJump); // Zýpla
        }

        UpdateAnimationState(); // Animasyon durumunu güncelle
    }

    private void FixedUpdate()
    {
        dirx = Input.GetAxisRaw("Horizontal"); // Yatay giriþ deðerini al
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y); // Hareket etmek için hýzý güncelle
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsayýlan olarak idle olarak ayarla

        if (rb.velocity.y > .1f) // Yükselme hýzý pozitif ise (zýplama animasyonu)
        {
            state = MovementState.jumping;
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

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in baþlangýç noktasý
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast baþlangýç noktasý
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sað raycast baþlangýç noktasý

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir þey var mý kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sað raycast'i at ve yerde bir þey var mý kontrol et

        // Debug çizgilerini çiz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sað raycast'ten herhangi biri yerde bir þey algýladýysa true dön, aksi halde false dön
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'ýný Static olarak ayarla
        anim.Play("Player_start"); // Baþlangýç animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'ýný Dynamic olarak ayarla
    }
}

*/

/*
DJ çalýþýyor animasyon yok

using System.Collections;
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
    private bool doubleJump; // Çift zýplama

    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket hýzý
    [SerializeField] private float playerJump = 14f; // Oyuncunun zýplama gücü
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmadýðýný kontrol etmek için kullanýlan layer maskesi

    private enum MovementState { idle, running, jumping, falling, doubleJump } // Oyuncunun hareket durumlarý

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
        bool isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmadýðýný kontrol et

        dirx = Input.GetAxisRaw("Horizontal"); // Yatay giriþ deðerini al

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerJump);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerJump * 0.75f);
                doubleJump = false;
            }
        }

        UpdateAnimationState(); // Animasyon durumunu güncelle
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y); // Hareket etmek için hýzý güncelle
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsayýlan olarak idle olarak ayarla

        
        if (rb.velocity.y > .1f && !doubleJump) // Yükselme hýzý pozitif ise (zýplama animasyonu)
        {
            Debug.Log("Jump");
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && doubleJump)
        {
            Debug.Log("Double Jump");
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

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in baþlangýç noktasý
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast baþlangýç noktasý
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sað raycast baþlangýç noktasý

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir þey var mý kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sað raycast'i at ve yerde bir þey var mý kontrol et

        // Debug çizgilerini çiz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sað raycast'ten herhangi biri yerde bir þey algýladýysa true dön, aksi halde false dön
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'ýný Static olarak ayarla
        anim.Play("Player_start"); // Baþlangýç animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'ýný Dynamic olarak ayarla
    }
}

*/