using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
