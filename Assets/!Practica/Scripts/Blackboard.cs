using System.Threading;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    //GENERAL--------------------------------------------------------

    public GameObject dog;
    public GameObject wolf;

    //UI----------------------------------------------------------
    public float timer = 120;

    public float SheepAlive = 10;
    public float SheepDeath = 10;
    public float SheepSaved = 10;

    //PERRO----------------------------------------------------------
    public float gutDogRadius = 100;

    //OVEJA----------------------------------------------------------

    public GameObject recinto;
    public float recintoRadius = 50;

    public float speedDebuff = 0.2f;
    public float initialSpeed = 20;
    public float dogInRadius = 100f;
    public float wolfInRadius = 100f;
    public float speedMultiplier = 0.7f;
    public float timerMuerte = 0f;
    public float deathTime = 3f;
    public float seekMultiplier = 2.0f;

    //LOVO----------------------------------------------------------
    public float gutMadrigueraRadius = 150;
    public float gutWolfaRadius = 150;

    public float movSprint = 5f;

    private void Start()
    {

    }

    
}
