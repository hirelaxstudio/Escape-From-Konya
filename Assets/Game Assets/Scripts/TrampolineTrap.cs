using System.Collections;
using UnityEngine;

public class TrampolineTrap : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float repeatDelay = 1f;
    [SerializeField] private float jumpForce = 14f;

    private Animator anim;
    private PlayerMovement playerMovement;
    private bool isTrampolineActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();

        StartCoroutine(TriggerAnimationWithDelay());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isTrampolineActive && collision.contacts[0].normal == Vector2.down)
        {
            playerMovement.Rb.velocity = new Vector2(playerMovement.Rb.velocity.x, jumpForce);
            playerMovement.DoubleJump = true;
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