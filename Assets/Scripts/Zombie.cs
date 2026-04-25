using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;
    public int damage = 10;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float disengageRange = 2.2f;
    public float attackRate = 1.2f;
    public float attackAnimLockTime = 0.45f;
    public float damageDelay = 0.2f; 
    private float nextAttackTime;
    private bool isAttacking = false;
    private bool inAttackZone = false;
    private bool damageAppliedThisAttack = false;

    [Header("Target")]
    public Transform target;

    [Header("Components")]
    public NavMeshAgent agent;
    public Animator animator;
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip attackClip;
    public AudioClip hurtClip;
    public AudioClip deathClip;
    public AudioClip idleClip;
    public AudioClip footstepClip;

    [Header("Idle Sound")]
    public float idleMinDelay = 4f;
    public float idleMaxDelay = 8f;
    private float nextIdleSoundTime;

    [Header("Footstep Sound")]
    public float footstepRate = 0.5f;
    private float nextFootstepTime;

    [Header("Movement")]
    public float turnSpeed = 10f;
    public float minMoveAnimSpeed = 0.1f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        nextIdleSoundTime = Time.time + Random.Range(idleMinDelay, idleMaxDelay);

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
        }

        if (agent != null)
        {
            if (agent.stoppingDistance >= attackRange)
                agent.stoppingDistance = attackRange - 0.2f;
        }
    }

    void Update()
    {
        if (isDead || target == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        PlayIdleSound();
        PlayFootstepSound();

        if (isAttacking)
        {
            agent.isStopped = true;
            SetMoveAnimation(0f);
            FaceTarget();
            return;
        }

        if (!inAttackZone && distance <= attackRange)
        {
            inAttackZone = true;
        }
        else if (inAttackZone && distance > disengageRange)
        {
            inAttackZone = false;
        }

        if (!inAttackZone)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }

            SetMoveAnimation(agent.velocity.magnitude);
        }
        else
        {
            if (agent.isOnNavMesh)
                agent.isStopped = true;

            SetMoveAnimation(0f);
            FaceTarget();

            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0f;

        if (lookPos.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {
        if (isAttacking || isDead) return;

        isAttacking = true;
        damageAppliedThisAttack = false;

        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("Attack");
        }

        PlayClip(attackClip);

        Invoke(nameof(DealDamage), damageDelay);
        Invoke(nameof(EndAttack), attackAnimLockTime);
    }

    void DealDamage()
    {
        if (isDead || damageAppliedThisAttack) return;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange + 0.2f) return;

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = target.GetComponentInChildren<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = target.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            damageAppliedThisAttack = true;
        }
    }

    void EndAttack()
    {
        if (isDead) return;
        isAttacking = false;
        damageAppliedThisAttack = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        PlayClip(hurtClip);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        isAttacking = false;
        inAttackZone = false;
        CancelInvoke();

        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("Die");
        }

        PlayClip(deathClip);

        if (GameManager.Instance != null)
            GameManager.Instance.AddCoins(1);

        ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();
        if (spawner != null)
            spawner.ZombieKilled();

        Destroy(gameObject, 2f);
    }

    void PlayIdleSound()
    {
        if (isDead || isAttacking) return;
        if (idleClip == null || audioSource == null) return;

        if (Time.time >= nextIdleSoundTime)
        {
            audioSource.PlayOneShot(idleClip);
            nextIdleSoundTime = Time.time + Random.Range(idleMinDelay, idleMaxDelay);
        }
    }

    void PlayFootstepSound()
    {
        if (isDead || isAttacking) return;
        if (footstepClip == null || audioSource == null || agent == null) return;

        if (!agent.isStopped && agent.velocity.magnitude > minMoveAnimSpeed && Time.time >= nextFootstepTime)
        {
            audioSource.PlayOneShot(footstepClip);
            nextFootstepTime = Time.time + footstepRate;
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    void SetMoveAnimation(float speedValue)
    {
        if (animator != null)
            animator.SetFloat("Speed", speedValue);
    }

    public void Setup(int health, float speed, int attackDamage)
    {
        maxHealth = health;
        currentHealth = health;
        damage = attackDamage;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (agent != null)
            agent.speed = speed;
    }
}