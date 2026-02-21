using UnityEngine;

public class Wolf_BLACKBOARD : MonoBehaviour
{
    public GameObject attractor;

    public string sheepTag = "SHEEP";
    public string bushTag = "BUSH";

    public float sheepDetectableRaidus = 6f;
    public float sheepReachedRadius = 0.7f;

    public float maxPersuingTime = 5f;

    public float restDuration = 2.5f;
    public float restingDuration = 2.5f;

    public float eatDuration = 1.0f;

    public float bushSearchRadius = 50f;
    public float hideReachedRadius = 1.2f;
}