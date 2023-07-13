using System.Collections;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    private Animator anim; 
    private BoxCollider2D bc;
    private AudioSource auso;

    [SerializeField] float brokenTime = 2f;

    private bool isTriggered = false;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        auso = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered && collision.contacts[0].normal == Vector2.down)
        {
            StartCoroutine(TriggerObject());
        }
    }

    private IEnumerator TriggerObject()
    {
        isTriggered = true;

        anim.SetTrigger("swinging");

        yield return new WaitForSeconds(2.333f);

        bc.enabled = false;

        auso.Play();
        anim.SetTrigger("breaking");

        yield return new WaitForSeconds(0.917f + brokenTime);

        anim.SetTrigger("repairing");

        yield return new WaitForSeconds(1f);

        bc.enabled = true;

        isTriggered = false;
    }
}