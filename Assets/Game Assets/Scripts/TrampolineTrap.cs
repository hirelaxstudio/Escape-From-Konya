using System.Collections;
using System.Reflection;
using UnityEngine;

public class TrampolineTrap : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float repeatDelay = 1f;
    [SerializeField] private float jumpForce = 14f;

    private Animator anim;
    private Rigidbody2D playerRB;
    private bool isTrampolineActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRB = GameObject.FindObjectOfType<PlayerMovement>().Rb;

        StartCoroutine(TriggerAnimationWithDelay());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        Trambolinin yanlar�na temas ederken de z�plat�yor. Bunu istemiyorum.
        "collision.contacts[0].normal == Vector2.up" kodu PlatformCollisionHandler'da �al��t�
        ama burada �al��mad�. ��z�m olarak raycast kullanmay� d���nd�m ama PlayerMovement'da
        da raycast kulland���m i�in 2 raycast performans sorunu ya�ar� diye d���n�yorum. Bu y�zden
        raycast'i ayr� bir script olarak yaz�p hem hitLeft.collider != null && hitLeft.collider.CompareTag("Trampolines")
        �eklinde Tag'lar� kontrol edebilecek hem hitLeft.collider != null && hitLeft.collider.gameObject.layer == groundLayer
        �eklinde layer'lar� kontrol edebilecek �ekilde yazmay� d���n�yorum ama nas�l yazar�m bilmiyorum.
        */
        string result1 = (collision.gameObject.CompareTag("Player")) ? "Player, " : "not player, ";
        string result2 = (isTrampolineActive) ? "active, " : "not active, ";
        string result3 = (collision.contacts[0].normal == Vector2.down) ? "down" : "not down";
        Debug.Log("Trambolin: " + result1 + " " + result2 + " " + result3);

        if (collision.gameObject.CompareTag("Player") && isTrampolineActive && collision.contacts[0].normal == Vector2.down)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        yield return new WaitForSeconds(startDelay); // startDelay kadar bekle

        while (true)
        {
            isTrampolineActive = true; // Trambolin aktif olsun

            anim.SetTrigger("idle"); // "idle" animasyonunu tetikle

            yield return new WaitForSeconds(0.5f); // Animasyonun uzunlu�u kadar bekle

            isTrampolineActive = false; // Trambolin aktif olmas�n

            yield return new WaitForSeconds(repeatDelay); // repeatDelay kadar bekle
        }
    }
}