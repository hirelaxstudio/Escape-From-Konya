using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int score = 0;

    [SerializeField] private Text scoreText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable items"))
        {
            Destroy(collision.gameObject);
            score++;
            scoreText.text = "Score: " + score;
        }
    }
}
