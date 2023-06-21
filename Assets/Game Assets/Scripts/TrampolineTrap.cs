using System.Collections;
using UnityEngine;

public class TrampolineTrap : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float repeatDelay = 1f;
    [SerializeField] private float jumpForce = 14f;

    private Animator anim;
    private GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(TriggerAnimationWithDelay());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms") && collision.contacts[0].normal == Vector2.up)
        {
            player.transform.SetParent(collision.transform);
        }
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        yield return new WaitForSeconds(startDelay); // startDelay kadar bekle

        while (true)
        {
            anim.SetTrigger("idle"); // "idle" animasyonunu tetikle

            yield return new WaitForSeconds(0.667f); // Animasyonun uzunlu�u kadar bekle

            yield return new WaitForSeconds(repeatDelay); // repeatDelay kadar bekle
        }
    }
}
/*
PlatformCollisionHandler scripti Platform objesine component eklendi�i i�in unity "collision" bunun Platform objesi oldu�unu bilmiyormu zaten? Neden "collision.gameObject.CompareTag("Platforms")" �eklinde kontrol etme ihtiyac� duyuyor? Galiba ben OnCollisionEnter2D anlamad���m i�in bu soruyu sordum. Unity Game Developer gibi davran. Unity'e yeni ba�layan birisinin anlayabilece�i bir �ekilde anlat.
*/