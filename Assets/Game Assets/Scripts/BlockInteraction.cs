using System.Collections;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D bc;

    [SerializeField] float brokenTime = 2f;

    private bool isTriggered = false;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string result1 = (collision.gameObject.CompareTag("Player")) ? "Player, " : "not player, ";
        string result2 = (isTriggered) ? "triggered, " : "not triggered, ";
        string result3 = (collision.contacts[0].normal == Vector2.down) ? "down" : "not down";
        Debug.Log("Block: " + result1 + " " + result2 + " " + result3);

        if (collision.gameObject.CompareTag("Player") && !isTriggered && collision.contacts[0].normal == Vector2.down)
        {
            StartCoroutine(TriggerObject());
        }
    }

    private IEnumerator TriggerObject()
    {
        isTriggered = true;

        anim.SetTrigger("swinging"); // Block_swinging animasyonunu oynat

        yield return new WaitForSeconds(2.333f);

        bc.enabled = false; // BoxCollider2D'yi kapat

        anim.SetTrigger("breaking"); // Block_breaking animasyonunu oynat

        yield return new WaitForSeconds(0.917f + brokenTime); // Block_breaking + brokenTime kadar bekle

        anim.SetTrigger("repairing"); // Block_repaired animasyonunu oynat

        yield return new WaitForSeconds(1f); // Block_repaired kadar bekle

        bc.enabled = true; // BoxCollider2D'yi aç

        isTriggered = false;
    }
}

/*
Player üzerine bastýktan belli bir süre sonra kýrýlan bir "Block" objesi istiyorum. Bunun için aþaðýdaki gibi çalýþan bir koda ihtiyacým var:
```
Eðer Player objesi Block objesinin üst kýsmýna temas ederse:
1: Block_swinging animasyonunu oynat (Bu animasyon block'un sallanmasýný içerir)
2: BoxCollider2D'yi kapat
3: Block_swinging animasyonunu oynat (Bu animasyon block'un kýrýlýp küçük bir topa dönüþmesini içerir)
4: brokenTime kadar bekle (Bu deðer Block_repaired animasyonu oynatýlmadan önce ne kadar süre geçmesi gerektiðini belirten bir sayýdýr)
5: Block_repaired animasyonunu oynat (Bu animasyon topa dönüþmüþ block'un tekrar block'a dönüþmesini içerir)
6: BoxCollide2d'yi aç
```
*/