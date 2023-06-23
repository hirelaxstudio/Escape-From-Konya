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
        Trambolinin yanlarýna temas ederken de zýplatýyor. Bunu istemiyorum.
        "collision.contacts[0].normal == Vector2.up" kodu PlatformCollisionHandler'da çalýþtý
        ama burada çalýþmadý. Çözüm olarak raycast kullanmayý düþündüm ama PlayerMovement'da
        da raycast kullandýðým için 2 raycast performans sorunu yaþarý diye düþünüyorum. Bu yüzden
        raycast'i ayrý bir script olarak yazýp hem hitLeft.collider != null && hitLeft.collider.CompareTag("Trampolines")
        þeklinde Tag'larý kontrol edebilecek hem hitLeft.collider != null && hitLeft.collider.gameObject.layer == groundLayer
        þeklinde layer'larý kontrol edebilecek þekilde yazmayý düþünüyorum ama nasýl yazarým bilmiyorum.
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

            yield return new WaitForSeconds(0.5f); // Animasyonun uzunluðu kadar bekle

            isTrampolineActive = false; // Trambolin aktif olmasýn

            yield return new WaitForSeconds(repeatDelay); // repeatDelay kadar bekle
        }
    }
}