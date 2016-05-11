using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapTypes
{
    VerticalTrap,
    HorizontalTrap,
    ExplosionTrap
}

public class InteractionController : MonoBehaviour
{
    //Public
    public float ReachingDistance;
    public float CooldownTime = 5.0f;
    public GameObject TrapCube;
    public GameObject MineCube;
    public Candy_GameManager GameManager;

    //Audio
    public AudioClip[] MunchingSFX;
    public AudioClip[] DiggingSFX;
    public AudioClip[] TrapSFX;
    public AudioClip CooldownOn;
    public AudioClip CooldownOff;
    public AudioSource TrapSound;

    //Private
    private GameObject ObjectHit;
    private AudioSource PlayerSound;
    private bool TrapOnCooldown;

    //Constants
    private const int MaxVerticalTrapLength = 4;
    private const int MaxVerticalTrapWidth = 2;
    private const int MaxHorizontalTrapLength = 2;
    private const int MaxHorizontalTrapWidth = 4;

    // Use this for initialization
    void Start()
    {
        PlayerSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ObjectHit = GetGameObjectClicked();

            if (ObjectHit != null)
            {
                //If in the EdibleCube Layer, then add the happiness value of that cube to the Player, and play Munching sound effects.
                //Also, deduct EdibleCube health points from cube with every click.
                if (ObjectHit.layer == 8)
                {
                    EdibleCubeHealthScript edibleCubeHealth = ObjectHit.GetComponent<EdibleCubeHealthScript>();

                    edibleCubeHealth.DeductHealthPoints();

                    GameManager.ChangeHappinessLevel(edibleCubeHealth.HappinessValue);
                    PlayerSound.clip = MunchingSFX[Random.Range(0, MunchingSFX.Length)];
                    PlayerSound.Play();

                }
                //If in the default Cube Layer, deduct cube's health points with every click, and play Digging sound with every click.
                if (ObjectHit.layer == 0)
                {
                    CubeHealthScript cubeHealth = ObjectHit.GetComponent<CubeHealthScript>();

                    cubeHealth.DeductHealthPoints();
                    PlayerSound.clip = DiggingSFX[Random.Range(0, DiggingSFX.Length)];
                    PlayerSound.Play();
                }
            }
        }


        if (Input.GetButtonDown("VerticalTrap"))
        {
            //Checks of trap is on cooldown.
            if (!TrapOnCooldown)
            {
                ObjectHit = GetGameObjectClicked();
                if (ObjectHit != null)
                {
                    GenerateTrap(ObjectHit.transform.position + ObjectHit.transform.up, TrapTypes.VerticalTrap);
                }
            }
            else
            {
                TrapSound.clip = CooldownOn;
                TrapSound.Play();
            }
        }

        if (Input.GetButtonDown("HorizontalTrap"))
        {
            //Checks of trap is on cooldown.
            if (!TrapOnCooldown)
            {
                ObjectHit = GetGameObjectClicked();
                if (ObjectHit != null)
                {
                    GenerateTrap(ObjectHit.transform.position + ObjectHit.transform.up, TrapTypes.HorizontalTrap);
                }
            }
            else
            {
                TrapSound.clip = CooldownOn;
                TrapSound.Play();
            }
        }

        if (Input.GetButtonDown("MineTrap"))
        {
            //Checks of trap is on cooldown.
            if (!TrapOnCooldown)
            {
                ObjectHit = GetGameObjectClicked();
                if (ObjectHit != null)
                {
                    GenerateTrap(ObjectHit.transform.position + ObjectHit.transform.up, TrapTypes.ExplosionTrap);
                }
            }
            else
            {
                TrapSound.clip = CooldownOn;
                TrapSound.Play();
            }
        }
    }

    /// <summary>
    /// Casts Raycast out to the ReachingDistance from the camera and retrieves GameObject player it hits.
    /// <returns>GameObject RayCast hit</returns>
    public GameObject GetGameObjectClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ReachingDistance))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Casts Raycast out to the ReachingDistance from the camera and retrieves GameObject player it hits.
    /// <returns>GameObject RayCast hit</returns>
    public void GenerateTrap(Vector3 spawnPosition, TrapTypes trapType)
    {
        int maxTrapLength = 0;
        int maxTrapWidth = 0;

        if (trapType == TrapTypes.ExplosionTrap)
        {
            GameObject mineCubeSpawned = Instantiate(MineCube, spawnPosition, Quaternion.identity) as GameObject;
            mineCubeSpawned.GetComponent<MineExplosionScript>().ExplosionStartLocation = spawnPosition;
        }
        else
        {
            if (trapType == TrapTypes.VerticalTrap)
            {
                maxTrapLength = MaxVerticalTrapLength;
                maxTrapWidth = MaxVerticalTrapWidth;
            }
            else if (trapType == TrapTypes.HorizontalTrap)
            {
                maxTrapLength = MaxHorizontalTrapLength;
                maxTrapWidth = MaxHorizontalTrapWidth;
            }

            // Get a copy of the forward vector
            Vector3 forward = transform.forward;
            // Zero out the y component of the forward vector to only get the direction in the X,Z plane
            forward.y = 0;

            Quaternion characterRotation = Quaternion.LookRotation(forward);
            GameObject bottomCube = Instantiate(TrapCube, spawnPosition, characterRotation) as GameObject;

            for (int i = 0; i < maxTrapWidth; i++)
            {
                if (i != 0)
                {
                    bottomCube = Instantiate(TrapCube, bottomCube.transform.position + bottomCube.transform.right, characterRotation) as GameObject;
                }

                for (int j = 0; j < maxTrapLength; j++)
                {
                    Instantiate(TrapCube, bottomCube.transform.position + new Vector3(0.0f, j, 0.0f), characterRotation);
                }
            }
        }
        PlayerSound.clip = TrapSFX[Random.Range(0, TrapSFX.Length)];
        PlayerSound.Play();
        TrapOnCooldown = true;
        StartCoroutine(TrapCooldown());
    }
    
    //Trap cooldown
    protected IEnumerator TrapCooldown()
    {
        yield return new WaitForSeconds(CooldownTime);

        TrapOnCooldown = false;
        TrapSound.clip = CooldownOff;
        TrapSound.Play();
    }
}
