using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource auso;

    private static Vector2 checkpointPosition;
    private static Vector2 oldCheckpointPosition;
    private static bool isCheckpointActived;
    private bool isFinishLevel;
    private bool isDead;
    public bool IsDead { get { return isDead; } }

    [SerializeField] private AudioClip playerDeathAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        auso = GetComponent<AudioSource>();
        isFinishLevel = false;
        isDead = false;
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

            if (isCheckpointActived && ((checkpointPosition != oldCheckpointPosition) || checkpointPosition == Vector2.zero))
            {
                collision.gameObject.GetComponent<AudioSource>().Play();
            }

            oldCheckpointPosition = checkpointPosition;

        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            isFinishLevel = true;
            isCheckpointActived = false;
            collision.gameObject.GetComponent<AudioSource>().Play();
            Invoke("LevelController", 0.5f);
        }
    }

    private void Die()
    {
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        auso.PlayOneShot(playerDeathAudio);
        anim.SetTrigger("death");
        Invoke("LevelController", 0.8f);
    }

    private void LevelController()
    {
        if (isCheckpointActived)
        {
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