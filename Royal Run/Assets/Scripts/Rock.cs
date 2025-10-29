using Unity.Cinemachine;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AudioSource boulderSmashAudioSource;
    [SerializeField] float shakeModifier = 10f;
    CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] float collisionCooldown = 1f;
    float collisionTimer = 0f;
     void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

     void Update()
    {
        collisionTimer += Time.deltaTime;    
    }

    void OnCollisionEnter(Collision other)
    {
        if (collisionTimer < collisionCooldown) return;
        FireImpulse();
        CollisionFX(other);
        collisionTimer = 0f;
    }

    private void FireImpulse()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntesity = 1f / distance * shakeModifier;
        shakeIntesity = Mathf.Min(shakeIntesity, 1f);
        cinemachineImpulseSource.GenerateImpulse(shakeIntesity);
    }

    void CollisionFX(Collision other)
    {
        ContactPoint contactPoint = other.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        boulderSmashAudioSource.Play();
    }
}
