using UnityEngine;
using System.Collections;

public enum EdibleCubes
{
    SoftCandy = 0,
    MediumCandy = 1,
    HardCandy = 2,
    Vegetable = 3
}

public class EdibleCubeHealthScript : MonoBehaviour
{
    //Public
    public int HealthPoints;
    public int HappinessLevelMultiplier = 5;
    public EdibleCubes CubeType;

    //Textures
    public Texture Damaged;
    public Texture Dying;

    //Hidden in inspector
    [HideInInspector]
    public int HappinessValue;

    //Private
    private Renderer mRenderer;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
        
        switch (CubeType)
        {
            case EdibleCubes.SoftCandy:
                HealthPoints = 1;
                break;
            case EdibleCubes.Vegetable:
                HealthPoints = 1;
                break;
            case EdibleCubes.MediumCandy:
                HealthPoints = 2;
                break;
            case EdibleCubes.HardCandy:
                HealthPoints = 3;
                break;
        }

        HappinessValue = HealthPoints * HappinessLevelMultiplier;
    }

    /// <summary>
    /// Deducts health points and changes Cube texture depending on Cube Health
    /// </summary>
    public void DeductHealthPoints()
    {
        HealthPoints--;

        if (HealthPoints == 0)
        {
            MinecraftLevelGenerator.RevealCubesNearby(gameObject);
            Destroy(gameObject);
        }
        if (HealthPoints == 1)
        {
            mRenderer.material.SetTexture("_MainTex", Dying);
        }
        else if (HealthPoints == 2)
        {
            mRenderer.material.SetTexture("_MainTex", Damaged);
        }
    }
}
