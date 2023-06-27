using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private static Vector2 checkpointPosition;
    private static bool isCheckpointActived;
    private bool isFinishLevel;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isFinishLevel = false;
        if (isCheckpointActived)
        {
            gameObject.transform.position = checkpointPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Checkpoints"))
        {
            checkpointPosition = collision.gameObject.transform.position;
            isCheckpointActived = true;
            Debug.Log("Checkpoint Position: " + checkpointPosition);
            Debug.Log("isCheckpointActived: " + isCheckpointActived);
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            isFinishLevel = true;
            isCheckpointActived = false;
            Invoke("LevelController", 1f);
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Invoke("LevelController", 0.5f);
    }

    private void LevelController()
    {
        if (isCheckpointActived)
        {
            Debug.Log("RestartLevel CP");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (isFinishLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } 
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}