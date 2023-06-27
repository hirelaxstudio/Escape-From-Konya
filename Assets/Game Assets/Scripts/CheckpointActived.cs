using System.Collections;
using UnityEngine;

public class CheckpointActivedAnim : MonoBehaviour
{
    private Animator anim;
    private bool isCheckpointActived = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCheckpointActived)
        {
            isCheckpointActived = true;
            StartCoroutine(TriggerAnims());
        }
    }

    private IEnumerator TriggerAnims()
    {
        anim.Play("Checkpoint_active");
        yield return new WaitForSeconds(1.625f);
        anim.Play("Checkpoint_idle");
    }
}
