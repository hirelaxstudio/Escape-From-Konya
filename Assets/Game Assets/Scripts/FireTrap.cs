using System.Collections;
using UnityEngine;

/*
sD ba�l�yo
sD bitiyo
fire anim ba�l�yo
fire anim bitiyo
fire box collider kapan�yo
repeatDelay ba�l�yo
repeatDelay bitiyo
fire box collider a��l�yo
REPEAT
 */
public class FireTrap : MonoBehaviour
{
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float repeatDelay = 1f;

    private Animator anim;
    private BoxCollider2D bc;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();

        StartCoroutine(TriggerAnimationWithDelay());
    }

    private IEnumerator TriggerAnimationWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            anim.SetTrigger("fire"); // "fire" animasyonunu tetikle

            bc.enabled = true; // Box collider'� aktif et

            yield return new WaitForSeconds(0.667f); // Animasyonun uzunlu�u kadar bekle

            bc.enabled = false; // Box collider'� pasif et

            yield return new WaitForSeconds(repeatDelay);
        }
    }
}
