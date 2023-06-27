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
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            anim.SetTrigger("idle");

            bc.enabled = true;

            yield return new WaitForSeconds(0.667f);

            bc.enabled = false;

            yield return new WaitForSeconds(repeatDelay);
        }
    }
}
