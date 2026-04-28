using UnityEngine;

public enum ObstacleType
{
    Normal,
    Tall,
    Low
}

public class Obstacle : MonoBehaviour
{
    public ObstacleType type;
}