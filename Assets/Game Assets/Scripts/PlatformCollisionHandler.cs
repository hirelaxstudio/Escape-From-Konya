using UnityEngine;

public class PlatformCollisionHandler : MonoBehaviour
{
    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string result1 = (collision.gameObject.CompareTag("Platforms")) ? "Platform, " : "Not Platform, ";
        string result2 = (collision.contacts[0].normal == Vector2.up) ? "Up" : "Not Up";
        Debug.Log("Player: " + result1 + " " + result2);
        if (collision.gameObject.CompareTag("Platforms") && collision.contacts[0].normal == Vector2.up)
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
        {
            transform.SetParent(null);
        }
    }
}
