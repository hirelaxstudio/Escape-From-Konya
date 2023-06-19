using UnityEngine;

public class PlatformCollisionHandler : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms") && collision.contacts[0].normal == Vector2.up)
        {
            player.transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
        {
            player.transform.SetParent(null);
        }
    }
}
