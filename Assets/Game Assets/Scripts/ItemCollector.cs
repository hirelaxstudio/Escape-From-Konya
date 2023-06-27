using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable items"))
        {
            collision.gameObject.GetComponent<Animator>().Play("Cherry_disappear");

            Destroy(collision.gameObject, 0.3f);

            Score.score++;
            scoreText.text = "Score: " + Score.score;
        }
    }
}
