using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;
    const string hitString = "Hit";
    float cooldownTimer = 0f;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (cooldownTimer < collisionCooldown) return;

        animator.SetTrigger(hitString);
        cooldownTimer = 0f;
    }
}
