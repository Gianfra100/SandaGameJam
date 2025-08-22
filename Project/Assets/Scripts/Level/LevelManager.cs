using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public  Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Level Time")]
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
}
