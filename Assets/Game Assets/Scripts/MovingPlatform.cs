using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint; // Baþlangýç noktasý
    [SerializeField] Transform endPoint; // Bitiþ noktasý
    [SerializeField] float duration = 3f; // Hareket süresi

    private void Start()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (endPoint != null) // endPoint null deðilse hareketi baþlat
        {
            transform.DOMove(endPoint.position, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
        {
            Debug.LogWarning("endPoint is null. Make sure to assign a valid Transform reference.");
        }
    }

    private void OnDestroy()
    {
        transform.DOKill(); // Hareketi sonlandýr
    }
}