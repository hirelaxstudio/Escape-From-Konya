using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    private Animator anim;

    private bool isLevelComplate = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isLevelComplate)
        {
            anim.Play("End_trigger");
            isLevelComplate = true;
            Invoke("ComplateLevel", 1f);
        }
    }

    private void ComplateLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
