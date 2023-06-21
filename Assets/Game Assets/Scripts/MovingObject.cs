using UnityEngine;
using DG.Tweening;

public class MovingObject : MonoBehaviour
{
    [SerializeField] Transform startPoint; // Ba�lang�� noktas�
    [SerializeField] Transform endPoint; // Biti� noktas�
    [SerializeField] float duration = 3f; // Hareket s�resi

    private void Start()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        if (endPoint != null) // endPoint null de�ilse hareketi ba�lat
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
        transform.DOKill(); // Hareketi sonland�r
    }
}