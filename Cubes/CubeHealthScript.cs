using UnityEngine;
using System.Collections;

public class CubeHealthScript : MonoBehaviour
{
    //Public
    public int HealthPoints;
    public Texture Damaged;
    public Texture Dying;

    //Private
    private Renderer mRenderer;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
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
