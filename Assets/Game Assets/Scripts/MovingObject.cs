using UnityEngine;
using DG.Tweening;

public class MovingObject : MonoBehaviour
{
    [SerializeField] Transform startPoint; // Baþlangýç noktasý
    [SerializeField] Transform endPoint; // Bitiþ noktasý
    [SerializeField] float duration = 3f; // Hareket süresi

    private void Start()
    {
        MoveObject();
    }

    private void MoveObject()
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