using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private static Vector2 checkpointPosition;
    private static bool isCheckpointActived;
    private bool isFinishLevel;
    [SerializeField] private List<AudioClip> clipList;
    private AudioSource auso;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        auso = GetComponent<AudioSource>();
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
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            isFinishLevel = true;
            isCheckpointActived = false;
            collision.gameObject.GetComponent<AudioSource>().Play();
            Invoke("LevelController", 1f);
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        auso.PlayOneShot(clipList[0]);
        anim.SetTrigger("death");
        Invoke("LevelController", 0.5f);
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