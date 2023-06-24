/*
// New

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; // Oyuncunun Rigidbody bile�eni
    public Rigidbody2D Rb { get { return rb; } } // Oyuncunun Rigidbody bile�enine eri�im
    private Animator anim; // Oyuncunun Animator bile�eni
    private SpriteRenderer sr; // Oyuncunun SpriteRenderer bile�eni
    private BoxCollider2D bc; // Oyuncunun BoxCollider2D bile�eni

    private float dirx; // Yatay giri� de�eri
    private float raycastExtentY; // Yatay raycast
    private float raycastExtentX; // Dikey raycast

    private bool isGrounded; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan bool de�i�keni
    private bool canJump; // Z�plama durumunu kontrol etmek i�in kullan�lan bool de�i�keni
    private bool canDoubleJump; // �ift z�plama durumunu kontrol etmek i�in kullan�lan bool de�i�keni

    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket h�z�
    [SerializeField] private float playerJump = 14f; // Oyuncunun z�plama g�c�
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan layer maskesi

    private enum MovementState { idle, running, jumping, falling, doubleJumping } // Oyuncunun hareket durumlar�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yar�s�
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yar�s� 

        StartCoroutine(LevelStart());
    }

    private void Update()
    {
        isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmad���n� kontrol et


        // Z�plama i�lemi
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

        UpdateAnimationState(); // Animasyon durumunu g�ncelle
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

        // Karakter havadaysa ve yatay giri� de�eri 0'dan b�y�k ise y�z�n� �evirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f);
        }

        anim.SetInteger("state", (int)state); // Animator'a hareket durumunu ileti
    }

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in ba�lang�� noktas�
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast ba�lang�� noktas�
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sa� raycast ba�lang�� noktas�

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir �ey var m� kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sa� raycast'i at ve yerde bir �ey var m� kontrol et

        // Debug �izgilerini �iz
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
    private Rigidbody2D rb; // Oyuncunun Rigidbody bile�eni
    public Rigidbody2D Rb { get { return rb; } } // Oyuncunun Rigidbody bile�enine eri�im
    private Animator anim; // Oyuncunun Animator bile�eni
    private SpriteRenderer sr; // Oyuncunun SpriteRenderer bile�eni
    private BoxCollider2D bc; // Oyuncunun BoxCollider2D bile�eni

    private float dirx; // Yatay giri� de�eri
    private float raycastExtentY; // Yatay raycast
    private float raycastExtentX; // Dikey raycast

    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket h�z�
    [SerializeField] private float playerJump = 14f; // Oyuncunun z�plama g�c�
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan layer maskesi

    private enum MovementState { idle, running, jumping, falling } // Oyuncunun hareket durumlar�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bile�enine eri�im
        anim = GetComponent<Animator>(); // Animator bile�enine eri�im
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer bile�enine eri�im
        bc = GetComponent<BoxCollider2D>(); // BoxCollider2D bile�enine eri�im

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yar�s�
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yar�s� 

        StartCoroutine(LevelStart()); // Leveli Ba�lat
    }

    private void Update()
    {
        bool isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmad���n� kontrol et

        // Z�plama i�lemi
        if (isGrounded && Input.GetKeyDown(KeyCode.W)) // Oyuncu yerde ve W tu�una bas�ld�ysa
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJump); // Z�pla
        }

        UpdateAnimationState(); // Animasyon durumunu g�ncelle
    }

    private void FixedUpdate()
    {
        dirx = Input.GetAxisRaw("Horizontal"); // Yatay giri� de�erini al
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y); // Hareket etmek i�in h�z� g�ncelle
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsay�lan olarak idle olarak ayarla

        if (rb.velocity.y > .1f) // Y�kselme h�z� pozitif ise (z�plama animasyonu)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f) // Y�kselme h�z� negatif ise (d��me animasyonu)
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirx) > 0f) // Yatay giri� de�eri 0'dan b�y�k ise (ko�ma animasyonu ve y�z�n� �evirme)
        {
            state = MovementState.running;
            sr.flipX = (dirx < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        // Karakter havadaysa ve yatay giri� de�eri 0'dan b�y�k ise y�z�n� �evirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        anim.SetInteger("state", (int)state); // Animator'a hareket durumunu ileti
    }

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in ba�lang�� noktas�
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast ba�lang�� noktas�
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sa� raycast ba�lang�� noktas�

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir �ey var m� kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sa� raycast'i at ve yerde bir �ey var m� kontrol et

        // Debug �izgilerini �iz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sa� raycast'ten herhangi biri yerde bir �ey alg�lad�ysa true d�n, aksi halde false d�n
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'�n� Static olarak ayarla
        anim.Play("Player_start"); // Ba�lang�� animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'�n� Dynamic olarak ayarla
    }
}

*/

/*
DJ �al���yor animasyon yok

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; // Oyuncunun Rigidbody bile�eni
    public Rigidbody2D Rb { get { return rb; } } // Oyuncunun Rigidbody bile�enine eri�im
    private Animator anim; // Oyuncunun Animator bile�eni
    private SpriteRenderer sr; // Oyuncunun SpriteRenderer bile�eni
    private BoxCollider2D bc; // Oyuncunun BoxCollider2D bile�eni

    private float dirx; // Yatay giri� de�eri
    private float raycastExtentY; // Yatay raycast
    private float raycastExtentX; // Dikey raycast
    private bool doubleJump; // �ift z�plama

    [SerializeField] private float playerSpeed = 7f; // Oyuncunun hareket h�z�
    [SerializeField] private float playerJump = 14f; // Oyuncunun z�plama g�c�
    [SerializeField] private LayerMask groundLayer; // Yerde olup olmad���n� kontrol etmek i�in kullan�lan layer maskesi

    private enum MovementState { idle, running, jumping, falling, doubleJump } // Oyuncunun hareket durumlar�

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody bile�enine eri�im
        anim = GetComponent<Animator>(); // Animator bile�enine eri�im
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer bile�enine eri�im
        bc = GetComponent<BoxCollider2D>(); // BoxCollider2D bile�enine eri�im

        raycastExtentY = bc.bounds.extents.y; // Oyuncunun Y-eksenindeki boyutunun yar�s�
        raycastExtentX = bc.bounds.extents.x; // Oyuncunun X-eksenindeki boyutunun yar�s� 

        StartCoroutine(LevelStart()); // Leveli Ba�lat
    }

    private void Update()
    {
        bool isGrounded = IsGroundedCheck(); // Oyuncunun yerde olup olmad���n� kontrol et

        dirx = Input.GetAxisRaw("Horizontal"); // Yatay giri� de�erini al

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

        UpdateAnimationState(); // Animasyon durumunu g�ncelle
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y); // Hareket etmek i�in h�z� g�ncelle
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle; // Hareket durumunu varsay�lan olarak idle olarak ayarla

        
        if (rb.velocity.y > .1f && !doubleJump) // Y�kselme h�z� pozitif ise (z�plama animasyonu)
        {
            Debug.Log("Jump");
            state = MovementState.jumping;
        }
        else if (rb.velocity.y > .1f && doubleJump)
        {
            Debug.Log("Double Jump");
            state = MovementState.doubleJump;
        }
        else if (rb.velocity.y < -.1f) // Y�kselme h�z� negatif ise (d��me animasyonu)
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirx) > 0f) // Yatay giri� de�eri 0'dan b�y�k ise (ko�ma animasyonu ve y�z�n� �evirme)
        {
            state = MovementState.running;
            sr.flipX = (dirx < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        // Karakter havadaysa ve yatay giri� de�eri 0'dan b�y�k ise y�z�n� �evirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f); // Yatay giri� de�erine g�re y�z�n� �evirme
        }

        anim.SetInteger("state", (int)state); // Animator'a hareket durumunu ileti
    }

    private bool IsGroundedCheck()
    {
        Vector2 raycastOrigin = bc.bounds.center; // Raycast'in ba�lang�� noktas�
        Vector2 raycastOriginLeft = new Vector2(raycastOrigin.x - raycastExtentX, raycastOrigin.y); // Sol raycast ba�lang�� noktas�
        Vector2 raycastOriginRight = new Vector2(raycastOrigin.x + raycastExtentX, raycastOrigin.y); // Sa� raycast ba�lang�� noktas�

        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sol raycast'i at ve yerde bir �ey var m� kontrol et
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, raycastExtentY + 0.1f, groundLayer); // Sa� raycast'i at ve yerde bir �ey var m� kontrol et

        // Debug �izgilerini �iz
        Debug.DrawRay(raycastOriginLeft, Vector2.down * (raycastExtentY + 0.1f), hitLeft.collider != null ? Color.green : Color.red);
        Debug.DrawRay(raycastOriginRight, Vector2.down * (raycastExtentY + 0.1f), hitRight.collider != null ? Color.green : Color.red);

        return hitLeft.collider != null || hitRight.collider != null; // Sol veya sa� raycast'ten herhangi biri yerde bir �ey alg�lad�ysa true d�n, aksi halde false d�n
    }

    private IEnumerator LevelStart()
    {
        rb.bodyType = RigidbodyType2D.Static; // Rigidbody'nin body type'�n� Static olarak ayarla
        anim.Play("Player_start"); // Ba�lang�� animasyonunu oynat

        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody'nin body type'�n� Dynamic olarak ayarla
    }
}

*/