using UnityEngine;

public class SimpleZombieAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackDistance = 1.8f;
    public float attackCooldown = 1.2f;
    public int damage = 10;

    private Transform target;
    private float lastAttackTime;

    void Update()
    {
        FindNearestPlayer();

        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > attackDistance)
        {
            Vector3 direction = (targetPos - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            AttackPlayer();
        }

        // force zombie to stay on ground
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }

    void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = player.transform;
            }
        }

        target = closest;
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

        Debug.Log("Zombie attacked " + target.name);
    }
}