using UnityEngine;

/// <summary>
/// Place this script on a trap GameObject with a Collider set to "Is Trigger".
/// Enemies need a script/component tagged "Enemy" or a Health component to take damage.
/// </summary>
public class Trap : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage dealt to the enemy on contact.")]
    public float damage = 100f;

    [Tooltip("If true, the trap destroys itself after one use.")]
    public bool destroyOnUse = false;

    [Tooltip("Cooldown in seconds between activations (0 = no cooldown).")]
    public float cooldown = 0f;

    [Header("Visual Feedback")]
    [Tooltip("Optional particle effect spawned when the trap fires.")]
    public GameObject triggerEffect;

    private bool isOnCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only react to objects tagged "Enemy"
        if (!other.CompareTag("Enemy")) return;
        Debug.Log("zombie 1");
        if (isOnCooldown) return;

        // Try to find a Health component on the enemy or its parent
        Zombie health = other.GetComponent<Zombie>()
                     ?? other.GetComponentInParent<Zombie>();

        if (health != null)
        {
            Debug.Log("zombie 2");
            health.TakeDamage((int)damage);
        }
        else
        {
            Debug.Log("zombie 3");
            // Fallback: destroy the enemy directly if no Health script found
            Debug.LogWarning($"[Trap] No Health component found on {other.name}. Destroying directly.");
            Destroy(other.gameObject);
        }

        // Spawn optional visual effect
        if (triggerEffect != null)
            Instantiate(triggerEffect, transform.position, Quaternion.identity);

        if (destroyOnUse)
        {
            Destroy(gameObject);
            return;
        }

        if (cooldown > 0f)
            StartCoroutine(CooldownRoutine());
    }

    private System.Collections.IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
