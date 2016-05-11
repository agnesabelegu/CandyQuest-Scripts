using UnityEngine;
public class MineExplosionScript : MonoBehaviour
{
    //Public
    public float ExplosionForce;
    public float ExplosionRadius;

    //Hidden in inspector
    [HideInInspector]
    public Vector3 ExplosionStartLocation;
    [HideInInspector]
    public bool Exploded;

    //Private
    private AudioSource[] AudioSources;
    private ParticleSystem ParticleSystem;

    //Constants
    private const float TimeToSelfDestruct = 2.0f;

    void Start()
    {
        AudioSources = GetComponents<AudioSource>();
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Exploded)
        {
            Destroy(gameObject, TimeToSelfDestruct);
        }
    }

    void OnTriggerEnter(Collider activator)
    {
        if (!Exploded)
        {
            Collider[] colliders = Physics.OverlapSphere(ExplosionStartLocation, ExplosionRadius);

            foreach (Collider collider in colliders)
            {
                if (!collider)
                    continue;
                if (!collider.tag.Equals("Vegetable"))
                    continue;
                if (collider.GetComponent<Rigidbody>())
                {
                    AudioSources[0].Play();

                    ImpactReceiver impactReceiver = collider.transform.GetComponent<ImpactReceiver>();
                    if (impactReceiver)
                    {
                        Vector3 dir = collider.transform.position - transform.position;
                        float force = Mathf.Clamp(ExplosionForce / 3, 0, 1000);
                        impactReceiver.AddImpact(dir, force);
                    }
                    Exploded = true;
                    ParticleSystem.Play();
                }
            }
            AudioSources[1].Play();
        }
    }
}
 