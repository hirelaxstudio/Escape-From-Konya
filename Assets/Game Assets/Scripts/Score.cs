using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score { get; set; }
    private void Start()
    {
        score = 0;
    }
}
