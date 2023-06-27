using System.Collections;
using UnityEngine;

public class CheckpointActived : MonoBehaviour
{
    private Animator anim;

    private Vector2 checkpointPosition;
    public Vector2 CheckpointPosition { get { return checkpointPosition; } }

    private bool isCheckpointActived = false;
    public bool IsCheckpointActived { get { return isCheckpointActived; } set { isCheckpointActived = value; } 
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCheckpointActived)
        {
            StartCoroutine(TriggerAnims());
            isCheckpointActived = true;
            Debug.Log("CheckpointActived: " + isCheckpointActived);
            StoreCheckpointPosition();
        }
    }

    private IEnumerator TriggerAnims()
    {
        anim.Play("Checkpoint_active");
        yield return new WaitForSeconds(1.625f);
        anim.Play("Checkpoint_idle");
    }

    private void StoreCheckpointPosition()
    {
        checkpointPosition = transform.position;
        Debug.Log("CheckpointActived Checkpoint Position: " + checkpointPosition);
    }
}
