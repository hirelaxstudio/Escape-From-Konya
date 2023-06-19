using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
    }

    // 2. Die() metodu
    //private void Die()
    //{
    //    rb.bodyType = RigidbodyType2D.Static;
    //    anim.SetTrigger("death");
    //    StartCoroutine(RestartLevelCoroutine());
    //}

    //private IEnumerator RestartLevelCoroutine()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    // 1. Die() metodu
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Invoke("RestartLevel", 0.5f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
