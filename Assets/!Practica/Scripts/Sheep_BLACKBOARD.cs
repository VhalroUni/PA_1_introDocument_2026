using UnityEngine;

public class Sheep_BLACKBOARD : MonoBehaviour
{
    public GameObject centerScreen;
    public GameObject centerSafeZone;
    public GameObject safeZone;

    public float initialSpeed = 1f;
    public float closeEnoughRadius = 0.5f;
    public float safeZoneRadius = 5f;
    public float dogInRadius = 10f;
    public float wolfInRadius = 1f;

    public float attractorMultiplier = 3f;

    public float deathTime = 3f;
    public float deathTimer = 0f;

}
