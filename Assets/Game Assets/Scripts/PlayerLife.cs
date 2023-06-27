using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 CPPosition;
    private bool ICActived;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Debug.Log("OnCollisionEnter2D isCheckpointActived: " + ICActived);
            Debug.Log("Checkpoint Position: " + CPPosition);
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Debug.Log("OnTriggerEnter2D isCheckpointActived: " + ICActived);
            Debug.Log("Checkpoint Position: " + CPPosition);
            Die();
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Invoke("RestartLevel", 0.5f);
    }

    // ICActived ve CPPosition deðerleri dinamik olarak deðiþmediði için Chechpoint sistemi çalýþmýyor.
    private void RestartLevel()
    {
        if (ICActived)
        {
            Debug.Log("RestartLevel CP");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            gameObject.transform.position = CPPosition;
        }
        else
        {
            ICActived = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
