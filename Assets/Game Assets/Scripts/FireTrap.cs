using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float repeatDelay = 1f;

    private Animator anim;
    private BoxCollider2D bc;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();

        StartCoroutine(TriggerAnimationWithDelay());
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        yield return new WaitForSeconds(startDelay); // startDelay kadar bekle

        while (true)
        {
            anim.SetTrigger("idle"); // "idle" animasyonunu tetikle

            bc.enabled = true; // BoxCollider'ý aktif et

            yield return new WaitForSeconds(0.667f); // Animasyonun uzunluðu kadar bekle

            bc.enabled = false; // BoxCollider'ý pasif et

            yield return new WaitForSeconds(repeatDelay); // repeatDelay kadar bekle
        }
    }
}
